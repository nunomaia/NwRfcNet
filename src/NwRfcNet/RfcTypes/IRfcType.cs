using System;

namespace NwRfcNet.RfcTypes
{
    internal interface IRfcType<T>
    {
        T RfcValue { get; }
    }
}
