using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NwRfcNet.Util;

namespace NwRfcNet
{
    /// <summary>
    /// Helps to create the parameters to build a RFC connection.
    /// </summary>
    public class RfcConnectionParameterBuilder
    {
        private readonly RfcConnectionParameters parameters;
        private const string ConnectionUriScheme = "sap";
        private const char ConnectionStringItemSeparator = ';';
        private const char ConnectionStringKeyValueSeparator = '=';
        private static readonly char[] ConnectionStringKeyValueSeparatorSplit = new char[] { ConnectionStringKeyValueSeparator };
        private static readonly string[] UserNameKeyAliases = new[] { "userName", "userId", "uid", "user", "u" };
        private static readonly string[] PasswordKeyAliases = new[] { "password", "passwd", "pass", "pwd", "p" };
        private static readonly string[] TargetHostKeyAliases = new[] { "target_host", "targetHost", "host", "server", "h" };
        private static readonly string[] LogonLanguageKeyAliases = new[] { "language", "lang", "l" };
        private static readonly string[] LogonClientKeyAliases = new[] { "client", "cl", "c" };
        private static readonly string[] SystemNumberKeyAliases = new[] { "system_number", "systemnumber", "sysnr" };
        private static readonly string[] SystemIdKeyAliases = new[] { "system_id", "systemid", "sysid" };
        private static readonly string[] TraceKeyAliases = new[] { "trace", "tr", "RfcSdkTrace" };
        private static readonly string[] PoolSizeKeyAliases = new[] { "connection_pool_size", "pool_size", "ps" };
        private static readonly string[] SapRouterKeyAliases = new[] { "sap_router_name", "sap_router", "router_name", "router", "rn" };
        private static readonly string[] SncModeKeyAliases = new[] { "snc_mode", "sncmode", "UseSnc", "snc" };
        private static readonly string[] SncQopKeyAliases = new[] { "snc_qop", "sncqop" };
        private static readonly string[] SncMyNameKeyAliases = new[] { "snc_myname", "sncmyname" };
        private static readonly string[] SncPartnerKeyAliases = new[] { "snc_partnername", "sncpartnername", "snc_partner", "sncpartner" };
        private static readonly string[] SncLibKeyAliases = new[] { "snc_library", "snc_lib", "snclib" };

        private static readonly string[] ActivationAliases = new[] { "1", "enabled", "On", "true", "yes" };
        private static readonly string[] DeactivationAliases = new[] { "0", "disabled", "Off", "false", "no" };

        /// <summary>
        /// Creates a new instance of the <see cref="RfcConnectionParameterBuilder"/> class.
        /// </summary>
        public RfcConnectionParameterBuilder()
        {
            this.parameters = new RfcConnectionParameters();
        }

        /// <summary>
        /// Sets the user name for the RFC connection.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseLogonUserName(string userName, string key = RfcConnectionParameters.DefaultUserNameParameterKey) =>
          this.SetParameter(key, userName);

        /// <summary>
        /// Sets the password for the RFC connection.
        /// </summary>
        /// <param name="password">The password of the connection.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseLogonPassword(string password, string key = RfcConnectionParameters.DefaultPasswordParameterKey) =>
          this.SetParameter(key, password);

        /// <summary>
        /// Sets the connection client.
        /// </summary>
        /// <param name="client">The sap server connection client.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseLogonClient(string client, string key = RfcConnectionParameters.DefaultClientParameterKey) =>
          this.SetParameter(key, client);

        /// <summary>
        /// Sets the connection language.
        /// </summary>
        /// <param name="connectionLanguage">The host name of the sap server.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseLogonLanguage(string connectionLanguage, string key = RfcConnectionParameters.DefaultConnectionLanguageParameterKey) =>
          this.SetParameter(key, connectionLanguage);

        /// <summary>
        /// Sets the host name of the sap server.
        /// </summary>
        /// <param name="host">The host name of the sap server.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseConnectionHost(string host, string key = RfcConnectionParameters.DefaultHostParameterKey) =>
          this.SetParameter(key, host);

