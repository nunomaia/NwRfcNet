using System;
using System.Globalization;

namespace NwRfcNet.RfcTypes
{
    public class RfcBcd : IRfcType<decimal>
    {
        private readonly CultureInfo _cultureInfo;

        #region Constructors

        public RfcBcd(decimal value, CultureInfo cultureInfo = null)
        {
            this._cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
            this.RfcValue = value;
        }

        public RfcBcd(string value, CultureInfo cultureInfo = null)
        {
            this._cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
            this.RfcValue = decimal.Parse(value, this._cultureInfo);
        }

        #endregion

        #region Properties

        public decimal RfcValue { get; }

        #endregion

        public override string ToString() => RfcValue.ToString(this._cultureInfo);

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
