using NwRfcNet.Interop;
using NwRfcNet.TypeMapper;
using System;
using System.Collections.Generic;

namespace NwRfcNet
{
    /// <summary>
    /// Represents a RFC connection to a SAP application server (Type A - Connection)
    /// </summary>
    public sealed class RfcConnection : IDisposable
    {
        // To detect redundant calls
        private bool disposed = false;

        private readonly IRfcConnectionParameters options;

        /// <summary>
        /// Creates a new <see cref="RfcConnection"/> with parameters out of a dictionary.
        /// </summary>
        /// <param name="connectionParameters">The connection parameters.</param>
        public RfcConnection(IDictionary<string, string> connectionParameters)
        {
            if (connectionParameters is null)
            {
                throw new ArgumentNullException(nameof(connectionParameters));
            }

            options = new RfcConnectionParameterBuilder().FromDictionary(connectionParameters).Build();
        }

        /// <summary>
        /// Creates a new <see cref="RfcConnection"/> with parameters out of an uri (must be in form 'sap://[userName]:[password]@[host]?client=..&amp;system_number=...').
        /// </summary>
        /// <param name="connectionUri">The connection uri.</param>
        public RfcConnection(Uri connectionUri)
        {
            if (connectionUri is null)
            {
                throw new ArgumentNullException(nameof(connectionUri));
            }

            options = new RfcConnectionParameterBuilder().FromConnectionUri(connectionUri).Build();
        }

        /// <summary>
        /// Creates a new <see cref="RfcConnection"/> with parameters out of a connection string (must be in form 'Server=[host]; UserName=[userName]; Password=[password]; Client=[Client]...').
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RfcConnection(string connectionString)
        {
            if (connectionString is null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options = new RfcConnectionParameterBuilder()
                .FromConnectionString(connectionString)
                .Build();
        }

        /// <summary>
        /// Creates a new <see cref="RfcConnection"/> with parameters created using a <see cref="RfcConnectionParameterBuilder"/>.
        /// </summary>
        /// <param name="builder">The connection parameter builder action.</param>
        public RfcConnection(Action<RfcConnectionParameterBuilder> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var builderInstance = new RfcConnectionParameterBuilder();
            builder(builderInstance);
            options = builderInstance.Build();
        }

        /// <summary>
        /// Creates a new <see cref="RfcConnection"/> with parameters created using a <see cref="RfcConnectionParameterBuilder"/>.
        /// </summary>
        /// <param name="builder">The connection parameter builder.</param>
        public RfcConnection(RfcConnectionParameterBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            options = builder.Build();
        }

        /// <summary>
        /// Connection handler to RFC Server
        /// </summary>
        internal IntPtr ConnectionHandle { get; private set; } = IntPtr.Zero;

        /// <summary>
        /// Gets the parameters used to create this connection.
        /// </summary>
        public IReadOnlyDictionary<string, string> ConnectionParameters => options.Parameters;

        /// <summary>
        /// Gets or sets the mapper used during this connection.
        /// </summary>
        public RfcMapper Mapper { get; set; } = RfcMapper.DefaultMapper;

        /// <summary>
        /// Opens the connection.
        /// </summary>
        public void Open()
        {
            ThrowWhenConnectionIsOpen();
            var parms = CreateInteropConnectionParameters(options.Parameters);
            ConnectionHandle = RfcInterop.RfcOpenConnection(parms, (uint)parms.Length, out var errorInfo);
            errorInfo.OnErrorThrowException(() => Clear());
        }

        /// <summary>
        /// Creates a call to a remove function.
        /// </summary>
        /// <param name="functionName">The name of the function.</param>
        /// <returns>The called function.</returns>
        public RfcFunction CallRfcFunction(string functionName)
        {
            ThrowWhenConnectionIsClosed();
            return new RfcFunction(this, functionName);
        }

        /// <summary>
        /// Sends a ping to the connected RFC host to check if the connection is still alive.
        /// </summary>
        public bool Ping()
        {
            ThrowWhenConnectionIsClosed();
            return RfcInterop.RfcPing(ConnectionHandle, out _) == RfcInterop.RFC_RC.RFC_OK;
        }

        /// <summary>
        /// Information about currently loaded native library.
        /// </summary>
        /// <returns>The version of the used native library.</returns>
        public static RfcLibVersion GetLibVersion()
        {
            RfcInterop.RfcGetVersion(out var majorVersion, out var minorVersion, out var patchLevel);
            return new RfcLibVersion(majorVersion, minorVersion, patchLevel);
        }

        /// <summary>
        /// Defines Trace Level
        /// </summary>
        /// <param name="destination">The server destination definined in sapnwrfc.ini</param>
        /// <param name="traceLevel">The trace level to use.</param>
        public void SetTraceLevel(string destination, TraceLevel traceLevel)
        {
            ThrowWhenConnectionIsClosed();
            var rc = RfcInterop.RfcSetTraceLevel(ConnectionHandle, destination, (uint)traceLevel, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Close()
        {
            if (ConnectionHandle == IntPtr.Zero)
            {
                return;
            }
            RfcInterop.RfcCloseConnection(ConnectionHandle, out var errorInfo);
            errorInfo.OnErrorThrowException(() => Clear());
            Clear();
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                Close();
            }

            disposed = true;
        }

        /// <summary>
        /// Disposes the connection. Disposing automatically closes the connection.
        /// </summary>
        public void Dispose() => Dispose(true);

        private void Clear() => ConnectionHandle = IntPtr.Zero;

        private void ThrowWhenConnectionIsOpen()
        {
            ThrowWhenAlreadyDisposed();
            if (ConnectionHandle != IntPtr.Zero)
            {
                throw new InvalidOperationException("Connection is open");
            }
        }

        private void ThrowWhenConnectionIsClosed()
        {
            ThrowWhenAlreadyDisposed();
            if (ConnectionHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Connection is closed");
            }
        }

        private void ThrowWhenAlreadyDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(RfcConnection).Name);
            }
        }

        private static RfcInterop.RFC_CONNECTION_PARAMETER[] CreateInteropConnectionParameters(IReadOnlyDictionary<string, string> parameters)
        {
            var rfcParams = new List<RfcInterop.RFC_CONNECTION_PARAMETER>();
            foreach (var prop in parameters)
            {
                if (prop.Value != null)
                {
                    rfcParams.Add(new RfcInterop.RFC_CONNECTION_PARAMETER()
                    {
                        Name = prop.Key,
                        Value = prop.Value
                    }); ;
                }
            }
            return rfcParams.ToArray();
        }


    }

    /// <summary>
    /// RFC Library Trace Level
    /// </summary>
    public enum TraceLevel : uint
    {
        Off = 0,
        Brief = 1,
        Verbose = 2,
        Full = 3,
    }
}
