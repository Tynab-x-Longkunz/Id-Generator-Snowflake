using BenchmarkDotNet.Attributes;
using Id_Generator_Snowflake;
using System.Text;
using static System.Console;
using static System.Threading.Tasks.Parallel;

namespace Id_Generator_Snowflake_Benchmarks.Benchmarks;

public class IdGeneratorBenchmarks
{
    #region Fields
    private IdGenerator? _idGenerator;      // The IdGenerator object for generating Ids
    private HashSet<long>? _generatedIds;   // The list of generated Ids
    #endregion

    #region Properties
    /// <summary>
    /// The number of Ids to generate.
    /// </summary>
    [Params(1_000, 10_000, 100_000, 1_000_000)]
    public int Size { get; set; }
    #endregion

    #region Methods
    /// <summary>
    /// Initial setup before running the benchmark.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _idGenerator = new IdGenerator(0, 0);
        _generatedIds = new HashSet<long>();
    }

    /// <summary>
    /// Generate Ids by calling the NextId method from the IdGenerator object.
    /// </summary>
    [Benchmark]
    public void GenerateIds() => For(0, Size, index =>
    {
        var id = _idGenerator?.NextId() ?? 0;
        lock (_generatedIds!)
        {
            if (!_generatedIds.Add(id))
            {
                WriteLine(new StringBuilder().Append("Handle duplicate Id error: ").Append(id).ToString());
            }
        }
    });
    #endregion
}
