namespace NwRfcNet.Bapi
{
    public class BapiReturn
    {
        /// <summary>
        /// Message Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Message Type
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Message Text
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Application log: log number
        /// </summary>
        public string LogNo { get; set; }

        /// <summary>
        /// Internal message serial number
        /// </summary>
        public string LogMessageNumber { get; set; }

        /// <summary>
        /// Message Variable 1
        /// </summary>
        public string MessageV1 { get; set; }

        /// <summary>
        /// Message Variable 2
        /// </summary>
        public string MessageV2 { get; set; }

        /// <summary>
        /// Message Variable 3
        /// </summary>
        public string MessageV3 { get; set; }

        /// <summary>
        /// Message Variable 4
        /// </summary>
        public string MessageV4 { get; set; }
    }
}
