using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace AnonymousTypes.Test {
    public class AnonymousTypesProperties {

        [Fact]
        public void anonymous_types_are_reference_types() {
            var a = new { X = 1 };

            IsReferenceType(a);

            // error CS0453: The type '<anonymous type: int X>' must be
            // a non-nullable value type in order to use it
            // as parameter 'T' in the generic type or method 'IsValueType<T>(T)'.
            //
            // IsValueType(a);

            void IsReferenceType<T>(T t)
                where T : class { }

            // disable 'declaration is never used' warning
#pragma warning disable CS8321
            void IsValueType<T>(T t)
                where T : struct { }
#pragma warning restore CS8321
        }

        [Fact]
        public void property_names_are_inferred() {
            var test = new Test();

            var anonymous = new {
                test,
                test.field,
                test.Property,
                ExplicitName = test,

                // error:
                // test.ToString()
            };

            Assert.Same(test, anonymous.test);
            Assert.Equal(test.field, anonymous.field);
            Assert.Equal(test.Property, anonymous.Property);
            Assert.Same(test, anonymous.ExplicitName);
        }

        private class Test {
            public readonly int field = 1;
            public int Property => 2;
            public int Method() => 3;
        }

        [Fact]
        public void anonymous_types_are_immutable() {
            var anonymous = new { X = 1 };

            // error CS0200: Property or indexer
            // '<anonymous type: int X>.X' cannot be assigned to
            // -- it is read only
            //
            // anonymous.X = 100;

            Assert.Equal(1, anonymous.X);
        }

        [Fact]
        public void anonymous_types_provide_ToString_method() {
            var anonymous = new {
                X = 1,
                Y = "foo",
                Z = new {
                    IsGreat = true
                }
            };

            Assert.Equal(
                "{ X = 1, Y = foo, Z = { IsGreat = True } }",
                anonymous.ToString()
            );
        }

        [Fact]
        public void anonymous_types_provide_Equals_and_GetHashCode_methods() {
            var anonymous1 = new {
                X = 1,
                Y = new {
                    A = "foo",
                    B = "bar",
                }
            };

            var anonymous2 = new {
                X = 1,
                Y = new {
                    A = "foo",
                    B = "bar",
                }
            };

            var anonymous3Different = new {
                X = 1,
                Y = new {
                    A = "foozbble", // <-- difference
                    B = "bar"
                }
            };

            Assert.True(anonymous1.Equals(anonymous2));
            Assert.Equal(anonymous1.GetHashCode(), anonymous2.GetHashCode());

            Assert.False(anonymous1.Equals(anonymous3Different));
            Assert.False(anonymous1.Equals(null));
        }

        [Fact]
        public void GetHashCode_depends_on_property_names() {
            var a = new { X = 1, Y = 2 };
            var b = new { foo = 1, bar = 2 };

            Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GetHashCode_depends_on_order_of_properties() {
            var a = new { X = "xxx", Y = "yyy" };
            var b = new { Y = "yyy", X = "xxx" };

            Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void anonymous_types_are_decorated_with_debugger_display_attribute() {
            var anonymous = new { X = 1 };

            var debuggerDisplay = anonymous.GetType()
                .GetCustomAttributes(inherit: true)
                .OfType<DebuggerDisplayAttribute>()
                .Single();

            Assert.Equal("\\{ X = {X} }", debuggerDisplay.Value);
            Assert.Equal("<Anonymous Type>", debuggerDisplay.Type);
        }

        [Fact]
        public void anonymous_types_can_be_created_using_reflection() {
            // Oder of ctor paramters is the same as order of declared fields,
            // in this case ctor(Field1, Field2).
            var anonymous = new {
                Field1 = "foo",
                Field2 = 2
            };

            var type = anonymous.GetType();
            var copy = Activator.CreateInstance(type, new object[] { "foo", 2 });

            Assert.Equal(anonymous, copy);
        }
    }
}