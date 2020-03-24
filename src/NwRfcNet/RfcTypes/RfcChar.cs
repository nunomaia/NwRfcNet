using NwRfcNet.Interop;
using System;

namespace NwRfcNet.RfcTypes
{
    public class RfcChar : IRfcType<string>
    {
        public RfcChar(char[] buffer, int maxSize)
        {
            if (maxSize > buffer.Length)
                RfcValue = new String(buffer).PadRight(maxSize - buffer.Length);
            else
                RfcValue = new String(buffer);
        }

        public string RfcValue { get; }

        #region Interop

        /// <summary>
        /// sets the value of a RFC STRING field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        internal void SetFieldValue(IntPtr dataHandle, string name)
        {
            var buffer = RfcValue.ToCharArray();
            var rc = RfcInterop.RfcSetChars(dataHandle, name, buffer, (uint) buffer.Length, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }

        /// <summary>
        /// Gets the value of a RFC STRING field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        /// <returns></returns>
        internal static RfcChar GetFieldValue(IntPtr dataHandle, string name, int length)
        {
            char[] buffer = new char[length];
            var rc = RfcInterop.RfcGetChars(dataHandle, name, buffer, (uint)buffer.Length, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
            return new RfcChar(buffer, buffer.Length);

        }
        #endregion 
    }
}
