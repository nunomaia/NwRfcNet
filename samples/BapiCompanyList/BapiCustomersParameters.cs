using System.Collections.Generic;

namespace NwRfcNet.Sample
{
    public class ListCustomersInputParameters
    {
        public int MaxRows { get; set; }

        public IdRange[] Range { get; set; }
    }

    public class IdRange
    {
        public string Sign { get; set; }
        public string Option { get; set; }
        public string Low { get; set; }
        public string High { get; set; }
    }

    public class CustomerAddress
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
    }

    public class BapiReturn
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Number { get; set; }
        public string Message { get; set; }
        public string LogNumber { get; set; }
        public string LogMsgNumber { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public string Message3 { get; set; }
        public string Message4 { get; set; }
    }


    public class ListCustomersOutputParameters
    {
        public BapiReturn Return { get; set; }

        public CustomerAddress[] Addresses { get; set; }
    }
}
