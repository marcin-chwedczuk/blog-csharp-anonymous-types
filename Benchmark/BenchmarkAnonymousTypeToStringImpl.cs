using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmark {
    [CoreJob(baseline: true)]
    public class BenchmarkAnonymousTypeToStringImpl {
        private ClassicToString<string> _refTypeClassic;
        private AnonymousTypeToString<string> _refTypeAnon;

        private ClassicToString<int> _primitiveClassic;
        private AnonymousTypeToString<int> _primitiveAnon;

        private ClassicToString<Point> _valueTypeClassic;
        private AnonymousTypeToString<Point> _valueTypeAnon;

        [GlobalSetup]
        public void Setup() {
            _refTypeClassic = new ClassicToString<string>("foo");
            _refTypeAnon = new AnonymousTypeToString<string>("foo");

            _primitiveClassic = new ClassicToString<int>(345);
            _primitiveAnon = new AnonymousTypeToString<int>(345);

            _valueTypeClassic = new ClassicToString<Point>(new Point(3, 7));
            _valueTypeAnon = new AnonymousTypeToString<Point>(new Point(3, 7));
        }

        [Benchmark]
        public object[] classic_string() => _refTypeClassic.ToStringImpl();

        [Benchmark]
        public object[] anon_string() => _refTypeAnon.ToStringImpl();

        [Benchmark]
        public object[] classic_int() => _primitiveClassic.ToStringImpl();

        [Benchmark]
        public object[] anon_int() => _primitiveAnon.ToStringImpl();

        [Benchmark]
        public object[] classic_point_struct() => _valueTypeClassic.ToStringImpl();

        [Benchmark]
        public object[] anon_point_struct() => _valueTypeAnon.ToStringImpl();

        internal sealed class ClassicToString<T> {
            private readonly T t;

            public ClassicToString(T t) {
                this.t = t;
            }

            public object[] ToStringImpl() {
                var arr = new object[1];

                if (t != null) {
                    arr[0] = t.ToString();
                }
                else {
                    arr[0] = null;
                }

                return arr;
            }
        }

        internal sealed class AnonymousTypeToString<T> {
            private readonly T t;

            public AnonymousTypeToString(T t) {
                this.t = t;
            }

            public object[] ToStringImpl() {
                var arr = new object[1];

                if (default(T) != null) {
                    arr[0] = t.ToString();
                }
                else if (t is object obj && (obj != null)) {
                    arr[0] = obj.ToString();
                }
                else {
                    arr[0] = null;
                }

                return arr;
            }
        }

        public struct Point {
            public int X;
            public int Y;

            public Point(int x, int y) {
                X = x; Y = y;
            }

            public override string ToString()
                => $"(X: {X}, Y: {Y})";
        }
    }
}