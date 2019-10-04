using NwRfcNet.Interop;
using System;

namespace NwRfcNet.RfcTypes
{
    public class RfcString : IRfcType<string>
    {
        public RfcString(string value) => RfcValue = value;

        public RfcString(char[] value) => RfcValue = new string(value);

        public RfcString(char[] value, int startIndex, int length) => RfcValue = new string(value, startIndex, length);

        public String RfcValue { get; }

        #region Interop

        /// <summary>
        /// sets the value of a RFC STRING field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        internal void SetFieldValue(IntPtr dataHandle, string name)
        {
            var rc = RfcInterop.RfcSetString(dataHandle, name, RfcValue, (uint) RfcValue.Length, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }

        /// <summary>
        /// Gets the value of a RFC STRING field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        /// <returns></returns>
        internal static RfcString GetFieldValue(IntPtr dataHandle, string name)
        {
            char[] buffer = new char[1024];
            var rc = RfcInterop.RfcGetString(dataHandle, name, buffer, (uint)buffer.Length, out var bufferLength, out var errorInfo);

            if (rc == RfcInterop.RFC_RC.RFC_BUFFER_TOO_SMALL)
            {
                buffer = new char[bufferLength];
                rc = RfcInterop.RfcGetString(dataHandle, name, buffer, (uint)buffer.Length, out bufferLength, out errorInfo);
            }

            rc.OnErrorThrowException(errorInfo);
            return new RfcString(buffer, 0, (int) bufferLength);

        }
        #endregion 
    }
}
