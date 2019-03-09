using System;
using Xunit;

public class AnonymousType1Test {

    [Fact]
    public void to_string_works() {
        Assert.Equal(
            "{ X = 3 }",
            new AnonymousType1<int>(3).ToString()
        );

        Assert.Equal(
            "{ X =  }",
            new AnonymousType1<int?>(null).ToString()
        );

        Assert.Equal(
            "{ X =  }",
            new AnonymousType1<string>(null).ToString()
        );

        Assert.Equal(
            "{ X = foo }",
            new AnonymousType1<string>("foo").ToString()
        );
    }

    [Fact]
    public void equals_works() {
        var t1 = new AnonymousType1<string>("t1");
        var t2 = new AnonymousType1<string>("t1");
        var t3 = new AnonymousType1<string>("different");

        Assert.True(t1.Equals(t2));
        Assert.Equal(t1.GetHashCode(), t2.GetHashCode());

        Assert.False(t1.Equals(null));
        Assert.False(t1.Equals(t3));
    }
}