        /// <summary>
        /// Sets the sap system number.
        /// </summary>
        /// <param name="systemNumber">The sap system number.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseSystemNumber(string systemNumber, string key = RfcConnectionParameters.DefaultSystemNumberKey) =>
          this.SetParameter(key, systemNumber);

        /// <summary>
        /// Sets the id of the sap system.
        /// </summary>
        /// <param name="systemId">The id of the sap system.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseSystemId(string systemId, string key = RfcConnectionParameters.DefaultSystemIdKey) =>
          this.SetParameter(key, systemId);

        /// <summary>
        /// Sets the trace state of the connection (This must be 'true/false', '0/1', 'On/Off', 'enabled/disabled', 'yes/no').
        /// </summary>
        /// <param name="trace">The trace activation state of the connection.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseTrace(string trace = "0", string key = RfcConnectionParameters.DefaultTraceParameterKey)
        {
            if (trace is null)
            {
                throw new ArgumentNullException(nameof(trace));
            }

            if (ActivationAliases.Any(aa => trace.IndexOf(aa, StringComparison.OrdinalIgnoreCase) > -1))
            {
                this.SetParameter(key, "1");
            }
            else if (DeactivationAliases.Any(da => trace.IndexOf(da, StringComparison.OrdinalIgnoreCase) > -1))
            {
                this.SetParameter(key, "0");
            }
            else
            {
                throw new ArgumentException(paramName: nameof(trace), message: $"The give value '{trace}' was invalid. Permitted values are: 'true/false', '0/1', 'On/Off', 'enabled/disabled', 'yes/no'");
            }

            return this;
        }

        /// <summary>
        /// Sets size of the connection pool (This must be a positive value).
        /// </summary>
        /// <param name="connectionPoolSize">The size of the connection pool.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseConnectionPooling(int connectionPoolSize = 10, string key = RfcConnectionParameters.DefaultConnectionPoolSizeParameterKey)
        {
            if (connectionPoolSize < 1)
            {
                throw new ArgumentException(paramName: nameof(connectionPoolSize), message: "The connection pool size must be at greater than zero.");
            }

            return this.SetParameter(key, connectionPoolSize.ToString());
        }

        /// <summary>
        /// Sets the name of the sap router for the RFC connection.
        /// </summary>
        /// <param name="sapRouter">The name of the sap router.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseSapRouter(string sapRouter, string key = RfcConnectionParameters.DefaultSapRouterParameterKey) =>
           this.SetParameter(key, sapRouter);

        /// <summary>
        /// Sets the secure network communication quality of protection (This must be one of the values '1','2','3','8','9').
        /// </summary>
        /// <param name="sncQop">The SNC quality of protection (protection level).</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseSecureNetworkCommunicationQop(string sncQop = "3", string key = RfcConnectionParameters.DefaultSncQopParameterKey)
        {
            if (int.TryParse(sncQop, out var parsed) && (parsed == 1 || parsed == 2 || parsed == 3 || parsed == 8 || parsed == 9))
            {
                this.SetParameter(key, parsed.ToString());
            }

            throw new ArgumentException(paramName: nameof(sncQop), message: "The only permitted values for the quality of protection are: '1','2','3','8','9'.");
        }

        /// <summary>
        /// Sets the secure network communication mode (This should be one of the values '0'->SNC-Disabled, '1'->SNC-Enabled).
        /// </summary>
        /// <param name="sncMode">The SNC mode ('0'->SNC-Disabled, '1'->SNC-Enabled).</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseSecureNetworkCommunicationMode(string sncMode, string key = RfcConnectionParameters.DefaultSncModeParameterKey)
        {
            if (int.TryParse(sncMode, out var parsed) && (parsed == 0 || parsed == 1))
            {
                this.SetParameter(key, parsed.ToString());
            }

            throw new ArgumentException(paramName: nameof(sncMode), message: "The only permitted values for the snc-mode are: '0' for SNC-Disabled or'1' for SNC-Enabled.");
        }

        /// <summary>
        /// Sets the secure network communication name of the RFC server program.
        /// </summary>
        /// <param name="sncMyName">The SNC name of the RFC server program.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseSecureNetworkCommunicationMyName(string sncMyName, string key = RfcConnectionParameters.DefaultSncMyNameParameterKey) =>
           this.SetParameter(key, sncMyName);

