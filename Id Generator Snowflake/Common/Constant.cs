namespace Id_Generator_Snowflake.Common;

internal readonly struct Constant
{
    internal const int WKR_ID_BITS = 5; // Number of bits used to store the Worker Id
    internal const int DC_ID_BITS = 5;  // Number of bits used to store the Datacenter Id
    internal const int SEQ_BITS = 13;   // Number of bits used to store the Sequence

    internal const long MAX_WKR_ID = -1 ^ (-1 << WKR_ID_BITS);  // Maximum value of the Worker Id
    internal const long MAX_DC_ID = -1 ^ (-1 << DC_ID_BITS);    // Maximum value of the Datacenter Id
    internal const long MAX_SEQ = -1 ^ (-1 << SEQ_BITS);        // Maximum value of the Sequence

    internal const int WKR_ID_SHFT = SEQ_BITS;                              // Number of bits to shift left to store the Worker Id
    internal const int DC_ID_SHFT = SEQ_BITS + WKR_ID_BITS;                 // Number of bits to shift left to store the Datacenter Id
    internal const int TS_LEFT_SHFT = SEQ_BITS + WKR_ID_BITS + DC_ID_BITS;  // Number of bits to shift left to store the Timestamp

    // 01-01-2023 00:00:00
    internal const long TS_EPOCH = 1_672_531_200_000; // Unix timestamp representing the custom epoch for Snowflake Ids

    internal const string BASE_AL_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";   // Alphanumeric characters used for base conversion
    internal static readonly int BASE_AL_CHARS_LEN = BASE_AL_CHARS.Length;          // Length of the alphanumeric character set used for base conversion
}
