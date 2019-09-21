using System;
using System.Globalization;

namespace NwRfcNet.RfcTypes
{
    public class RfcBcd : IRfcType<decimal>
    {
        #region Constructors

        public RfcBcd(decimal value) => RfcValue = value;

        public RfcBcd(string value) => RfcValue = decimal.Parse(value, CultureInfo.InvariantCulture);

        #endregion

        #region Properties

        public decimal RfcValue { get; }

        #endregion

        public override string ToString() => RfcValue.ToString(CultureInfo.InvariantCulture);

        #region Interop

        /// <summary>
        /// sets the value of a RFC INT8 field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        internal void SetFieldValue(IntPtr dataHandle, string name)
        {
            var str = new RfcString(RfcValue.ToString());
            str.SetFieldValue(dataHandle, name);
        }

        /// <summary>
        /// Gets the value of a RFC INT8 field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        /// <returns></returns>
        internal static RfcBcd GetFieldValue(IntPtr dataHandle, string name)
        {
            var str = RfcString.GetFieldValue(dataHandle, name);
            return new RfcBcd(str.RfcValue);
        }
        #endregion        
    }
}
