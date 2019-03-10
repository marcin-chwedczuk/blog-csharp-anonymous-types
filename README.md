

# Anonymous types under cover

NOTE: _Due to some problems with upgrading Jekyll, this blog post is exceptionally
presented as GitHub `README.md`._

When you write `new { X = 1 }` in C#, 
the compiler, behind your back, generates this class to represent your anonymous type:
```cs
// Names of generated class, generic parameter, fields, etc. were changed
// to improve readability.
[CompilerGenerated]
[DebuggerDisplay("\\{ X = {X} }", Type = "<Anonymous Type>")]
internal sealed class AnonymousType1<T1> {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly T1 xField;

    public T1 X => xField;

    [DebuggerHidden]
    public AnonymousType1(T1 X) {
        xField = X;
    }

    [DebuggerHidden]
    public override bool Equals(object value) {
        var other = value as AnonymousType1<T1>;
        return other != null &&
            EqualityComparer<T1>.Default.Equals(xField, other.xField);
    }

    [DebuggerHidden]
    public override int GetHashCode() {
        unchecked {
            return 502455694 * -1521134295 +
                EqualityComparer<T1>.Default.GetHashCode(xField);
        }
    }

    [DebuggerHidden]
    public override string ToString() {
        // !!! Reverse engineered from IL - may not be exact:

        var fmt = "{{ X = {0} }}";
        var arr = new object[1];

        // Is T1 a value type?
        if (default(T1) != null) {
            arr[0] = xField.ToString();
        }
        else if (xField is object obj && (obj != null)) {
            arr[0] = obj.ToString();
        }
        else {
            arr[0] = null;
        }

        return string.Format(fmt, arr);
    }
}
```
There are few interesting things to notice here, let's start with the most obvious one:

* The generated class is immutable.
* It is decorated by attributes like `DebuggerDispaly` and `DebuggerHidden` that
 should improve programmers experience while debugging.
* The generated class is both `sealed` and `internal` and is located in the global namespace.
* The generated class is generic. This allows the compiler to reduce code bloat, since a single class
 can be used to represent both `new { X = 1 }` and `new { X = "str" }` anonymous types.
* The compiler provides implementations of `ToString`, `Equals` and `GetHashCode` methods, but
 the generated class does not implement `IEquatable<T>` interface.

The generated `Equals` method is quite obvious. For anonymous type that contains three properties it
looks like this:
```cs
[DebuggerHidden]
public override bool Equals(object value) {
    var other = value as AnonymousType1<T1, T2, T3>;
    return other != null &&
        EqualityComparer<T1>.Default.Equals(field1, other.field1) &&
        EqualityComparer<T2>.Default.Equals(field2, other.field2) &&
        EqualityComparer<T3>.Default.Equals(field3, other.field3);
}
```

The generated `GetHashCode` method is more interesting.
Three property anonymous type contains the following `GetHashCode` method:
```cs
[DebuggerHidden]
public override int GetHashCode() {
    unchecked {
        var tmp = 0xXXXXXXXX 
        tmp = tmp * -1521134295 + EqualityComparer<T1>.Default.GetHashCode(field1);
        tmp = tmp * -1521134295 + EqualityComparer<T2>.Default.GetHashCode(field2);
        tmp = tmp * -1521134295 + EqualityComparer<T3>.Default.GetHashCode(field3);
        return tmp;
    }
}

```
The `0xXXXXXXXX` number is computed at compilation time and is basically a hash of names of anonymous type's properties.
This means that two anonymous types that carry the same values but under different names will have
different hashes:
```cs
var a = new { X   = 1, Y   = 2 };
var b = new { foo = 1, bar = 2 };

Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
```
This also means that order of properties is significant and that changing properties' order also changes the hash:
```cs
var a = new { X = "xxx", Y = "yyy" };
var b = new { Y = "yyy", X = "xxx" };

Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
```

The last method that we will look at is `ToString`. I cannot understand why, but the IL code
for this method is very complicated. I expected that this method would contain a code similar to:
```cs
var str1 = (field1 != null) ? field1.ToString() : null;
```
that converts field values into strings. 
Instead after an embarrassingly long time of "reverse engineering",
I arrived at this C# code (for `string`ing a single field):
```cs
var str1;
if (default(T1) != null) {
    // if T is a value type...
    str1 = field1.ToString();
}
else if (field1 is object obj && (obj != null)) {
    // if T is non-null reference type...
    str1 = obj.ToString();
}
else {
    str1 = null;
}
```
I just couldn't believe that the compiler generated code like that (since `!= null` works perfectly well with generic types.)
I even micro-benchmarked this code against the simpler version, only to find out that they have almost the same performance
(see `BenchmarkAnonymousTypeToStringImpl` class in the attached source code).
As the last act of desperation I looked up the code that actually generates this classes in Roslyn compiler
[AnonymousTypeMethodBodySynthesizer.cs](https://github.com/dotnet/roslyn/blob/14e0068d9d529ade86c0ed1544477a221809d0e0/src/Compilers/CSharp/Portable/Compiler/AnonymousTypeMethodBodySynthesizer.cs)
but it contained no comments explaining why `ToString` is generated in such a way (self documenting code anyone?)
I also coudn't find anything useful in `docs` folder. Anyway if you know why it is implemented this way (is really `default(T) != null` the fastest
way to check if `T` is a value type?) please send me an email or create a pull request with an explanation to this repo.

OK, thats all for today. If you like this post please :+1: star :+1:
Also if you have a topic that you would like to read about, please send me an email.


## References:

* https://sharplab.io - To get IL code for `new { X = 1 }` anonymous type

