using static System.DateTimeOffset;

namespace Id_Generator_Snowflake.Utilities;

internal static class Util
{
    /// <summary>
    /// Get the current timestamp.
    /// </summary>
    internal static long TimeGen() => UtcNow.ToUnixTimeMilliseconds();

    /// <summary>
    /// Find the next timestamp greater than the last timestamp.
    /// </summary>
    internal static long TilNextMillis(this long lastTimestamp)
    {
        var curTs = TimeGen();

        while (curTs <= lastTimestamp)
        {
            curTs = TimeGen();
        }

        return curTs;
    }
}
