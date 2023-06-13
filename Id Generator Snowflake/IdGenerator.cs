using System.Text;
using static Id_Generator_Snowflake.Common.Constant;
using static Id_Generator_Snowflake.Properties.Resources;
using static Id_Generator_Snowflake.Utilities.Util;

namespace Id_Generator_Snowflake;

public class IdGenerator
{
    #region Fields
    private ulong _lastTimestamp = default; // Thời điểm timestamp cuối cùng
    private readonly object _lock = new();  // Đối tượng lock để đồng bộ hóa truy cập

    /// <summary>
    /// Thời điểm Epoch (được sử dụng làm offset cho timestamp)
    /// </summary>
    public const ulong Twepoch = 1_672_531_200_000; // 01/012023 00:00:00

    /// <summary>
    /// Số bit cần dịch trái để lưu trữ timestamp
    /// </summary>
    public const int TimestampLeftShift = SEQ_BITS + WKR_ID_BITS + DC_ID_BITS;
    #endregion

    #region Properties
    /// <summary>
    /// Worker Id của IdGenerator
    /// </summary>
    public ulong WorkerId { get; protected set; }

    /// <summary>
    /// Datacenter Id của IdGenerator
    /// </summary>
    public ulong DatacenterId { get; protected set; }

    /// <summary>
    /// Sequence hiện tại
    /// </summary>
    public ulong Sequence { get; internal set; }
    #endregion

    #region Constructors
    public IdGenerator(ulong workerId, ulong datacenterId, ulong sequence = default)
    {
        if (workerId > MAX_WKR_ID)
        {
            throw new ArgumentException(new StringBuilder().Append(wkr_id_exc_pfx).Append(MAX_WKR_ID).Append(exc_sfx).ToString());
        }

        if (datacenterId > MAX_DC_ID)
        {
            throw new ArgumentException(new StringBuilder().Append(dc_id_exc_pfx).Append(MAX_DC_ID).Append(exc_sfx).ToString());
        }

        WorkerId = workerId;
        DatacenterId = datacenterId;
        Sequence = sequence;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Tạo và trả về một Id mới.
    /// </summary>
    /// <returns>Một Id mới được tạo.</returns>
    public ulong NextId()
    {
        lock (_lock)
        {
            var ts = TimeGen();

            if (ts < _lastTimestamp)
            {
                throw new Exception(ts_exc);
            }
            else if (ts == _lastTimestamp)
            {
                Sequence = (Sequence + 1) & SEQ_MASK;

                if (Sequence is 0)
                {
                    ts = _lastTimestamp.TilNextMillis();
                }
            }
            else
            {
                Sequence = default;
            }

            _lastTimestamp = ts;

            return ((ts - Twepoch) << TimestampLeftShift) | (DatacenterId << DC_ID_SHFT) | (WorkerId << WKR_ID_SHFT) | (Sequence & SEQ_MASK);
        }
    }

    /// <summary>
    /// Calculate data from id generated
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    public (DateTime, ulong, ulong) ParseId(ulong id)
    {
        var DatacenterId = (id >> DC_ID_SHFT) & ((1UL << DC_ID_BITS) - 1);
        var WorkerId = (id >> WKR_ID_SHFT) & ((1UL << WKR_ID_BITS) - 1);
        ulong timestamp = ((id >> TimestampLeftShift) + Twepoch);
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)timestamp);
        DateTime dateTime = dateTimeOffset.UtcDateTime;
        return (dateTime, WorkerId, DatacenterId);
    }
    #endregion
}
