using NwRfcNet.Interop;
using NwRfcNet.TypeMapper;
using System;

namespace NwRfcNet 
{
    /// <summary>
    /// Represents a connection to a RFC server.
    /// </summary>
    public sealed class RfcConnection : IDisposable
    {
        // To detect redundant calls
        private bool _disposed = false;

        // connection parameters
        private readonly RfcConnectionBuilder _parms = new RfcConnectionBuilder();

        #region Constructors

        /// <summary>
        /// Creates a connection to a RFC server
        /// </summary>
        public RfcConnection() { }

        /// <summary>
        /// Creates a connection to a RFC server
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="hostname"></param>
        /// <param name="client"></param>
        /// <param name="language"></param>
        /// <param name="systemNumber"></param>
        /// <param name="sapRouter"></param>
        public RfcConnection(string userName = null,
            string password = null,
            string hostname = null,
            string client = null,
            string language = null,
            string systemNumber = null,
            string sapRouter = null,
            string sncQop = null,
            string sncMyname = null,
            string sncPartnername = null,
            string sncLib = null
            )
        {
            UserName = userName;
            Password = password;
            Hostname = hostname;
            Client = client;
            Language = language;
            SystemNumber = systemNumber;
            SapRouter = sapRouter;
            SncQop = sncQop;
            SncMyname = sncMyname;
            SncPartnername = sncPartnername;
            SncLib = sncLib;
        }

        #endregion

        /// <summary>
        /// Connection handler to RFC Server
        /// </summary>
        internal IntPtr ConnectionHandle { get; private set; } = IntPtr.Zero;

        #region Properties

        public string UserName
        {
            get => _parms.UserName;
            set
            {
                CheckConnectionIsClosed();
                _parms.UserName = value;
            }
        }

        public string Password
        {
            private get => _parms.Password;
            set
            {
                CheckConnectionIsClosed();
                _parms.Password = value;
            }
        }

        public string Hostname
        {
            get => _parms.Hostname;
            set
            {
                CheckConnectionIsClosed();
                _parms.Hostname = value;
            }
        }

        public string Client
        {
            get => _parms.Client;
            set
            {
                CheckConnectionIsClosed();
                _parms.Client = value;
            }
        }

        public string SystemNumber
        {
            get => _parms.SystemNumber;
            set
            {
                CheckConnectionIsClosed();
                _parms.SystemNumber = value;
            }
        }

        public string Language
        {
            get => _parms.Language;
            set
            {
                CheckConnectionIsClosed();
                _parms.Language = value;
            }
        }

        public string SapRouter
        {
            get => _parms.SapRouter;
            set
            {
                CheckConnectionIsClosed();
                _parms.SapRouter = value;
            }
        }

        public string Trace
        {
            get => _parms.Trace;
            set
            {
                CheckConnectionIsClosed();
                _parms.Trace = value;
            }
        }

        public string SncQop
        {
            get => _parms.SncQop;
            set
            {
                CheckConnectionIsClosed();
                _parms.SncQop = value;
            }
        }

        public string SncMyname
        {
            get => _parms.SncMyname;
            set
            {
                CheckConnectionIsClosed();
                _parms.SncMyname = value;
            }
        }

        public string SncPartnername
        {
            get => _parms.SncPartnername;
            set
            {
                CheckConnectionIsClosed();
                _parms.SncPartnername = value;
            }
        }

        public string SncLib
        {
            get => _parms.SncLib;
            set
            {
                CheckConnectionIsClosed();
                _parms.SncLib = value;
            }
        }

        public RfcMapper Mapper { get; set; } = RfcMapper.DefaultMapper;

        #endregion

        /// <summary>
        /// Opens an RFC connection 
        /// </summary>
        public void Open()
        {
            CheckConnectionIsClosed();
            var parms = _parms.GetParms();
            ConnectionHandle = RfcInterop.RfcOpenConnection(parms, (uint) parms.Length, out var errorInfo);
            errorInfo.OnErrorThrowException(() => Clear());
        }

        /// <summary>
        /// Creates a call to a RFC 
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public RfcFunction CallRfcFunction(string functionName)
        {
            CheckConnectionIsOpen();
            return new RfcFunction(this, functionName);
        }

        /// <summary>
        ///  Sends a ping to the RFC backend to check if the connection is still alive.
        /// </summary>
        public bool Ping()
        {
            CheckConnectionIsOpen();
            return RfcInterop.RfcPing(ConnectionHandle, out _) == RfcInterop.RFC_RC.RFC_OK;
        }

        /// <summary>
        /// Information about currently loaded sapnwrfc library
        /// </summary>
        /// <returns></returns>
        public static RfcLibVersion GetLibVersion()
        {
            RfcInterop.RfcGetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel);
            return new RfcLibVersion(majorVersion, minorVersion, patchLevel);
        }

        /// <summary>
        /// Defines Trace Level
        /// </summary>
        /// <param name="destination">server destination definined in sapnwrfc.ini</param>
        /// <param name="traceLevel">Trace Level</param>
        public void SetTraceLevel(string destination, TraceLevel traceLevel)
        {
            CheckConnectionIsOpen();
            var rc = RfcInterop.RfcSetTraceLevel(ConnectionHandle, destination, (uint) traceLevel, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }

        /// <summary>
        /// Closes an RFC connection  
        /// </summary>
        public void Close()
        {
            if (ConnectionHandle == IntPtr.Zero)
                return;

            RfcInterop.RfcCloseConnection(ConnectionHandle, out var errorInfo);
            errorInfo.OnErrorThrowException(() => Clear());
            Clear();
        }


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

        private void Clear() => ConnectionHandle = IntPtr.Zero;

        private void CheckConnectionIsClosed()
        {
            CheckDisposed();
            if (ConnectionHandle != IntPtr.Zero)
                throw new InvalidOperationException("Connection is open");
        }

        private void CheckConnectionIsOpen()
        {
            CheckDisposed();
            if (ConnectionHandle == IntPtr.Zero)
                throw new InvalidOperationException("Connection is closed");
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(RfcConnection).Name);
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