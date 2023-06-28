using static Id_Generator_Snowflake.Common.Constant;
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

    /// <summary>
    /// Convert a numeric value to an alphabetic representation.
    /// </summary>
    /// <param name="value">The numeric value to convert.</param>
    /// <returns>The alphabetic representation of the numeric value.</returns>
    internal static string ConvertToBaseAlphabetic(this long value)
    {
        var rslt = string.Empty;

        do
        {
            rslt = BASE_AL_CHARS[(int)(value % BASE_AL_CHARS_LEN)] + rslt;
            value /= BASE_AL_CHARS_LEN;
        } while (value > 0);

        return rslt;
    }

    /// <summary>
    /// Convert an alphabetic value to its numeric representation.
    /// </summary>
    /// <param name="value">The alphabetic value to convert.</param>
    /// <returns>The numeric representation of the alphabetic value.</returns>
    internal static long ConvertFromBaseAlphabetic(this string value)
    {
        var rslt = 0L;

        for (var i = 0; i < value.Length; i++)
        {
            rslt = rslt * BASE_AL_CHARS_LEN + BASE_AL_CHARS.IndexOf(value[i]);
        }

        return rslt;
    }
}
