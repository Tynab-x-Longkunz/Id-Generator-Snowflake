using static System.Console;

#if DEBUG
using Id_Generator_Snowflake;
using System.Diagnostics;
using System.Text;
using static System.Threading.Tasks.Parallel;

var idGen = new IdGenerator(0, 0);
var genIds = new HashSet<ulong>();
var numIds = 1000000;

var sw = new Stopwatch();
sw.Start();
For(0, numIds, index =>
{
    var id = idGen?.NextId() ?? 0;
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
    WriteLine(new StringBuilder().Append("Handle mismatch between expected and actual number of generated IDs: ").Append(idsCnt).Append('/').Append(numIds).ToString());
}

WriteLine($"Time: {sw.ElapsedMilliseconds} ms");
#endif

#if RELEASE
using Id_Generator_Snowflake_Benchmarks.Benchmarks;
using static BenchmarkDotNet.Running.BenchmarkRunner;

_ = Run<IdGeneratorBenchmarks>();
#endif

_ = ReadLine();
