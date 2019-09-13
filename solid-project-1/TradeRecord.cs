namespace solid_project_1
{
    public class TradeRecord
    {
        public int Id { get; set; }
        public string DestinationCurrency { get; set; }
        public float Lots { get; set; }
        public decimal Price { get; set; }
        public string SourceCurrency { get; set; }
        public override string ToString()
        {
            return $"ID:{Id}, SourceCur:{SourceCurrency}, DestCur:{DestinationCurrency}, Number of Lots:{Lots}, Price:{Price}";
        }
    }
}