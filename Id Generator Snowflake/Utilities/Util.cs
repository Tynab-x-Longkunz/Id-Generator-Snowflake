using static System.DateTimeOffset;

namespace Id_Generator_Snowflake.Utilities;

internal static class Util
{
    /// <summary>
    /// Get the current timestamp.
    /// </summary>
    internal static ulong TimeGen() => (ulong)UtcNow.ToUnixTimeMilliseconds();

    /// <summary>
    /// Find the next timestamp greater than the last timestamp.
    /// </summary>
    internal static ulong TilNextMillis(this ulong lastTimestamp)
    {
        var ts = TimeGen();

        while (ts <= lastTimestamp)
        {
            ts = TimeGen();
        }

        return ts;
    }
}
