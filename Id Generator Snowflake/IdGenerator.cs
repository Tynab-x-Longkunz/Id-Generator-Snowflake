using System.Text;
using static Id_Generator_Snowflake.Common.Constant;
using static Id_Generator_Snowflake.Properties.Resources;
using static Id_Generator_Snowflake.Utilities.Util;
using static System.DateTimeOffset;

namespace Id_Generator_Snowflake;

public class IdGenerator
{
    #region Fields
    private ulong _lastTimestamp = default; // Last timestamp recorded
    private readonly object _lock = new();  // Lock object for access synchronization

    /// <summary>
    /// Epoch timestamp (used as an offset for the timestamp)
    /// </summary>
    public const ulong Twepoch = 1_672_531_200_000; // 01/012023 00:00:00

    /// <summary>
    /// Number of bits to shift left to store the timestamp
    /// </summary>
    public const int TimestampLeftShift = SEQ_BITS + WKR_ID_BITS + DC_ID_BITS;
    #endregion

    #region Properties
    /// <summary>
    /// Worker Id of the IdGenerator
    /// </summary>
    public ulong WorkerId { get; protected set; }

    /// <summary>
    /// Datacenter Id của IdGenerator
    /// </summary>
    public ulong DatacenterId { get; protected set; }

    /// <summary>
    /// Current sequence
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
    /// Generate and return a new Id.
    /// </summary>
    /// <returns>A newly generated Id.</returns>
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

    ///<summary>
    /// Parse an Id and return its components.
    ///</summary>
    /// <param name="id">The Id to be parsed.</param>
    /// <returns>
    /// A tuple consisting of three elements:
    /// - DateTime: Represents the time corresponding to the Id.
    /// - ulong: WorkerId of the Id.
    /// - ulong: DatacenterId of the Id.
    ///</returns>
    public static (DateTime, ulong, ulong) ParseId(ulong id)
    {
        var datacenterId = (id >> DC_ID_SHFT) & ((1UL << DC_ID_BITS) - 1);
        var workerId = (id >> WKR_ID_SHFT) & ((1UL << WKR_ID_BITS) - 1);
        var timestamp = (id >> TimestampLeftShift) + Twepoch;
        var dateTime = FromUnixTimeMilliseconds((long)timestamp).UtcDateTime;

        return (dateTime, workerId, datacenterId);
    }
    #endregion
}
