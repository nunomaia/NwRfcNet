using NwRfcNet.Interop;
using NwRfcNet.Util;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NwRfcNet.RfcTypes
{
    /// <summary>
    /// Represents an RFC Date. Format (YYYYMMDD)
    /// </summary>
    public class RfcDate
    {
        private const string YearFormat = "YYYY";
        private const string MonthFormat = "MM";
        private const string DayFormat = "DD";

        private const string RfcDateTemplate = "YYYYMMDD";

        // Null RFC date in string format
        public static readonly string NullRfcDateString = String.Empty.PadRight(RfcDateTemplate.Length);
        public static readonly string ZerolRfcDateString = String.Empty.PadRight(RfcDateTemplate.Length, '0');

        private static readonly Regex RfcDateFormat = new Regex("^[0-9]{8}$", RegexOptions.Compiled);

        #region Constructors

        /// <summary>
        /// Creates a new RfcDate with format YYYMMDD
        /// </summary>
        /// <param name="date">YYYYMMDD RFC Date</param>
        public RfcDate(string date)
        {
            if (string.IsNullOrEmpty(date) || date == NullRfcDateString || date == ZerolRfcDateString)
            {
                RfcValue = null;
                return;
            }
                
            if (!RfcDateFormat.IsMatch(date))
                throw new ArgumentException($"Invalid RFC Date {date}");

            int year = Int32.Parse(date.Substring(0, YearFormat.Length));
            int month = Int32.Parse(date.Substring(YearFormat.Length, MonthFormat.Length));
            int day = Int32.Parse(date.Substring(YearFormat.Length + MonthFormat.Length, DayFormat.Length));

            RfcValue = new DateTime(year, month, day);
        }

        /// <summary>
        /// Creates a new RfcDate with format YYYMMDD
        /// </summary>
        /// <param name="date">YYYYMMDD RFC Date</param>
        public RfcDate(DateTime? date) => RfcValue = date?.Date;

        #endregion  

        #region Properties

        /// <summary>
        /// Date
        /// </summary>
        public DateTime? RfcValue { get;  }

        #endregion

        /// <summary>
        /// Converts date to RFC Date format
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
            => RfcValue?.ToString("yyyyMMdd") ?? ZerolRfcDateString;

        public char[] ToBuffer() => ToString().ToCharArray();


        #region Interop

        /// <summary>
        /// sets the value of a RFC date field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        internal void SetFieldValue(IntPtr dataHandle, string name)
        {
            if (RfcValue != null)
            {
                var rc = RfcInterop.RfcSetDate(dataHandle, name, ToBuffer(), out var errorInfo);
                rc.OnErrorThrowException(errorInfo);
            }
        }

        /// <summary>
        /// Gets the value of a RFC date field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        /// <returns></returns>
        internal static RfcDate GetFieldValue(IntPtr dataHandle, string name)
        {
            var buffer = new char[RfcDateTemplate.Length]; //   = new StringBuilder(YearFormat.Length + MonthFormat.Length + DayFormat.Length);
            buffer.FillAll(' ');

            var rc = RfcInterop.RfcGetDate(dataHandle, name, buffer, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);

            string date = new string(buffer);

            if ((date == NullRfcDateString) || (date == ZerolRfcDateString))
                return null;

            return new RfcDate(date);
        }
        #endregion
    }
}
