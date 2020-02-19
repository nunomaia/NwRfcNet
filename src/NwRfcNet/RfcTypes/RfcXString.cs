﻿using NwRfcNet.Interop;
using System;

namespace NwRfcNet.RfcTypes
{
    public class RfcXString : IRfcType<byte[]>
    {
        public RfcXString(byte[] value) => RfcValue = value;

        // public RfcXString(byte[] value, int startIndex, int length) => RfcValue = new byte[length](value, startIndex, length);

        public byte[] RfcValue { get; }

        #region Interop

        /// <summary>
        /// sets the value of a RFC STRING field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        internal void SetFieldValue(IntPtr dataHandle, string name)
        {
            var rc = RfcInterop.RfcSetXString(dataHandle, name, RfcValue, (uint) RfcValue.Length, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }

        /// <summary>
        /// Gets the value of a RFC STRING field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        /// <returns></returns>
        internal static RfcXString GetFieldValue(IntPtr dataHandle, string name)
        {
            byte[] buffer = new byte[1];
            var rc = RfcInterop.RfcGetXString(dataHandle, name, buffer, (uint)buffer.Length, out var bufferLength, out var errorInfo);

            if (rc == RfcInterop.RFC_RC.RFC_BUFFER_TOO_SMALL)
            {
                buffer = new byte[bufferLength];
                rc = RfcInterop.RfcGetXString(dataHandle, name, buffer, (uint)buffer.Length, out bufferLength, out errorInfo);
            }

            rc.OnErrorThrowException(errorInfo);
            return new RfcXString(buffer);

        }
        #endregion 
    }
}
