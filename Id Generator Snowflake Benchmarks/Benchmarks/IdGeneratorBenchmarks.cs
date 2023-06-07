using BenchmarkDotNet.Attributes;
using Id_Generator_Snowflake;

namespace Id_Generator_Snowflake_Benchmarks.Benchmarks;

public class IdGeneratorBenchmarks
{
    #region Fields
    private IdGenerator? _idGen; // Đối tượng IdGenerator để tạo ID
    #endregion

    #region Properties
    /// <summary>
    /// Số lượng ID cần tạo
    /// </summary>
    [Params(1000, 10000, 100000, 1000000)]
    public int NumberOfIds { get; set; }
    #endregion

    #region Methods
    /// <summary>
    /// Thiết lập ban đầu trước khi chạy bài kiểm tra.
    /// </summary>
    [GlobalSetup]
    public void Setup() => _idGen = new IdGenerator(1, 1);

    /// <summary>
    /// Tạo ID bằng cách gọi phương thức NextId từ đối tượng IdGenerator.
    /// </summary>
    [Benchmark]
    public void GenerateIds()
    {
        for (var i = 0; i < NumberOfIds; i++)
        {
            _ = _idGen?.NextId();
        }
    }
    #endregion
}
