using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NwRfcNet.Interop
{
    internal static partial class RfcInterop
    {
        internal const string NwRfcLib = "sapnwrfc";

        [DllImport(NwRfcLib, CharSet = CharSet.Unicode)]
        internal static extern RFC_RC RfcSetTraceLevel(IntPtr connection, string destination, uint traceLevel, out RFC_ERROR_INFO errorInfo);

        [DllImport(NwRfcLib)]
        internal static extern IntPtr RfcGetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct RFC_ERROR_INFO
        {
            [MarshalAs(UnmanagedType.I4)]
            public RFC_RC Code;

            [MarshalAs(UnmanagedType.I4)]
            public RFC_ERROR_GROUP Group;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string Key;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string Message;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20 + 1)]
            public string AbapMsgClass;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1 + 1)]
            public string AbapMsgType;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3 + 1)]
            public string AbapMsgNumber;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50 + 1)]
            public string AbapMsgV1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50 + 1)]
            public string AbapMsgV2;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50 + 1)]
            public string AbapMsgV3;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50 + 1)]
            public string AbapMsgV4;
        }

        internal enum RFC_RC
        {
            RFC_OK,
            RFC_COMMUNICATION_FAILURE,
            RFC_LOGON_FAILURE,
            RFC_ABAP_RUNTIME_FAILURE,
            RFC_ABAP_MESSAGE,
            RFC_ABAP_EXCEPTION,
            RFC_CLOSED,
            RFC_CANCELED,
            RFC_TIMEOUT,
            RFC_MEMORY_INSUFFICIENT,
            RFC_VERSION_MISMATCH,
            RFC_INVALID_PROTOCOL,
            RFC_SERIALIZATION_FAILURE,
            RFC_INVALID_HANDLE,
            RFC_RETRY,
            RFC_EXTERNAL_FAILURE,
            RFC_EXECUTED,
            RFC_NOT_FOUND,
            RFC_NOT_SUPPORTED,
            RFC_ILLEGAL_STATE,
            RFC_INVALID_PARAMETER,
            RFC_CODEPAGE_CONVERSION_FAILURE,
            RFC_CONVERSION_FAILURE,
            RFC_BUFFER_TOO_SMALL,
            RFC_TABLE_MOVE_BOF,
            RFC_TABLE_MOVE_EOF,
            RFC_START_SAPGUI_FAILURE,
            RFC_ABAP_CLASS_EXCEPTION,
            RFC_UNKNOWN_ERROR,
            RFC_AUTHORIZATION_FAILURE,
            _RFC_RC_max_value
        };

        internal enum RFC_ERROR_GROUP
        {
            OK,
            ABAP_APPLICATION_FAILURE,
            ABAP_RUNTIME_FAILURE,
            LOGON_FAILURE,
            COMMUNICATION_FAILURE,
            EXTERNAL_RUNTIME_FAILURE,
            EXTERNAL_APPLICATION_FAILURE,
            EXTERNAL_AUTHORIZATION_FAILURE
        };

        [SuppressMessage("Naming", "CA1712:Do not prefix enum values with type name", Justification = "<Pending>")]
        internal enum RFCTYPE
        {
            RFCTYPE_CHAR        = 0,      
            RFCTYPE_DATE        = 1,       
            RFCTYPE_BCD         = 2,        
            RFCTYPE_TIME        = 3,       
            RFCTYPE_BYTE        = 4,       
            RFCTYPE_TABLE       = 5,  
            RFCTYPE_NUM         = 6,        
            RFCTYPE_FLOAT       = 7,      
            RFCTYPE_INT         = 8,        
            RFCTYPE_INT2        = 9,      
            RFCTYPE_INT1        = 10,  
            RFCTYPE_NULL        = 14,      
            RFCTYPE_ABAPOBJECT  = 16,
            RFCTYPE_STRUCTURE   = 17,
            RFCTYPE_DECF16      = 23,    
            RFCTYPE_DECF34      = 24,   
            RFCTYPE_XMLDATA     = 28,   
            RFCTYPE_STRING      = 29,    
            RFCTYPE_XSTRING     = 30,   
            RFCTYPE_INT8,           
            RFCTYPE_UTCLONG,        
            RFCTYPE_UTCSECOND,      
            RFCTYPE_UTCMINUTE,      
            RFCTYPE_DTDAY,          
            RFCTYPE_DTWEEK,    
            RFCTYPE_DTMONTH,        
            RFCTYPE_TSECOND,        
            RFCTYPE_TMINUTE,        
            RFCTYPE_CDAY,           
            RFCTYPE_BOX,            
            RFCTYPE_GENERIC_BOX,    
            RFCTYPE_max_value      
        };
    }    
}