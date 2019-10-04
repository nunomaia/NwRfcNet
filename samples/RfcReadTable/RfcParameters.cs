namespace Sample.RfcReadTable
{
    public class RfcParametersInput
    {
        public string QueryTable { get; set; }
        
        public string Delimiter { get; set; }
        
        public string No_Data { get; set; }
        
        public int RowSkips { get; set; }
        
        public int RowCount { get; set; }
    }

    public class RfcParametersOutput
    {
        public DataTable[] Data { get; set; }
    }

    public class DataTable
    {
        public string Wa { get; set; }
    }
}
