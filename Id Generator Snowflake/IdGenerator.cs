using System.Text;
using static Id_Generator_Snowflake.Common.Constant;
using static Id_Generator_Snowflake.Properties.Resources;
using static Id_Generator_Snowflake.Utilities.Util;
using static System.DateTimeOffset;

namespace Id_Generator_Snowflake;

public class IdGenerator
{
    #region Fields
    private long _lastTimestamp = -1;       // Last timestamp recorded
    private readonly object _lock = new();  // Lock object for access synchronization
    #endregion

    #region Properties
    /// <summary>
    /// Worker Id of the IdGenerator
    /// </summary>
    public long WorkerId { get; protected set; }

    /// <summary>
    /// Datacenter Id của IdGenerator
    /// </summary>
    public long DatacenterId { get; protected set; }

    /// <summary>
    /// Current sequence
    /// </summary>
    public long Sequence { get; internal set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the IdGenerator class with the specified worker Id, datacenter Id, and sequence.
    /// </summary>
    /// <param name="workerId">The Id of the worker.</param>
    /// <param name="datacenterId">The Id of the datacenter.</param>
    /// <param name="sequence">The initial sequence number.</param>
    /// <exception cref="ArgumentException">Thrown when the worker Id or datacenter Id is invalid.</exception>
    public IdGenerator(long workerId, long datacenterId, long sequence = default)
    {
        if (workerId is < 0 or > MAX_WKR_ID)
        {
            throw new ArgumentException(new StringBuilder().Append(wkr_id_exc_pfx).Append(MAX_WKR_ID).Append(exc_sfx).ToString());
        }

        if (datacenterId is < 0 or > MAX_DC_ID)
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
    public long NextId()
    {
        lock (_lock)
        {
            var curTs = TimeGen();

            if (curTs < _lastTimestamp)
            {
                throw new Exception(ts_exc);
            }
            else if (curTs == _lastTimestamp)
            {
                Sequence = (Sequence + 1) & MAX_SEQ;

                if (Sequence is 0)
                {
                    curTs = _lastTimestamp.TilNextMillis();
                }
            }
            else
            {
                Sequence = default;
            }

            _lastTimestamp = curTs;

            return ((curTs - TS_EPOCH) << TS_LEFT_SHFT) | (DatacenterId << DC_ID_SHFT) | (WorkerId << WKR_ID_SHFT) | (Sequence & MAX_SEQ);
        }
    }

    /// <summary>
    /// Generate and return a new alphabetic Id.
    /// </summary>
    /// <returns>A newly generated alphabetic Id.</returns>
    public string NextIdAlphabetic() => NextId().ConvertToBaseAlphabetic();

    /// <summary>
    /// Extracts the timestamp, worker Id, and datacenter Id components from a Snowflake Id.
    /// </summary>
    /// <param name="id">The Snowflake Id to extract components from.</param>
    /// <returns>A tuple containing the extracted timestamp, worker Id, and datacenter Id.</returns>
    public static (DateTime, long, long) ExtractIdComponents(long id) => (FromUnixTimeMilliseconds((id >> TS_LEFT_SHFT) + TS_EPOCH).UtcDateTime, (id >> WKR_ID_SHFT) & ((1 << WKR_ID_BITS) - 1), (id >> DC_ID_SHFT) & ((1 << DC_ID_BITS) - 1));

    /// <summary>
    /// Extracts the timestamp, worker Id, and datacenter Id components from an alphabetic Snowflake Id.
    /// </summary>
    /// <param name="id">The alphabetic Snowflake Id to extract components from.</param>
    /// <returns>A tuple containing the extracted timestamp, worker Id, and datacenter Id.</returns>
    public static (DateTime, long, long) ExtractIdAlphabeticComponents(string id) => ExtractIdComponents(id.ConvertFromBaseAlphabetic());
    #endregion
}
