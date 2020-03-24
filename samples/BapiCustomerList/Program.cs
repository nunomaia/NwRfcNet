using CommandLine;
using NwRfcNet;
using NwRfcNet.TypeMapper;
using System;

namespace Sample.BapiCustomerList
{
    class Options
    {
        [Option('c', "connection", Required = true, HelpText = "Connection String")]
        public string Connection { get; set; }
    }

    class Program
    {
        static void ParameterMapping()
        {
            var mapper = RfcMapper.DefaultMapper;

            mapper.Parameter<ListCustomersInputParameters>()
                .Property(x => x.MaxRows)
                .HasParameterName("MAXROWS")
                .HasParameterType(RfcFieldType.Int);

            mapper.Parameter<ListCustomersInputParameters>()
                .Property(x => x.Range)
                .HasParameterName("IDRANGE")
                .HasParameterType(RfcFieldType.Table);

            mapper.Parameter<IdRange>()
                .Property(x => x.Sign)
                .HasParameterName("SIGN")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(1);

            mapper.Parameter<IdRange>()
                .Property(x => x.Option)
                .HasParameterName("OPTION")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(2);

            mapper.Parameter<IdRange>()
                .Property(x => x.Low)
                .HasParameterName("LOW")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(10);

            mapper.Parameter<IdRange>()
                .Property(x => x.High)
                .HasParameterName("HIGH")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(10);

            mapper.Parameter<ListCustomersOutputParameters>()
                .Property(x => x.Addresses)
                .HasParameterName("ADDRESSDATA")
                .HasParameterType(RfcFieldType.Table);

            mapper.Parameter<CustomerAddress>()
                .Property(x => x.CustomerId)
                .HasParameterName("CUSTOMER")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(10);

            mapper.Parameter<CustomerAddress>()
                .Property(x => x.Name)
                .HasParameterName("NAME")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(40);
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

                           using var conn = new RfcConnection(o.Connection);
                           conn.Open();
                           using var func = conn.CallRfcFunction("BAPI_CUSTOMER_GETLIST");
                           var inParams = new ListCustomersInputParameters
                           {
                               MaxRows = 10,
                               Range = new IdRange[]
{
                                            new IdRange() {   Sign = "I", Option = "BT", High = "ZZZZZZZZZZ" }
}
                           };

                           func.Invoke(inParams);
                           var returnValue = func.GetOutputParameters<ListCustomersOutputParameters>();

                           Console.WriteLine(String.Format("|{0,-20}|{1,-10}", "Customer Number", "Customer Name"));
                           foreach (var row in returnValue.Addresses)
                           {
                               Console.WriteLine(String.Format("|{0,-20}|{1,-10}", row.CustomerId, row.Name));
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
