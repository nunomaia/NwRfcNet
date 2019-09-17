namespace NwRfcNet.Sample
{
    public class BapiCompanyOutputParameters
    {
        public CompanyDetails[] Details { get; set; }
    }

    public class CompanyDetails
    {
        public string CompanyCode { get; set; }

        public string Name { get; set; }
    }
}
