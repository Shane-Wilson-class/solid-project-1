namespace solid_project_1
{
    public record TradeRecord
    {
        public int Id { get; set; }
        public string DestinationCurrency { get; init; }
        public float Lots { get; init; }
        public decimal Price { get; init; }
        public string SourceCurrency { get; init; }
    }
}
