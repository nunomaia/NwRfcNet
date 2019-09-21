using NwRfcNet.Interop;
using NwRfcNet.Util;
using System;

namespace NwRfcNet.RfcTypes
{
    /// <summary>
    /// Represents an RFC INT8.
    /// </summary>
    public class RfcInt8
    {
        #region Constructors

        /// <summary>
        /// Creates a new RfcInt8
        /// </summary>
        /// <param name="value"></param>
        public RfcInt8(Int64 value) => RfcValue = value;

        #endregion  

        #region Properties

        /// <summary>
        /// Date
        /// </summary>
        public Int64 RfcValue { get;  }

        #endregion

        #region Interop

        /// <summary>
        /// sets the value of a RFC INT8 field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        internal void SetFieldValue(IntPtr dataHandle, string name)
        {
            var rc = RfcInterop.RfcSetInt8(dataHandle, name, RfcValue, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }

        /// <summary>
        /// Gets the value of a RFC INT8 field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        /// <returns></returns>
        internal static RfcInt8 GetFieldValue(IntPtr dataHandle, string name)
        {
            long value = 0;
            var rc = RfcInterop.RfcGetInt8(dataHandle, name, ref value, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
            return new RfcInt8(value);
        }
        #endregion
    }
}
