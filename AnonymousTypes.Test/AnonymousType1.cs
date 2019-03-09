
// Declared outside any namespace

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// new { X = 1 } will be transalated by comiler to:

// The exact transaltion algorithm is described,
// e.g. in comments in this file:
// https://github.com/dotnet/roslyn/blob/14e0068d9d529ade86c0ed1544477a221809d0e0/src/Compilers/CSharp/Portable/Compiler/AnonymousTypeMethodBodySynthesizer.cs

[CompilerGenerated]
[DebuggerDisplay("\\{ X = {X} }", Type = "<Anonymous Type>")]
internal sealed class AnonymousType1<T1> {

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly T1 xField;

    public T1 X {
        get {
            return xField;
        }
    }

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
            // The first part of the hash (502455694)
            // depends on the names of the anonymous'
            // type properties!
            return 502455694 * -1521134295 +
                EqualityComparer<T1>.Default.GetHashCode(xField);
        }
    }

    [DebuggerHidden]
    public override string ToString() {
        var fmt = "{{ X = {0} }}";
        var arr = new object[1];

        if (default(T1) != null) {
            // if T is a value type...
            arr[0] = xField.ToString();
        }
        else if (xField is object obj && (obj != null)) {
            // if T is non-null reference type...
            arr[0] = obj.ToString();
        }
        else {
            arr[0] = null;
        }

        return string.Format(fmt, arr);
    }
}