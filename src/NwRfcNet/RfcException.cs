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
        public RfcException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of RfcException class with a specified error message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public RfcException(string message, Exception innerException) : base(message, innerException) {}
    }
}
