namespace Id_Generator_Snowflake.Common;

internal static class Constant
{
    internal const int WKR_ID_BITS = 3; // Number of bits used to store the Worker Id
    internal const int DC_ID_BITS = 2;  // Number of bits used to store the Datacenter Id
    internal const int SEQ_BITS = 8;    // Number of bits used to store the Sequence

    internal const ulong MAX_WKR_ID = (1 << WKR_ID_BITS) - 1;   // Maximum value of the Worker Id
    internal const ulong MAX_DC_ID = (1 << DC_ID_BITS) - 1;     // Maximum value of the Datacenter Id

    internal const ulong SEQ_MASK = (1 << SEQ_BITS) - 1;    // Mask to retrieve the bits of the Sequence

    internal const int WKR_ID_SHFT = SEQ_BITS;              // Number of bits to shift left to store the Worker Id
    internal const int DC_ID_SHFT = SEQ_BITS + WKR_ID_BITS; // Number of bits to shift left to store the Datacenter Id
}