        /// <summary>
        /// Sets the secure network communication library.
        /// </summary>
        /// <param name="sncLibName">The path and file name of the 'gssapi' library.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseSecureNetworkCommunicationLibrary(string sncLibName, string key = RfcConnectionParameters.DefaultSncLibParameterKey) =>
           this.SetParameter(key, sncLibName);

        /// <summary>
        /// Sets the secure network communication partner.
        /// </summary>
        /// <param name="sncPartnerName">The partner name.</param>
        /// <param name="key">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseSecureNetworkCommunicationPartner(string sncPartnerName, string key = RfcConnectionParameters.DefaultSncPartnerNameParameterKey) =>
           this.SetParameter(key, sncPartnerName);

        /// <summary>
        /// Sets an additional connection parameter.
        /// </summary>
        /// <param name="parameterValue">The partner name.</param>
        /// <param name="parameterKey">The key for the parameter.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseAdditionalParameter(string parameterValue, string parameterKey) =>
           this.SetParameter(parameterKey, parameterValue);

        /// <summary>
        /// Sets multiple logon parameters for the connection creation.
        /// </summary>
        /// <param name="language">The connection language (This is normally a 2-letter country key like EN,DE or FR).</param>
        /// <param name="client">The client identifier (This is normally a 3 digit number with leading zeros like '001').</param>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder UseLogonInformation(string language, string client, string userName, string password)
        {
            if (language is null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (userName is null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            this.UseLogonLanguage(language);
            this.UseLogonClient(client);
            this.UseLogonUserName(userName);
            this.UseLogonPassword(password);
            return this;
        }

        /// <summary>
        /// Use an <see cref="Uri"/> instance to build a connection (must be in form 'scheme://userinfoparams@hostinfoparams?query_string').
        /// </summary>
        /// <param name="connectionUri">The uri representing the connection parameters.</param>
        /// <returns>The connection parameter builder.</returns>
        /// <remarks>
        /// A complete uri wourld be:
        /// <para>
        /// sap://user=[USER_NAME];passwd=[PASSWORD];Client=[CLIENT];lang=[LANGUAGE];[UseSnc]=[true|false]@connectiontype/conndetail1/conndetail2?GwHost=[GWHOST]?GwServ=[GWSERV]?MsServ=[MSSERV]?Group=[GROUP]?ListenerDest=[LISTENERDEST]?ListenerGwHost=[LISTENERGWHOST]?ListenerGwServ=[LISTENERGWSERV]?ListenerProgramId=[LISTENERPROGRAMID]?RfcSdkTrace=[true/false]?AbapDebug=[true/false]
        /// </para>
        /// A sample would be:
        /// <para>
        /// sap://Client=800;lang=EN@A/YourSAPHOST/00 
        /// </para>
        /// </remarks>
        public RfcConnectionParameterBuilder FromConnectionUri(Uri connectionUri)
        {
            if (connectionUri is null)
            {
                throw new ArgumentNullException(nameof(connectionUri));
            }

            if (!connectionUri.Scheme.Equals(ConnectionUriScheme, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(paramName: nameof(connectionUri), message: $"The connection string uri must have the scheme '{ConnectionUriScheme}'.");
            }

            var userInfo = connectionUri?.UserInfo.Split(new[] { ConnectionStringItemSeparator }, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };
            var userInfoKvp = userInfo
              .Where(kvp => kvp.Contains(ConnectionStringKeyValueSeparator))
              .Select(kvp => kvp.Split(ConnectionStringKeyValueSeparatorSplit, 2))
              .ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim(), StringComparer.OrdinalIgnoreCase);

            var userName = this.GetWhenPresent(userInfoKvp, UserNameKeyAliases);
            userName = connectionUri.UserEscaped ? Uri.UnescapeDataString(userName) : userName;

            var password = this.GetWhenPresent(userInfoKvp, PasswordKeyAliases);
            password = connectionUri.UserEscaped ? Uri.UnescapeDataString(password) : password;

            var client = this.GetWhenPresent(userInfoKvp, LogonClientKeyAliases);
            client = connectionUri.UserEscaped ? Uri.UnescapeDataString(client) : client;

            var language = this.GetWhenPresent(userInfoKvp, LogonLanguageKeyAliases);
            language = connectionUri.UserEscaped ? Uri.UnescapeDataString(language) : language;

            var sncMode = this.GetWhenPresent(userInfoKvp, SncModeKeyAliases);
            sncMode = connectionUri.UserEscaped ? Uri.UnescapeDataString(sncMode) : sncMode;

            var param = connectionUri.PathAndQuery.ParseQueryString();

            param.Add("userName", userName);
            param.Add("password", password);
            param.Add("client", client);
            param.Add("language", language);
            param.Add("snc_mode", sncMode);

            if (connectionUri.Host.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                var detail1 = connectionUri.Segments.Length > 1 ? connectionUri.Segments[1] : null;
                if (detail1 == null)
                {
                    throw new ArgumentException(paramName: nameof(connectionUri), message: "The uri must contain the sap application server host (ASHOST) in the second segment.");
                }
                var detail2 = connectionUri.Segments.Length > 2 ? connectionUri.Segments[2] : null;
                if (detail2 == null)
                {
                    throw new ArgumentException(paramName: nameof(connectionUri), message: "The uri must contain the sap system number (SYSNR) in the third segment.");
                }
            }
            else
            {
                throw new NotSupportedException($"The given connection type '{connectionUri.Host}' is not supported yet.");
            }

            this.FromNamedValues(param);
            return this;
        }

        /// <summary>
        /// Use a connection string to build a connection (must be in form 'Server=[host]; UserName=[userName]; Password=[password]; Client=[Client]...').
        /// </summary>
        /// <param name="connectionString">The string representing the connection parameters.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder FromConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(paramName: nameof(connectionString), message: "The connection string must not be null or empty.");
            }

            var keyValuePairs = connectionString.Split(ConnectionStringItemSeparator)
              .Where(kvp => kvp.Contains(ConnectionStringKeyValueSeparator))
              .Select(kvp => kvp.Split(ConnectionStringKeyValueSeparatorSplit, 2))
              .Select(kvp => new { Key = kvp[0].Trim(), Value = kvp[1].Trim() });

            var collection = new NameValueCollection();
            foreach (var kvp in keyValuePairs)
            {
                collection.Add(kvp.Key, kvp.Value);
            }
            return this.FromNameValueCollection(collection);
        }

