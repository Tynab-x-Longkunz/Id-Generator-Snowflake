using static System.Console;

#if DEBUG
using Id_Generator_Snowflake;
using System.Diagnostics;
using System.Text;
using static Id_Generator_Snowflake.IdGenerator;
using static System.Threading.Tasks.Parallel;
using static System.Threading.Thread;
using static YANLib.YANNum;

var wkrId = GenerateRandomLong(0, 31);
var dcId = GenerateRandomLong(0, 31);
WriteLine($"WorkerId (setup): {wkrId} - DatacenterId (setup): {dcId}");
var idGen = new IdGenerator(wkrId, dcId);
var genIds = new HashSet<object>();
var numIds = 100;
var top = 10;
var flow = 2;

switch (flow)
{
    case 1:
        {
            numIds = 1000000;

            var sw = new Stopwatch();

            sw.Start();

            For(0, numIds, index =>
            {
                var id = idGen.NextId();

                lock (genIds)
                {
                    if (!genIds.Add(id))
                    {
                        WriteLine(new StringBuilder().Append("Handle duplicate Id error: ").Append(id).ToString());
                    }
                }
            });

            sw.Stop();

            var idsCnt = genIds.Count;

            if (numIds != idsCnt)
            {
                WriteLine($"Handle mismatch between expected and actual number of generated Ids: {idsCnt} / {numIds}");
            }

            WriteLine($"Time: {sw.ElapsedMilliseconds:#,#} ms\n");

            break;
        }
    case 2:
        {
            numIds = 1000000;

            var sw = new Stopwatch();

            sw.Start();

            For(0, numIds, index =>
            {
                var id = idGen.NextIdAlphabetic();

                lock (genIds)
                {
                    if (!genIds.Add(id))
                    {
                        WriteLine(new StringBuilder().Append("Handle duplicate Id error: ").Append(id).ToString());
                    }
                }
            });

            sw.Stop();

            var idsCnt = genIds.Count;

            if (numIds != idsCnt)
            {
                WriteLine($"Handle mismatch between expected and actual number of generated Ids: {idsCnt} / {numIds}");
            }

            WriteLine($"Time: {sw.ElapsedMilliseconds:#,#} ms\n");

            break;
        }
    case 3:
        {
            for (var i = 0; i < numIds; i++)
            {
                var id = idGen.NextId();

                Sleep(100);

                lock (genIds)
                {
                    if (!genIds.Add(id))
                    {
                        WriteLine(new StringBuilder().Append("Handle duplicate Id error: ").Append(id).ToString());
                    }
                }
            }

            break;
        }
    default:
        {
            break;
        }
}

WriteLine($"Top {top:#,#} of {numIds:#,#} Ids:");

foreach (var id in genIds.Take(top))
{
    (DateTime, long, long) tupl;
    switch (flow)
    {
        case 2:
            {
                tupl = ExtractIdAlphabeticComponents((string)id);
                break;
            }
        default:
            {
                tupl = ExtractIdComponents((long)id);
                break;
            }
    }
    WriteLine($"Id (generated): {id} - Time (extracted): {tupl.Item1} - WorkerId (extracted): {tupl.Item2} - DatacenterId (extracted): {tupl.Item3}");
}
#endif

#if RELEASE
using Id_Generator_Snowflake_Benchmarks.Benchmarks;
using static BenchmarkDotNet.Running.BenchmarkRunner;

_ = Run<IdGeneratorBenchmarks>();
#endif

_ = ReadLine();
