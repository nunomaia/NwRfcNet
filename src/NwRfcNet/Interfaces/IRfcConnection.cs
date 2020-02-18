using System;
using NwRfcNet.TypeMapper;

namespace NwRfcNet.Interfaces
{
    public interface IRfcConnection : IDisposable
    {
        string Client { get; set; }

        string Hostname { get; set; }

        string Language { get; set; }

        RfcMapper Mapper { get; set; }

        string Password { set; }

        string SapRouter { get; set; }

        string SncLib { get; set; }

        string SncMyname { get; set; }

        string SncPartnername { get; set; }

        string SncQop { get; set; }

        string SystemNumber { get; set; }

        string Trace { get; set; }

        string UserName { get; set; }

        IntPtr ConnectionHandle { get; }

        IRfcFunction CallRfcFunction(string functionName);

        void Close();

        void Open();

        bool Ping();

        void SetTraceLevel(string destination, TraceLevel traceLevel);
    }
}
