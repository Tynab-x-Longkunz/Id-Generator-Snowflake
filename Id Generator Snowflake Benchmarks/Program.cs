using Id_Generator_Snowflake_Benchmarks.Benchmarks;
using static BenchmarkDotNet.Running.BenchmarkRunner;
using static System.Console;

_ = Run<IdGeneratorBenchmarks>();
_ = ReadLine();