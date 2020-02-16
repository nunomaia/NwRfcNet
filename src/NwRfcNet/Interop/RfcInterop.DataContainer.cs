using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NwRfcNet.Interop
{
    internal static partial class RfcInterop
    {
        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern IntPtr RfcCreateFunction(IntPtr funcDescHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcDestroyFunction(IntPtr funcHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetInt(IntPtr dataHandle, string name, out int value, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcSetInt(IntPtr dataHandle, string name, int value, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetChars(IntPtr dataHandle, string name, char[] charBuffer, uint bufferLength, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcSetChars(IntPtr dataHandle, string name, char[] charValue, uint valueLength, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetStructure(IntPtr dataHandle, string name, out IntPtr structHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetTable(IntPtr dataHandle, string name, out IntPtr tableHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetRowCount(IntPtr tableHandle, out uint rowCount, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib)]
        public static extern RFC_RC RfcMoveToNextRow(IntPtr tableHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib)]
        public static extern RFC_RC RfcMoveTo(IntPtr tableHandle, uint index, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib)]
        public static extern IntPtr RfcGetCurrentRow(IntPtr tableHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetString(IntPtr dataHandle, string name, char[] stringBuffer, uint bufferLength, out uint stringLength, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern IntPtr RfcCreateTable(IntPtr typeDescHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern IntPtr RfcAppendNewRow(IntPtr tableHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib)]
        public static extern RFC_RC RfcDeleteAllRows(IntPtr tableHandle, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetDate(IntPtr dataHandle, string name, char[] emptyDate, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcSetDate(IntPtr dataHandle, string name, char[] date, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcSetTime(IntPtr dataHandle, string name, char[] time, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetTime(IntPtr dataHandle, string name, char[] emptyTime, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetInt8(IntPtr dataHandle, string name, ref long value, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcSetInt8(IntPtr dataHandle, string name, long value, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcSetString(IntPtr dataHandle, string name, string value, uint valueLength, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetString(IntPtr dataHandle, string name, ref char[] stringBuffer, uint valueLength, out uint stringLength, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcGetXString(IntPtr dataHandle, string name, byte[] byteBuffer, uint bufferLength, out uint xstringLength, out RFC_ERROR_INFO errorInfo);
        
        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        public static extern RFC_RC RfcSetXString(IntPtr dataHandle, string name, byte[] byteValue, uint bufferLength, out RFC_ERROR_INFO errorInfo);

    }
}