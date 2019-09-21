using NwRfcNet.Interop;
using NwRfcNet.Util;
using System;
using System.Text.RegularExpressions;

namespace NwRfcNet.RfcTypes
{
    /// <summary>
    /// Represents an RFC Time. Format (HHMMSS)
    /// </summary>
    public class RfcTime
    {
        private const string HoursFormat = "HH";
        private const string MinutesFormat = "MM";
        private const string SecondsFormat = "SS";

        private const string RfcTimeTemplate = "HHMMSS";

        // Null RFC date in string format
        public static readonly string NullRfcTimeString = String.Empty.PadRight(RfcTimeTemplate.Length);
        public static readonly string ZeroRfcTimeString = String.Empty.PadRight(RfcTimeTemplate.Length, '0');

        private static readonly Regex RfcTimeFormat = new Regex("^[0-9]{6}$", RegexOptions.Compiled);

        /// <summary>
        /// Creates a new RfcDate with format YYYMMDD
        /// </summary>
        /// <param name="date">YYYYMMDD RFC Date</param>
        public RfcTime(string time)
        {
            if (string.IsNullOrEmpty(time) || time == NullRfcTimeString || time == ZeroRfcTimeString)
            {
                RfcValue = null;
                return;
            }

            if (!RfcTimeFormat.IsMatch(time))
                throw new ArgumentException($"Invalid RFC Time {time}");

            int hours = Int32.Parse(time.Substring(0, HoursFormat.Length));
            int minutes = Int32.Parse(time.Substring(HoursFormat.Length, MinutesFormat.Length));
            int seconds = Int32.Parse(time.Substring(HoursFormat.Length + MinutesFormat.Length, SecondsFormat.Length));

            RfcValue = new TimeSpan(hours, minutes, seconds);
        }

        /// <summary>
        /// Creates a new RfcDate with format YYYMMDD
        /// </summary>
        /// <param name="date">YYYYMMDD RFC Date</param>
        public RfcTime(TimeSpan? time) => RfcValue = time;

        #region Properties

        /// <summary>
        /// Time
        /// </summary>
        public TimeSpan? RfcValue { get; }

        #endregion

        /// <summary>
        /// Converts date to RFC Time format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => RfcValue?.ToString("hhmmss") ?? ZeroRfcTimeString;

        public char[] ToBuffer() => ToString().ToCharArray();

        #region Interop

        /// <summary>
        /// Sets the value of a RFC time field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        internal void SetFieldValue(IntPtr dataHandle, string name)
        {
            if (RfcValue != null)
            {
                var rc = RfcInterop.RfcSetTime(dataHandle, name, ToBuffer(), out var errorInfo);
                rc.OnErrorThrowException(errorInfo);
            }
        }

        /// <summary>
        /// Gets the value of a RFC date field
        /// </summary>
        /// <param name="dataHandle">handle to container</param>
        /// <param name="name">field name</param>
        /// <returns></returns>
        internal static RfcTime GetFieldValue(IntPtr dataHandle, string name)
        {
            var buffer = new char[RfcTimeTemplate.Length]; //   = new StringBuilder(YearFormat.Length + MonthFormat.Length + DayFormat.Length);
            buffer.FillAll(' ');

            var rc = RfcInterop.RfcGetTime(dataHandle, name, buffer, out var errorInfo);
            rc.OnErrorThrowException(errorInfo);

            string time = new string(buffer);

            if ((time == NullRfcTimeString) || (time == ZeroRfcTimeString))
                return null;

            return new RfcTime(time);
        }
        #endregion
    }
}
