using NwRfcNet.Bapi;

namespace Sample.BapiCustomerList
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


    public class ListCustomersOutputParameters
    {
        public BapiReturn Return { get; set; }

        public CustomerAddress[] Addresses { get; set; }
    }
}
