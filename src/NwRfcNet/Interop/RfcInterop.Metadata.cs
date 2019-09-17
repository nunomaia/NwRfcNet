using System;
using System.Runtime.InteropServices;

namespace NwRfcNet.Interop
{
    internal static partial class RfcInterop
    {
        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern IntPtr RfcGetFunctionDesc(IntPtr rfcHandle, string funcName, out RFC_ERROR_INFO errorInfo);
    }    
}