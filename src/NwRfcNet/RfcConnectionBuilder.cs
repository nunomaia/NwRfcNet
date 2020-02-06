using System;
using System.Collections.Generic;
using System.Linq;
using static NwRfcNet.Interop.RfcInterop;

namespace NwRfcNet
{
    /// <summary>
    /// Create and manage RFC connection propertries used by <see cref="RfcConnection"/> class.
    /// </summary>
    internal class RfcConnectionBuilder
    {
        private static readonly IReadOnlyDictionary<string, string> PropertyToRfcParam = typeof(RfcConnectionBuilder)
                .GetProperties()
                .Where(p => p.IsDefined(typeof(RfcConnectionPropertyAttribute), false))
                .ToDictionary(
                    key => key.Name,
                    prop => ((RfcConnectionPropertyAttribute[])prop.GetCustomAttributes(typeof(RfcConnectionPropertyAttribute), false))
                                .First().RfcParamName);

        #region Properties

        [RfcConnectionPropertyAttribute("user")]
        public string UserName { get; set; }

        [RfcConnectionPropertyAttribute("passwd")]
        public string Password { get; set; }

        [RfcConnectionPropertyAttribute("ASHOST")]
        public string Hostname { get; set; }

        [RfcConnectionPropertyAttribute("MSHOST")]
        public string MessageServerHostname { get; set; }

        [RfcConnectionPropertyAttribute("client")]
        public string Client { get; set; }

        [RfcConnectionPropertyAttribute("sysnr")]
        public string SystemNumber { get; set; }

        [RfcConnectionPropertyAttribute("R3NAME")]
        public string SystemId { get; set; }

        [RfcConnectionPropertyAttribute("GROUP")]
        public string Group { get; set; }

        [RfcConnectionPropertyAttribute("lang")]
        public string Language { get; set; }

        [RfcConnectionPropertyAttribute("saprouter")]
        public string SapRouter { get; set; }

        [RfcConnectionPropertyAttribute("trace")]
        public string Trace { get; set; }

        [RfcConnectionPropertyAttribute("snc_qop")]
        public string SncQop { get; set; }

        [RfcConnectionPropertyAttribute("snc_myname")]
        public string SncMyname { get; set; }

        [RfcConnectionPropertyAttribute("snc_partnername")]
        public string SncPartnername { get; set; }

        [RfcConnectionPropertyAttribute("snc_lib")]
        public string SncLib { get; set; }


        #endregion  

        /// <summary>
        /// Builds RFC_CONNECTION_PARAMETER
        /// </summary>
        /// <returns></returns>
        internal RFC_CONNECTION_PARAMETER[] GetParms()
        {
            var rfcParams = new List<RFC_CONNECTION_PARAMETER>();
            foreach (var prop in PropertyToRfcParam)
            {
                string rfcValue = (string)this
                    .GetType()
                    .GetProperty(prop.Key)
                    .GetValue(this);

                if (rfcValue != null)
                {
                    rfcParams.Add(new RFC_CONNECTION_PARAMETER()
                    {
                        Name = prop.Value,
                        Value = rfcValue
                    }); ;
                }
            }

            return rfcParams.ToArray();
        }

        #region Attributes

        /// <summary>
        /// Identify Connection String property
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class RfcConnectionPropertyAttribute : Attribute
        {
            /// <summary>
            /// Name of Rfc connection parameter
            /// </summary>
            public string RfcParamName { get; }

            /// <summary>
            /// Creates a <see cref="RfcConnectionPropertyAttribute"/>.
            /// </summary>
            public RfcConnectionPropertyAttribute(string rfcParamName)
            {
                RfcParamName = rfcParamName;
            }
        }

        #endregion
    }
}
