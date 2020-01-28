namespace NwRfcNet.Bapi
{
    public static class BapiMessageType
    {
        public static readonly string Success = "S";

        public static readonly string Error = "E";

        public static readonly string Warning = "W";

        public static readonly string Information = "I";

        public static readonly string Abort = "A";
    }

    public static class BapiMessageTypeExtentions
    {
        public static bool IsError(this BapiReturn bapiReturn)
            => bapiReturn.MessageType == BapiMessageType.Error || bapiReturn.MessageType == BapiMessageType.Abort;
    }
}
