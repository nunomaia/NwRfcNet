using System;

namespace Sample.BapiCustomerDetail
{
    public class CustomerDataInput
    {
        public string Customer { get; set; }
    }

    public class CustomerDataOutput
    {
        public CustomerDetail Detail { get; set; }
    }

    public class CustomerDetail
    {
        public string Customer { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? ConfirmDate { get; set; }

        public TimeSpan? ConfirmationTime { get; set; }
    }
}
