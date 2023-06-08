﻿using BenchmarkDotNet.Attributes;
using Id_Generator_Snowflake;
using System.Text;
using static System.Console;
using static System.Threading.Tasks.Parallel;

namespace Id_Generator_Snowflake_Benchmarks.Benchmarks;

public class IdGeneratorBenchmarks
{
    #region Fields
    private IdGenerator? _idGenerator;      // Đối tượng IdGenerator để tạo ID
    private HashSet<ulong>? _generatedIds;  // Danh sách ID đã được tạo
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
    public void Setup()
    {
        _idGenerator = new IdGenerator(0, 0);
        _generatedIds = new HashSet<ulong>();
    }

    /// <summary>
    /// Tạo ID bằng cách gọi phương thức NextId từ đối tượng IdGenerator.
    /// </summary>
    [Benchmark]
    public void GenerateIds() => For(0, NumberOfIds, index =>
    {
        var id = _idGenerator?.NextId() ?? 0;
        if (_generatedIds is not null)
        {
            lock (_generatedIds)
            {
                if (!_generatedIds.Add(id))
                {
                    WriteLine(new StringBuilder().Append("Handle duplicate ID error: ").Append(id).ToString());
                }
            }
        }
    });

    /// <summary>
    /// Dọn dẹp sau khi chạy bài kiểm tra.
    /// </summary>
    [GlobalCleanup]
    public void Cleanup() => WriteLine($"Actual / Expected number of generated IDs: {_generatedIds?.Count} / {NumberOfIds}");
    #endregion
}
