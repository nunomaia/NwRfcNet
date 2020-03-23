using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace NwRfcNet.Util
{
    internal static class UriExtensions
    {
        private const string UriQueryParametersPrefix = "?";
        private const string UriQueryParametersSeparator = "&";
        private const string UriQueryParametersValueSeparator = "=";

        public static NameValueCollection ParseQueryString(this string queryString)
        {
            var nvc = new NameValueCollection();

            if (string.IsNullOrWhiteSpace(queryString))
            {
                return nvc;
            }

            // remove anything other than query string from url
            if (queryString.Contains(UriQueryParametersPrefix))
            {
                queryString = queryString.Substring(queryString.IndexOf(UriQueryParametersPrefix) + UriQueryParametersPrefix.Length);
            }

            foreach (var vp in Regex.Split(queryString, UriQueryParametersSeparator))
            {
                var singlePair = Regex.Split(vp, UriQueryParametersValueSeparator);
                if (singlePair.Length == 2)
                {
                    nvc.Add(singlePair[0], singlePair[1]);
                }
                else
                {
                    // only one key with no value specified in query string
                    nvc.Add(singlePair[0], string.Empty);
                }
            }

            return nvc;
        }
    }
}
