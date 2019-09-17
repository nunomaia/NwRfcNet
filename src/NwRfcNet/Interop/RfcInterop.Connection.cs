using System;
using System.Runtime.InteropServices;

namespace NwRfcNet.Interop
{
    internal static partial class RfcInterop
    {
        [DllImport(NwRfcLib)]
        public static extern IntPtr RfcOpenConnection(RFC_CONNECTION_PARAMETER[] connectionParams, uint paramCount, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib)]
        public static extern RFC_RC RfcCloseConnection(IntPtr rfcHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib)]
        public static extern RFC_RC RfcInvoke(IntPtr rfcHandle, IntPtr funcHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib)]
        internal static extern RFC_RC RfcPing(IntPtr rfcHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib)]
        internal static extern RFC_RC RfcIsConnectionHandleValid(IntPtr rfcHandle, ref int isValid, out RFC_ERROR_INFO errorInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct RFC_CONNECTION_PARAMETER
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string Name;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string Value;
        }
    }    
}