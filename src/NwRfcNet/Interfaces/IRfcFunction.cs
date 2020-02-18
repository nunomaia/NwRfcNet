using System;
using NwRfcNet.TypeMapper;

namespace NwRfcNet.Interfaces
{
    public interface IRfcFunction : IDisposable
    {
        string FunctionName { get; }

        RfcMapper Mapper { get; set; }

        void Close();

        TResult GetOutputParameters<TResult>();

        void Invoke();

        void Invoke<T>(T input);
    }
}
