using CommandLine;
using NwRfcNet;
using NwRfcNet.Bapi;
using NwRfcNet.TypeMapper;
using System;

namespace Sample.BapiGetGlAccountBalance
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

        [Option('a', "account", Required = true, HelpText = "G/L Account")]
        public string GlAccount { get; set; }

        [Option('k', "company", Required = true, HelpText = "FI Company Code")]
        public string Company { get; set; }

        [Option('f', "fiscalyear", Required = true, HelpText = "FI Fiscal Year")]
        public int FiscalYear { get; set; }
    }

    class Program
    {
        static void ParameterMapping()
        {
            var mapper = RfcMapper.DefaultMapper;

            mapper.Parameter<BapiParametersInput>()
                .Property(x => x.CompanyCode)
                .HasParameterName("COMPANYCODE")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(4);

            mapper.Parameter<BapiParametersInput>()
                .Property(x => x.GlAccount)
                .HasParameterName("GLACCT")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(10);

            mapper.Parameter<BapiParametersInput>()
                .Property(x => x.FiscalYear)
                .HasParameterName("FISCALYEAR")
                .HasParameterType(RfcFieldType.Int);

            mapper.Parameter<BapiParametersInput>()
                .Property(x => x.CurrencyType)
                .HasParameterName("CURRENCYTYPE")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(2);

            mapper.Parameter<BapiParametersOutput>()
                .Property(x => x.Detail)
                .HasParameterName("ACCOUNT_BALANCES")
                .HasParameterType(RfcFieldType.Table);

            mapper.MapBapiReturn();

            mapper.Parameter<BapiParametersOutput>()
                .Property(x => x.BapiReturn)
                .HasParameterName("RETURN")
                .HasParameterType(RfcFieldType.Structure);

            mapper.Parameter<GlBalanceDetail>()
                .Property(x => x.CompanyCode)
                .HasParameterName("COMP_CODE")
                .MaxLength(4)
                .HasParameterType(RfcFieldType.Char);

            mapper.Parameter<GlBalanceDetail>()
                .Property(x => x.GlAccount)
                .HasParameterName("GL_ACCOUNT")
                .MaxLength(10)
                .HasParameterType(RfcFieldType.Char);

            mapper.Parameter<GlBalanceDetail>()
                .Property(x => x.FiscalYear)
                .HasParameterName("FISC_YEAR")
                .HasParameterType(RfcFieldType.Int);

            mapper.Parameter<GlBalanceDetail>()
                .Property(x => x.FiscalPeriod)
                .HasParameterName("FIS_PERIOD")
                .HasParameterType(RfcFieldType.Int);

            mapper.Parameter<GlBalanceDetail>()
                .Property(x => x.TotalDebit)
                .HasParameterName("DEBITS_PER")
                .HasParameterType(RfcFieldType.Bcd);

            mapper.Parameter<GlBalanceDetail>()
                .Property(x => x.TotalCredit)
                .HasParameterName("CREDIT_PER")
                .HasParameterType(RfcFieldType.Bcd);

            mapper.Parameter<GlBalanceDetail>()
                .Property(x => x.MontlySales)
                .HasParameterName("PER_SALES")
                .HasParameterType(RfcFieldType.Bcd);

            mapper.Parameter<GlBalanceDetail>()
                .Property(x => x.Currency)
                .HasParameterName("CURRENCY")
                .MaxLength(10)
                .HasParameterType(RfcFieldType.Char);

        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       try
                       {
                           ParameterMapping();

                           var version = RfcConnection.GetLibVersion();
                           Console.WriteLine($"currently loaded sapnwrfc library version : Major {version.MajorVersion}, Minor {version.MinorVersion}, patchLevel {version.PatchLevel}");

                           using (var conn = new RfcConnection(builder => builder
                                .UseConnectionHost(o.Hostname)
                                .UseLogonUserName(o.UserName)
                                .UseLogonPassword(o.Password)
                                .UseLogonClient(o.Client)))
                           {
                               conn.Open();
                               using (var func = conn.CallRfcFunction("BAPI_GL_GETGLACCPERIODBALANCES"))
                               {
                                   var inParams = new BapiParametersInput
                                   {
                                       CompanyCode = o.Company,
                                       CurrencyType = "10", // document Currency
                                       FiscalYear = o.FiscalYear,
                                       GlAccount = o.GlAccount
                                   };
                                   func.Invoke(inParams);

                                   var result = func.GetOutputParameters<BapiParametersOutput>();

                                   Console.WriteLine(String.Format("|{0,-6}|{1,-6}|{2,-10}|{3,-10}", "Company", "Period", "Debit", "Credit"));
                                   foreach (var row in result.Detail)
                                   {
                                       Console.WriteLine(String.Format("|{0,-6}|{1,-6}|{2,-10}|{3,-10}",
                                           row.CompanyCode,
                                           row.FiscalPeriod,
                                           row.TotalCredit.ToString(),
                                           row.TotalDebit.ToString()
                                           )); ;
                                   }

                               }
                           }
                       }
                       catch (Exception ex)
                       {
                           Console.WriteLine(ex.Message);
                           throw ex;
                       }
                   });

        }
    }
}
