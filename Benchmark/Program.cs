using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Running;

namespace Benchmark {
    class Program {
        static void Main(string[] args) {
            // run: dotnet run -c Release
            var summary = BenchmarkRunner.Run<BenchmarkAnonymousTypeToStringImpl>();
        }

        /*
        Example results:
        |               Method |      Mean |     Error |    StdDev | Ratio |
        |--------------------- |----------:|----------:|----------:|------:|
        |       classic_string |  10.63 ns | 0.0958 ns | 0.0896 ns |  1.00 |
        |                      |           |           |           |       |
        |          anon_string |  10.44 ns | 0.0325 ns | 0.0288 ns |  1.00 |
        |                      |           |           |           |       |
        |          classic_int |  31.38 ns | 0.2193 ns | 0.2051 ns |  1.00 |
        |                      |           |           |           |       |
        |             anon_int |  30.40 ns | 0.1655 ns | 0.1382 ns |  1.00 |
        |                      |           |           |           |       |
        | classic_point_struct | 199.74 ns | 0.9277 ns | 0.8224 ns |  1.00 |
        |                      |           |           |           |       |
        |    anon_point_struct | 202.54 ns | 2.5952 ns | 2.4276 ns |  1.00 |
        */
    }
}
