using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NwRfcNet
{
    internal class RfcConnectionParameters : IRfcConnectionParameters
    {
        public const string DefaultUserNameParameterKey = "user";
        public const string DefaultPasswordParameterKey = "passwd";
        public const string DefaultHostParameterKey = "ASHOST";
        public const string DefaultClientParameterKey = "client";

        public const string DefaultSystemNumberKey = "sysnr";
        public const string DefaultSystemIdKey = "SYSID";
        public const string DefaultConnectionNameParameterKey = "name";
        public const string DefaultConnectionLanguageParameterKey = "lang";
        public const string DefaultTraceParameterKey = "trace";
        public const string DefaultConnectionPoolSizeParameterKey = "POOL_SIZE";
        public const string DefaultSapRouterParameterKey = "saprouter";

        public const string DefaultSncQopParameterKey = "snc_qop";
        public const string DefaultSncMyNameParameterKey = "snc_myname";
        public const string DefaultSncPartnerNameParameterKey = "snc_partnername";
        public const string DefaultSncLibParameterKey = "snc_lib";
        public const string DefaultSncModeParameterKey = "snc_mode";

        private readonly IDictionary<string, string> connectionParameters;

        public RfcConnectionParameters() => 
            connectionParameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyDictionary<string, string> Parameters => 
            new ReadOnlyDictionary<string, string>(connectionParameters);

        public RfcConnectionParameters SetParameter(string key, string value)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!connectionParameters.ContainsKey(key))
            {
                connectionParameters.Add(key, value);
            }
            else
            {
                connectionParameters[key] = value;
            }
            return this;
        }
    }
}
