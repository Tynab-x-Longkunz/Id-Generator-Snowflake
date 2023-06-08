using static System.Console;

#if DEBUG
using Id_Generator_Snowflake;
using System.Diagnostics;
using System.Text;
using static System.Threading.Tasks.Parallel;

var idGen = new IdGenerator(0, 0);
var genIds = new HashSet<ulong>();
var numIds = 1_000_000;
var top = 10;

var sw = new Stopwatch();
sw.Start();
For(0, numIds, index =>
{
    var id = idGen.NextId();
    lock (genIds)
    {
        if (!genIds.Add(id))
        {
            WriteLine(new StringBuilder().Append("Handle duplicate ID error: ").Append(id).ToString());
        }
    }
});
sw.Stop();

var idsCnt = genIds.Count;
if (numIds != idsCnt)
{
    WriteLine($"Handle mismatch between expected and actual number of generated IDs: {idsCnt} / {numIds}");
}

WriteLine($"Time: {sw.ElapsedMilliseconds:#,#} ms\n");

WriteLine($"Top {top:#,#} of {numIds:#,#} IDs:");
foreach (var id in genIds.Take(top))
{
    WriteLine(id);
}
#endif

#if RELEASE
using Id_Generator_Snowflake_Benchmarks.Benchmarks;
using static BenchmarkDotNet.Running.BenchmarkRunner;

_ = Run<IdGeneratorBenchmarks>();
#endif

_ = ReadLine();
