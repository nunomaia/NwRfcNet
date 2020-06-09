using System;
using static NwRfcNet.Interop.RfcInterop;

namespace NwRfcNet
{
    internal static class RfcInteropExtentions
    {
        /// <summary>
        /// Check if RFC_ERROR_INFO has errors.
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <param name="cleanction">Method to be executed before throwing exception</param>
        internal static void OnErrorThrowException(this RFC_ERROR_INFO errorInfo, Action cleanction = null)
        {
            if (errorInfo.Code == RFC_RC.RFC_OK)
                return;

            cleanction?.Invoke();
            throw new RfcException(errorInfo);
        }

        /// <summary>
        /// Check if RFC_RC has errors.
        /// </summary>
        /// <param name="rfcRc"></param>
        /// <param name="errorInfo"></param>
        /// <param name="cleanction">>Method to be executed before throwing exception</param>
        internal static void OnErrorThrowException(this RFC_RC rfcRc,  RFC_ERROR_INFO errorInfo, Action cleanction = null)
        {
            if (rfcRc == RFC_RC.RFC_OK)
                return;

            cleanction?.Invoke();
            throw new RfcException(errorInfo);
        }
    }
}
