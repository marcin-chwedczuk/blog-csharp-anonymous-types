``` ini

BenchmarkDotNet=v0.11.4, OS=ubuntu 16.04
Intel Core i7-4771 CPU 3.50GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.104
  [Host] : .NET Core 2.2.2 (CoreCLR 4.6.27317.07, CoreFX 4.6.27318.02), 64bit RyuJIT
  Core   : .NET Core 2.2.2 (CoreCLR 4.6.27317.07, CoreFX 4.6.27318.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
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
