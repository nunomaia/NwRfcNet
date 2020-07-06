using System;

namespace NwRfcNet
{
    /// <summary>
    /// Represents RFC errors.
    /// </summary>
    public class RfcException : Exception
    {
        /// <summary>
        /// Initializes a new instance of RfcException
        /// </summary>
        public RfcException() {}

        /// <summary>
        ///  Initializes a new instance of RfcException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public RfcException(string message) : base(message)
        {
        }

        /// <summary>
        ///  Initializes a new instance of RfcException class with a specified error message
        /// </summary>
        /// <param name="errorInfo">RFC error info</param>
        public RfcException(RfcErrorInfo errorInfo) : base(errorInfo.Message)
        {
            ErrorInfo = errorInfo;
        }

        /// <summary>
        ///  Initializes a new instance of RfcException class with a specified error message
        /// </summary>
        /// <param name="errorInfo">RFC error info</param>
        internal RfcException(Interop.RfcInterop.RFC_ERROR_INFO errorInfo) : this(new RfcErrorInfo(errorInfo))
        {
        }

        /// <summary>
        ///  Initializes a new instance of RfcException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="errorInfo">RFC error info</param>
        public RfcException(string message, RfcErrorInfo errorInfo) : base(message)
        {
            ErrorInfo = errorInfo;
        }

        /// <summary>
        ///  Initializes a new instance of RfcException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="errorInfo">RFC error info</param>
        internal RfcException(string message, Interop.RfcInterop.RFC_ERROR_INFO errorInfo) : this(message, new RfcErrorInfo(errorInfo))
        {
        }

        /// <summary>
        /// Initializes a new instance of RfcException class with a specified error message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public RfcException(string message, Exception innerException) : base(message, innerException) {}

        /// <summary>
        /// Initializes a new instance of RfcException class with a specified error message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="errorInfo">RFC error info</param>
        public RfcException(string message, Exception innerException, RfcErrorInfo errorInfo) : base(message, innerException)
        {
            ErrorInfo = errorInfo;
        }

        /// <summary>
        /// Initializes a new instance of RfcException class with a specified error message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="errorInfo">RFC error info</param>
        internal RfcException(string message, Exception innerException, Interop.RfcInterop.RFC_ERROR_INFO errorInfo) : this(message, innerException, new RfcErrorInfo(errorInfo))
        {
        }

        public RfcErrorInfo ErrorInfo { get; }
    }
}
