using NwRfcNet.TypeMapper;
using System;
using System.Collections.Generic;

namespace NwRfcNet.Sample
{
    public class BapiDemo
    {
        private readonly RfcConnection _conn;

        public BapiDemo(RfcConnection conn) => _conn = conn ?? throw new ArgumentNullException(nameof(conn));

        public void ListCompanies()
        {
            using(var func = _conn.CallRfcFunction("BAPI_COMPANYCODE_GETLIST"))
            {
                Console.WriteLine($"Executing {func.FunctionName} ..... ");

                RfcMapper mapper = new RfcMapper();

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

                _conn.Mapper = mapper;
                func.Invoke();
                var returnValue =  func.GetOutputParameters<BapiCompanyOutputParameters>();

                Console.WriteLine(String.Format("|{0,-20}|{1,-10}", "Company Code", "Company Name"));
                foreach (var row in returnValue.Details)
                {
                    Console.WriteLine(String.Format("|{0,-20}|{1,-10}", row.CompanyCode, row.Name));
                }
            }
        }

        public void ListCustomers()
        {
            using (var func = _conn.CallRfcFunction("BAPI_CUSTOMER_GETLIST"))
            {
                Console.WriteLine($"Executing {func.FunctionName} ..... ");

                RfcMapper mapper = new RfcMapper();

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

                var inParams = new ListCustomersInputParameters
                {
                    MaxRows = 10,
                    Range = new IdRange[]
                    {
                        new IdRange() {   Sign = "I", Option = "BT", High = "ZZZZZZZZZZ" }
                    }
                };

                _conn.Mapper = mapper;
                func.Invoke(inParams);
                var returnValue = func.GetOutputParameters<ListCustomersOutputParameters>();

                Console.WriteLine(String.Format("|{0,-20}|{1,-10}", "Customer Number", "Customer Name"));
                foreach (var row in returnValue.Addresses)
                {
                    Console.WriteLine(String.Format("|{0,-20}|{1,-10}", row.CustomerId, row.Name));
                }

            }
        }

        public void Ping(string hostname)
        {
            Console.WriteLine($"Pinging {hostname} ..... ");
            _conn.Ping();
            Console.WriteLine($"Success ...");
        }
    }
}
