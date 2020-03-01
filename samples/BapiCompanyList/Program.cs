using CommandLine;
using NwRfcNet;
using NwRfcNet.TypeMapper;
using System;

namespace Sample.BapiCompanyList
{
    class Options
    {
        [Option('u', "username", Required = true, HelpText = "RFC User Name")]
        public string UserName { get; set; }

        [Option('p', "password", Required = true, HelpText = "RFC User Password")]
        public string Password { get; set; }

        [Option('h', "hostname", Required = true, HelpText = "RFC Server Hostname")]
        public string Hostname { get; set; }

        [Option('c', "client", Required = true, HelpText = "RFC Server Client Id")]
        public string Client { get; set; }
    }

    class Program
    {
        static void ParameterMapping()
        {
            var mapper = RfcMapper.DefaultMapper;

            mapper.Parameter<BapiCompanyOutputParameters>()
                .Property(x => x.Details)
                .HasParameterName("COMPANYCODE_LIST")
                .HasParameterType(RfcFieldType.Table);

            mapper.Parameter<CompanyDetails>()
                .Property(x => x.CompanyCode)
                .HasParameterName("COMP_CODE")
                .MaxLength(4)
                .HasParameterType(RfcFieldType.Char);

            mapper.Parameter<CompanyDetails>()
                .Property(x => x.Name)
                .HasParameterName("COMP_NAME")
                .MaxLength(25)
                .HasParameterType(RfcFieldType.Char);
        }


        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                try
                {
                    var version = RfcConnection.GetLibVersion();
                    Console.WriteLine($"currently loaded sapnwrfc library version : Major {version.MajorVersion}, Minor {version.MinorVersion}, patchLevel {version.PatchLevel}");

                    using (var conn = new RfcConnection(builder => builder
                        .UseConnectionHost(o.Hostname)
                        .UseLogonUserName(o.UserName)
                        .UseLogonPassword(o.Password)
                        .UseLogonClient(o.Client)))
                    {
                        ParameterMapping();
                        conn.Open();
                        using (var func = conn.CallRfcFunction("BAPI_COMPANYCODE_GETLIST"))
                        {
                            func.Invoke();
                            var returnValue = func.GetOutputParameters<BapiCompanyOutputParameters>();
                            Console.WriteLine(String.Format("|{0,-20}|{1,-10}", "Company Code", "Company Name"));
                            foreach (var row in returnValue.Details)
                            {
                                Console.WriteLine(String.Format("|{0,-20}|{1,-10}", row.CompanyCode, row.Name));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
        }
    }
}
