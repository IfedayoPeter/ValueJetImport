namespace ValueJetImport.Model
{
    public class ProcessedResult
    {
        public string Origin { get; set; }
        public DateTime FlightDate { get; set; }
        public decimal SumOfAmount { get; set; }
        public decimal SumOfYQ { get; set; }
        public int DistinctCountOfTktnbr { get; set; }
        public decimal NG { get; set; }
        public decimal QTRate { get; set; }
        public decimal QT { get; set; }
    }
}
