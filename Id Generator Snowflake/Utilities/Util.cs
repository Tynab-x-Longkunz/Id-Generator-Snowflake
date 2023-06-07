using static System.DateTimeOffset;

namespace Id_Generator_Snowflake.Utilities;

internal static class Util
{
    /// <summary>
    /// Lấy thời điểm timestamp hiện tại.
    /// </summary>
    internal static ulong TimeGen() => (ulong)UtcNow.ToUnixTimeMilliseconds();

    /// <summary>
    /// Tìm thời điểm timestamp kế tiếp lớn hơn hoặc bằng thời điểm cuối cùng.
    /// </summary>
    internal static ulong TilNextMillis(this ulong lastTimestamp)
    {
        var timestamp = TimeGen();

        while (timestamp <= lastTimestamp)
        {
            timestamp = TimeGen();
        }

        return timestamp;
    }
}
