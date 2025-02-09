namespace SoccerPlayerApi.Dtos.Facts
{
    public class ImportFactDto
    {
        public int RowNumber { get; set; }
        public decimal Amount { get; set; }
        public string DataType { get; set; }
        public string Dimension1Aggregation { get; set; }
        public string? Dimension2Aggregation { get; set; }
        public string? Dimension3Aggregation { get; set; }
        public string? Dimension4Aggregation { get; set; }
        public string TimeAggregation { get; set; }
    }
}
