using CommandLine;
using NwRfcNet;
using NwRfcNet.TypeMapper;
using System;

namespace Sample.BapiCustomerDetail
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

        [Option('k', "customer", Required = true, HelpText = "Customer Number")]
        public string Customer { get; set; }
    }
    class Program
    {
        static void ParameterMapping()
        {
            var mapper = RfcMapper.DefaultMapper;

            mapper.Parameter<CustomerDataInput>()
                .Property(x => x.Customer)
                .HasParameterName("CUSTOMERNO")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(10);

            mapper.Parameter<CustomerDataOutput>()
                .Property(x => x.Detail)
                .HasParameterName("CUSTOMERGENERALDETAIL")
                .HasParameterType(RfcFieldType.Structure);

            mapper.Parameter<CustomerDetail>()
                .Property(x => x.Customer)
                .HasParameterName("CUSTOMER")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(10);

            mapper.Parameter<CustomerDetail>()
                .Property(x => x.CreateDate)
                .HasParameterName("CREAT_DATE")
                .HasParameterType(RfcFieldType.Date)
                .MaxLength(10);

            mapper.Parameter<CustomerDetail>()
                .Property(x => x.ConfirmDate)
                .HasParameterName("CONF_DATE")
                .HasParameterType(RfcFieldType.Date)
                .MaxLength(10);

            mapper.Parameter<CustomerDetail>()
                .Property(x => x.ConfirmationTime)
                .HasParameterName("CONF_TIME")
                .HasParameterType(RfcFieldType.Time);
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
                               using (var func = conn.CallRfcFunction("BAPI_CUSTOMER_GETDETAIL2"))
                               {
                                   var inParams = new CustomerDataInput{ Customer = o.Customer };
                                   func.Invoke(inParams);

                                   var result = func.GetOutputParameters<CustomerDataOutput>();

                                   Console.WriteLine(String.Format("|{0,-20}|{1,-20}|{2,-20}|{3,-20}", "Customer Number", "Create Date", "Confirm Date", "Confirm Time"));

                                   Console.WriteLine(String.Format("|{0,-20}|{1,-20}|{2,-20}|{3,-20}", 
                                       result.Detail.Customer,
                                       result.Detail.CreateDate?.ToString("yyyy-MM-dd") ?? "Not Defined",
                                       result.Detail?.ConfirmDate?.ToString("yyyy-MM-dd") ?? "Not Defined",
                                       result.Detail?.ConfirmationTime?.ToString() ?? "Not Defined")
                                       );
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
