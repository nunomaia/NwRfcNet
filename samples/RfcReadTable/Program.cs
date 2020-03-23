using CommandLine;
using NwRfcNet;
using NwRfcNet.Bapi;
using NwRfcNet.TypeMapper;
using System;

namespace Sample.RfcReadTable
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

        [Option('t', "table", Required = true, HelpText = "Table")]
        public string GlAccount { get; set; }

    }

    class Program
    {
        static void ParameterMapping()
        {
            var mapper = RfcMapper.DefaultMapper;

            mapper.Parameter<RfcParametersInput>()
                .Property(x => x.QueryTable)
                .HasParameterName("QUERY_TABLE")
                .HasParameterType(RfcFieldType.Char).MaxLength(60);

            mapper.Parameter<RfcParametersInput>()
                .Property(x => x.Delimiter)
                .HasParameterName("DELIMITER")
                .HasParameterType(RfcFieldType.Char).MaxLength(2);

            mapper.Parameter<RfcParametersInput>()
                .Property(x => x.No_Data)
                .HasParameterName("NO_DATA")
                .HasParameterType(RfcFieldType.Char).MaxLength(2);

            mapper.Parameter<RfcParametersInput>()
                .Property(x => x.RowCount)
                .HasParameterName("ROWCOUNT")
                .HasParameterType(RfcFieldType.Int);

            mapper.Parameter<RfcParametersInput>()
                .Property(x => x.RowSkips)
                .HasParameterName("ROWSKIPS")
                .HasParameterType(RfcFieldType.Int);

            mapper.Parameter<RfcParametersOutput>()
                .Property(x => x.Data)
                .HasParameterName("DATA")
                .HasParameterType(RfcFieldType.Table);
            
            mapper.Parameter<DataTable>()
                .Property(x => x.Wa)
                .HasParameterName("WA")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(512);
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
                               using (var func = conn.CallRfcFunction("RFC_READ_TABLE"))
                               {
                                   var inParams = new RfcParametersInput
                                   {
                                       QueryTable = "T001",
                                       Delimiter = ";",
                                       No_Data = "",
                                       RowCount = 0,
                                       RowSkips = 0
                                   };
                                    
                                   func.Invoke(inParams);

                                   var result = func.GetOutputParameters<RfcParametersOutput>();

                                   if (result.Data == null)
                                   {
                                       Console.WriteLine("No Data");
                                   }
                                   
                                   foreach(var r in result.Data)
                                   {
                                       Console.WriteLine(r.Wa);
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
