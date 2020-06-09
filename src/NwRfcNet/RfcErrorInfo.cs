using System;
using System.Collections.Generic;
using System.Text;
using static NwRfcNet.Interop.RfcInterop;

namespace NwRfcNet
{
    public class RfcErrorInfo
    {
        internal RfcErrorInfo(RFC_ERROR_INFO errorInfo)
        {
            Code = (ErrorCode)errorInfo.Code;
            Group = (ErrorGroup)errorInfo.Group;
            Key = errorInfo.Key;
            Message = errorInfo.Message;
            AbapMsgClass = errorInfo.AbapMsgClass;
            AbapMsgType = errorInfo.AbapMsgType;
            AbapMsgNumber = errorInfo.AbapMsgNumber;
            AbapMsgV1 = errorInfo.AbapMsgV1;
            AbapMsgV2 = errorInfo.AbapMsgV2;
            AbapMsgV3 = errorInfo.AbapMsgV3;
            AbapMsgV4 = errorInfo.AbapMsgV4;
        }

        public ErrorCode Code { get; }
        public ErrorGroup Group { get; }
        public string Key { get; }
        public string Message { get; }
        public string AbapMsgClass { get; }
        public string AbapMsgType { get; }
        public string AbapMsgNumber { get; }
        public string AbapMsgV1 { get; }
        public string AbapMsgV2 { get; }
        public string AbapMsgV3 { get; }
        public string AbapMsgV4 { get; }

        public enum ErrorCode
        {
            Ok = RFC_RC.RFC_OK,
            CommunicationFailure = RFC_RC.RFC_COMMUNICATION_FAILURE,
            LogonFailure = RFC_RC.RFC_LOGON_FAILURE,
            AbapRuntimeFailure = RFC_RC.RFC_ABAP_RUNTIME_FAILURE,
            AbapMessage = RFC_RC.RFC_ABAP_MESSAGE,
            AbapException = RFC_RC.RFC_ABAP_EXCEPTION,
            Closed = RFC_RC.RFC_CLOSED,
            Canceled = RFC_RC.RFC_CANCELED,
            Timeout = RFC_RC.RFC_TIMEOUT,
            MemoryInsufficient = RFC_RC.RFC_MEMORY_INSUFFICIENT,
            VersionMismatch = RFC_RC.RFC_VERSION_MISMATCH,
            InvalidProtocol = RFC_RC.RFC_INVALID_PROTOCOL,
            SerializationFailure = RFC_RC.RFC_SERIALIZATION_FAILURE,
            InvalidHandle = RFC_RC.RFC_INVALID_HANDLE,
            Retry = RFC_RC.RFC_RETRY,
            ExternalFailure = RFC_RC.RFC_EXTERNAL_FAILURE,
            Executed = RFC_RC.RFC_EXECUTED,
            NotFound = RFC_RC.RFC_NOT_FOUND,
            NotSupported = RFC_RC.RFC_NOT_SUPPORTED,
            IllegalState = RFC_RC.RFC_ILLEGAL_STATE,
            InvalidParameter = RFC_RC.RFC_INVALID_PARAMETER,
            CodepageConversionFailure = RFC_RC.RFC_CODEPAGE_CONVERSION_FAILURE,
            ConversionFailure = RFC_RC.RFC_CONVERSION_FAILURE,
            BufferTooSmall = RFC_RC.RFC_BUFFER_TOO_SMALL,
            TableMoveBof = RFC_RC.RFC_TABLE_MOVE_BOF,
            TableMoveEof = RFC_RC.RFC_TABLE_MOVE_EOF,
            StartSapguiFailure = RFC_RC.RFC_START_SAPGUI_FAILURE,
            AbapClassException = RFC_RC.RFC_ABAP_CLASS_EXCEPTION,
            UnknownError = RFC_RC.RFC_UNKNOWN_ERROR,
            AuthorizationFailure = RFC_RC.RFC_AUTHORIZATION_FAILURE
        };

        public enum ErrorGroup
        {
            Ok = RFC_ERROR_GROUP.OK,
            AbapApplicationFailure = RFC_ERROR_GROUP.ABAP_APPLICATION_FAILURE,
            AbapRuntimeFailure = RFC_ERROR_GROUP.ABAP_RUNTIME_FAILURE,
            LogonFailure = RFC_ERROR_GROUP.LOGON_FAILURE,
            CommunicationFailure = RFC_ERROR_GROUP.COMMUNICATION_FAILURE,
            ExternalRuntimeFailure = RFC_ERROR_GROUP.EXTERNAL_RUNTIME_FAILURE,
            ExternalApplicationFailure = RFC_ERROR_GROUP.EXTERNAL_APPLICATION_FAILURE,
            ExternalAuthorizationFailure = RFC_ERROR_GROUP.EXTERNAL_AUTHORIZATION_FAILURE
        };
    }
}