        /// <summary>
        /// Use a <see cref="NameValueCollection"/> instance to build a connection.
        /// </summary>
        /// <param name="connectionParameters">The collection containing the connection parameters.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder FromNameValueCollection(NameValueCollection connectionParameters)
        {
            this.FromNamedValues(connectionParameters);
            return this;
        }

        /// <summary>
        /// Use a <see cref="IDictionary{TKey, TValue}"/> instance to build a connection.
        /// </summary>
        /// <param name="connectionParameters">The dictionary containing the connection parameters.</param>
        /// <returns>The connection parameter builder.</returns>
        public RfcConnectionParameterBuilder FromDictionary(IDictionary<string, string> connectionParameters)
        {
            if (connectionParameters is null)
            {
                throw new ArgumentNullException(nameof(connectionParameters));
            }

            var collection = new NameValueCollection();
            foreach (var kvp in connectionParameters)
            {
                collection.Add(kvp.Key, kvp.Value);
            }
            return this.FromNameValueCollection(collection);
        }

        private RfcConnectionParameterBuilder SetParameter(string key, string value)
        {
            this.parameters.SetParameter(key, value);
            return this;
        }

        private void SetWhenPresent(string value, Action<string> setAction)
        {
            if (!string.IsNullOrEmpty(value))
            {
                setAction(value);
            }
        }

        private string GetAndRemoveWhenPresent(ref NameValueCollection source, IEnumerable<string> aliases)
        {
            string foundItem = null;
            foreach (var alias in aliases)
            {
                var item = source.Get(alias);
                if (item != null)
                {
                    foundItem = item;
                }
                source.Remove(alias);
            }
            return foundItem;
        }

