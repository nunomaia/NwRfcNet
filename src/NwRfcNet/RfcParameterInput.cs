using NwRfcNet.Interop;
using NwRfcNet.TypeMapper;
using System;
using System.Reflection;
using System.Collections;
using NwRfcNet.RfcTypes;
using System.Text;

namespace NwRfcNet
{
    /// <summary>
    /// RFC Input paramter handler
    /// </summary>
    internal class RfcParameterInput  : RfcParameter
    {
        private readonly RfcMapper _mapper;

        internal RfcParameterInput(RfcMapper mapper) =>
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        internal void SetParameters(IntPtr handler, object inputValue)
        {
            if (inputValue == null)
                return;

            foreach (var propMap in _mapper[inputValue.GetType()])
            {
                object propValue = inputValue
                    .GetType()
                    .GetProperty(propMap.Key)
                    .GetValue(inputValue);

                switch (propMap.Value.ParameterType)
                {
                    case RfcFieldType.Char:
                        SetChars(handler, propMap.Value, (string) propValue);
                        break;

                    case RfcFieldType.Int:
                        SetInt(handler, propMap.Value, (int) propValue);
                        break;

                    case RfcFieldType.Table:
                        SetTable(handler, propMap.Value, (IEnumerable) propValue);
                        break;

                    case RfcFieldType.Structure:
                        var structHandle = GetStructure(handler, propMap.Value);
                        SetParameters(structHandle, propValue);
                        break;

                    case RfcFieldType.Date:
                        var date = new RfcDate((DateTime)((object)propMap.Value));
                        date.SetFieldValue(handler, propMap.Value.RfcParameterName);
                        break;

                    case RfcFieldType.Time:
                        var time = new RfcTime((TimeSpan)((object)propMap.Value));
                        time.SetFieldValue(handler, propMap.Value.RfcParameterName);
                        break;

                    case RfcFieldType.Int8:
                        var int8 = new RfcInt8((long)((object)propMap.Value));
                        int8.SetFieldValue(handler, propMap.Value.RfcParameterName);
                        break;

                    case RfcFieldType.Bcd:
                        var bcd = new RfcBcd((decimal)((object)propMap.Value));
                        bcd.SetFieldValue(handler, propMap.Value.RfcParameterName);
                        break;

                    default:
                        throw new RfcException("Rfc Type not handled");
                }
            }
        }

        private static void SetChars(IntPtr dataHandle, PropertyMap map, string charValue)
        {
            if (charValue == null)
                return;

            char[] buffer = new char[map.Length];
            charValue.CopyTo(0, buffer, 0, charValue.Length);
            for (int i = charValue.Length; i < buffer.Length; i++)
                buffer[i] =  ' ';

            var rc = RfcInterop.RfcSetChars(dataHandle, map.RfcParameterName, buffer, (uint) map.Length, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }
        private static void SetInt(IntPtr dataHandle, PropertyMap map, int intValue)
        {
            var rc = RfcInterop.RfcSetInt(dataHandle, map.RfcParameterName, intValue, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }

        private void SetTable(IntPtr dataHandle, PropertyMap map, IEnumerable data)
        {
            var rc = RfcInterop.RfcGetTable(dataHandle, map.RfcParameterName, out IntPtr tableHandle, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);

            foreach(var row in data)
            {
                var lineHandle = RfcInterop.RfcAppendNewRow(tableHandle, out errorInfo);
                errorInfo.OnErrorThrowException();
                SetParameters(lineHandle, row);
            }
        }              
    }
}
