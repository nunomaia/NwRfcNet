using NwRfcNet.Interop;
using NwRfcNet.TypeMapper;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace NwRfcNet
{
    /// <summary>
    /// RFC paramter handler base class
    /// </summary>
    internal abstract class RfcParameter
    {
        /// <summary>
        /// Returns the value of the specified field as a pointer to RFCTYPE_STRUCTURE 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        protected static IntPtr GetStructure(IntPtr handler, PropertyMap map)
        {
            var rc = RfcInterop.RfcGetStructure(handler, map.RfcParameterName, out IntPtr structHandle, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);
            return structHandle;
        }
    }
}