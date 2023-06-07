namespace Id_Generator_Snowflake.Common;

internal static class Constant
{
    internal const int WKR_ID_BITS = 3; // Số bit được sử dụng để lưu trữ Worker Id
    internal const int DC_ID_BITS = 2;  // Số bit được sử dụng để lưu trữ Datacenter Id
    internal const int SEQ_BITS = 8;    // Số bit được sử dụng để lưu trữ Sequence

    internal const ulong MAX_WKR_ID = (1 << WKR_ID_BITS) - 1;   // Giá trị tối đa của Worker Id
    internal const ulong MAX_DC_ID = (1 << DC_ID_BITS) - 1;     // Giá trị tối đa của Datacenter Id
    internal const ulong SEQ_MASK = (1 << SEQ_BITS) - 1;        // Mặt nạ để lấy các bit của Sequence
    internal const int WKR_ID_SHFT = SEQ_BITS;                  // Số bit cần dịch trái để lưu trữ Worker Id
    internal const int DC_ID_SHFT = SEQ_BITS + WKR_ID_BITS;     // Số bit cần dịch trái để lưu trữ Datacenter Id
}
