﻿using static System.Console;

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
WriteLine($"WorkerID (setup): {wkrId} - DatacenterID (setup): {dcId}");
var idGen = new IdGenerator(wkrId, dcId);
var genIds = new HashSet<long>();
var numIds = 100;
var top = 10;
var flow = 1;

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

            break;
        }
    case 2:
        {
            for (var i = 0; i < numIds; i++)
            {
                var id = idGen.NextId();

                Sleep(100);

                lock (genIds)
                {
                    if (!genIds.Add(id))
                    {
                        WriteLine(new StringBuilder().Append("Handle duplicate ID error: ").Append(id).ToString());
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

WriteLine($"Top {top:#,#} of {numIds:#,#} IDs:");

foreach (var id in genIds.Take(top))
{
    var tupl = ExtractIdComponents(id);
    WriteLine($"ID (generated): {id} - Time (extracted): {tupl.Item1} - WorkerID (extracted): {tupl.Item2} - DatacenterID (extracted): {tupl.Item3}");
}
#endif

#if RELEASE
using Id_Generator_Snowflake_Benchmarks.Benchmarks;
using static BenchmarkDotNet.Running.BenchmarkRunner;

_ = Run<IdGeneratorBenchmarks>();
#endif

_ = ReadLine();
