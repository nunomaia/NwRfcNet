namespace NwRfcNet.TypeMapper
{
    /// <summary>
    /// Suported RFC Types on public API
    /// </summary>
    public enum RfcFieldType
    {
        Char = 0,
        Date = 1,
        Bcd = 2,
        Time = 3,
        Byte = 4,
        Table = 5,
        Mum = 6,
        Float = 7,
        Int = 8,
        Structure = 17,
        Decf16 = 23,
        Decf34 = 24,
        String = 29,
        Xstring = 30,
        Int8 = 31,
        UtcLong = 32,
        UtcSecond = 33,
        UtcMinute = 34,
        DtDay = 35,
        DtWeek = 36,
        DtMonth = 37,
        TSecond = 38,
        TMinute = 39,
        Cday = 40,
    }                 
}