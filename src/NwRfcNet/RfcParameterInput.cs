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
    internal class RfcParameterInput : RfcParameter
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
                var propValue = inputValue
                    .GetType()
                    .GetProperty(propMap.Key)
                    ?.GetValue(inputValue);

                switch (propMap.Value.ParameterType)
                {
                    case RfcFieldType.Char:
                        SetChars(handler, propMap.Value, (string)propValue);
                        break;

                    case RfcFieldType.Int:
                        if (propValue != null)
                        {
                            SetInt(handler, propMap.Value, (int)propValue);
                        }
                        break;

                    case RfcFieldType.Table:
                        SetTable(handler, propMap.Value, (IEnumerable)propValue);
                        break;

                    case RfcFieldType.Structure:
                        var structHandle = GetStructure(handler, propMap.Value);
                        SetParameters(structHandle, propValue);
                        break;

                    case RfcFieldType.Date:
                        if (propValue != null)
                        {
                            var date = new RfcDate((DateTime)propValue);
                            date.SetFieldValue(handler, propMap.Value.RfcParameterName);
                        }
                        break;

                    case RfcFieldType.Time:
                        if (propValue != null)
                        {
                            var time = new RfcTime((TimeSpan)propValue);
                            time.SetFieldValue(handler, propMap.Value.RfcParameterName);
                        }
                        break;

                    case RfcFieldType.Int8:
                        if (propValue != null)
                        {
                            var int8 = new RfcInt8((long)propValue);
                            int8.SetFieldValue(handler, propMap.Value.RfcParameterName);
                        }
                        break;

                    case RfcFieldType.Bcd:
                        if (propValue != null)
                        {
                            var bcd = new RfcBcd((decimal)propValue);
                            bcd.SetFieldValue(handler, propMap.Value.RfcParameterName);
                        }
                        break;

                    case RfcFieldType.String:
                        SetString(handler, propMap.Value, (string)propValue);
                        break;

                    case RfcFieldType.Byte:
                        SetXString(handler, propMap.Value, (byte[])propValue);
                        break;
                    
                    default:
                        throw new RfcException($"{propMap.Key}, {propMap.Value.RfcParameterName} Rfc Type not handled");
                }
            }
        }

        private static void SetChars(IntPtr dataHandle, PropertyMap map, string charValue)
        {
            if (charValue == null)
                return;

            char[] buffer = new char[map.Length];

            //Truncate long string
            if (charValue.Length > map.Length)
                charValue = charValue.Substring(0, map.Length);

            switch (map.Alignment)
            {
                case StringAlignment.None:
                    charValue.CopyTo(0, buffer, 0, charValue.Length);
                    break;
                case StringAlignment.Left:
                    charValue.PadRight(map.Length, map.PaddingCharacter).CopyTo(0, buffer, 0, buffer.Length);
                    break;
                case StringAlignment.Right:
                    charValue.PadLeft(map.Length, map.PaddingCharacter).CopyTo(0, buffer, 0, buffer.Length);
                    break;
            }

            var rc = RfcInterop.RfcSetChars(dataHandle, map.RfcParameterName, buffer, (uint)map.Length, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }

        private static void SetString(IntPtr dataHandle, PropertyMap map, string stringValue)
        {
            var rc = RfcInterop.RfcSetString(dataHandle, map.RfcParameterName, stringValue, (uint)stringValue.Length, out var errorInfo);
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

            foreach (var row in data)
            {
                var lineHandle = RfcInterop.RfcAppendNewRow(tableHandle, out errorInfo);
                errorInfo.OnErrorThrowException();
                SetParameters(lineHandle, row);
            }
        }
        
        private static void SetXString(IntPtr dataHandle, PropertyMap map, byte[] byteValue)
        {
            var rc = RfcInterop.RfcSetXString(dataHandle, map.RfcParameterName, byteValue, (uint)byteValue.Length, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
        }
    }
}
