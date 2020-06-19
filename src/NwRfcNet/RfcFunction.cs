using NwRfcNet.Interop;
using NwRfcNet.TypeMapper;
using System;

namespace NwRfcNet
{
    /// <summary>
    ///  Represents a connection to a RFC function. 
    /// </summary>
    public sealed class RfcFunction : IDisposable
    {
        // To detect redundant calls
        private bool _disposed = false;

        // current active rfc connection 
        private readonly RfcConnection _rfcConnection;

        // handle to function description
        private IntPtr _functionDescHandle;

        // current function handle
        internal IntPtr FunctionHandle { get; private set; }

        #region Constructors

        internal RfcFunction(RfcConnection rfcConnection, string functionName)
        {
            _rfcConnection = rfcConnection;
            FunctionName = functionName;
        }

        #endregion

        #region Properties

        public string FunctionName { get; }

        /// <summary>
        /// If required, define a function level RfcMapper
        /// </summary>
        public RfcMapper Mapper { get; set; }

        #endregion

        #region Dispose

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                Close();

            _disposed = true;
        }

        public void Dispose() => Dispose(true);

        #endregion

        /// <summary>
        /// Closes current RFC function call.
        /// </summary>
        public void Close()
        {
            if (FunctionHandle == IntPtr.Zero)
                return;

            var rc = RfcInterop.RfcDestroyFunction(FunctionHandle, out var errorInfo);
            rc.OnErrorThrowException(errorInfo, () => Clear());
            Clear();
        }

        /// <summary>
        /// Invokes current function without parameters 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public void Invoke() => Invoke<object>(null);

        /// <summary>
        /// Invoke current function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public void Invoke<T>(T input)
        {
            _functionDescHandle = RfcInterop.RfcGetFunctionDesc(_rfcConnection.ConnectionHandle, FunctionName, out var errorInfo);
            errorInfo.OnErrorThrowException(() => Clear());

            FunctionHandle = RfcInterop.RfcCreateFunction(_functionDescHandle, out errorInfo);
            errorInfo.OnErrorThrowException(() => Clear());

            if (input != null)
            {
                var inParam = new RfcParameterInput(Mapper ?? _rfcConnection.Mapper);
                inParam.SetParameters(FunctionHandle, input);
            }

            var rc = RfcInterop.RfcInvoke(_rfcConnection.ConnectionHandle, FunctionHandle, out errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }

        public TResult GetOutputParameters<TResult>() => 
            (TResult)new RfcParameterOutput(Mapper ?? _rfcConnection.Mapper)
                .GetReturnParameters(FunctionHandle, typeof(TResult));

        /// <summary>
        /// Clear internal handles
        /// </summary>
        private void Clear()
        {
            _functionDescHandle = IntPtr.Zero;
            FunctionHandle = IntPtr.Zero;
        }
    }
}
