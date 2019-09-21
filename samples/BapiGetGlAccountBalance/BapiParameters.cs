using NwRfcNet.Bapi;

namespace Sample.BapiGetGlAccountBalance
{
    public class BapiParametersInput
    {
        public string CompanyCode { get; set; }

        public string GlAccount { get; set; }

        public int FiscalYear { get; set; }

        public string CurrencyType { get; set; }
    }

    public class BapiParametersOutput
    {
        public GlBalanceDetail[] Detail { get; set; }

        public BapiReturn BapiReturn { get; set; }
    }

    public class GlBalanceDetail
    {
        public string CompanyCode { get; set; }

        public string GlAccount { get; set; }

        public int FiscalYear { get; set; }

        public int FiscalPeriod { get; set; }

        public decimal TotalDebit { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal MontlySales { get; set; }
        
        public decimal AccountBalance { get; set; }

        public string Currency { get; set; }
    }
}