        private string GetWhenPresent(IDictionary<string, string> source, IEnumerable<string> aliases)
        {
            foreach (var alias in aliases)
            {
                if (source.ContainsKey(alias))
                {
                    return source[alias];
                }
            }
            return null;
        }

        private void FromValues(string userName,
          string password,
          string host,
          string language,
          string client,
          string systemNumber,
          string systemId,
          string trace,
          string poolSize,
          string saprouter,
          string sncMode,
          string sncQop,
          string sncMyName,
          string sncPartner,
          string sncLib)
        {
            this.SetWhenPresent(userName, val => this.UseLogonUserName(val));
            this.SetWhenPresent(password, val => this.UseLogonPassword(val));
            this.SetWhenPresent(host, val => this.UseConnectionHost(val));
            this.SetWhenPresent(language, val => this.UseLogonLanguage(val));
            this.SetWhenPresent(client, val => this.UseLogonClient(val));
            this.SetWhenPresent(systemNumber, val => this.UseSystemNumber(val));
            this.SetWhenPresent(systemId, val => this.UseSystemId(val));
            this.SetWhenPresent(trace, val => this.UseTrace(val));
            this.SetWhenPresent(poolSize, val => this.UseConnectionPooling(int.Parse(poolSize)));
            this.SetWhenPresent(saprouter, val => this.UseSapRouter(val));
            this.SetWhenPresent(sncMode, val => this.UseSecureNetworkCommunicationMode(val));
            this.SetWhenPresent(sncQop, val => this.UseSecureNetworkCommunicationQop(val));
            this.SetWhenPresent(sncMyName, val => this.UseSecureNetworkCommunicationMyName(val));
            this.SetWhenPresent(sncPartner, val => this.UseSecureNetworkCommunicationPartner(val));
            this.SetWhenPresent(sncLib, val => this.UseSecureNetworkCommunicationLibrary(val));
        }

        private void FromNamedValues(NameValueCollection param)
        {
            var localCopy = new NameValueCollection(param);

            var userName = this.GetAndRemoveWhenPresent(ref localCopy, UserNameKeyAliases);
            var password = this.GetAndRemoveWhenPresent(ref localCopy, PasswordKeyAliases);
            var host = this.GetAndRemoveWhenPresent(ref localCopy, TargetHostKeyAliases);
            var language = this.GetAndRemoveWhenPresent(ref localCopy, LogonLanguageKeyAliases);
            var client = this.GetAndRemoveWhenPresent(ref localCopy, LogonClientKeyAliases);
            var systemNumber = this.GetAndRemoveWhenPresent(ref localCopy, SystemNumberKeyAliases);
            var systemId = this.GetAndRemoveWhenPresent(ref localCopy, SystemIdKeyAliases);
            var trace = this.GetAndRemoveWhenPresent(ref localCopy, TraceKeyAliases);
            var poolSize = this.GetAndRemoveWhenPresent(ref localCopy, PoolSizeKeyAliases);
            var saprouter = this.GetAndRemoveWhenPresent(ref localCopy, SapRouterKeyAliases);

            var sncMode = this.GetAndRemoveWhenPresent(ref localCopy, SncModeKeyAliases);
            var sncQop = this.GetAndRemoveWhenPresent(ref localCopy, SncQopKeyAliases);
            var sncMyName = this.GetAndRemoveWhenPresent(ref localCopy, SncMyNameKeyAliases);
            var sncPartner = this.GetAndRemoveWhenPresent(ref localCopy, SncPartnerKeyAliases);
            var sncLib = this.GetAndRemoveWhenPresent(ref localCopy, SncLibKeyAliases);

            this.FromValues(userName, password, host, language, client, systemNumber, systemId, trace, poolSize, saprouter, sncMode, sncQop, sncMyName, sncPartner, sncLib);

            foreach (var additionalParameter in localCopy.AllKeys)
            {
                this.parameters.SetParameter(additionalParameter, localCopy.Get(additionalParameter));
            }
        }

        internal RfcConnectionParameters Build() => this.parameters;
    }
}
