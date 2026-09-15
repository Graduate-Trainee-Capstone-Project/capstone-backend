namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class StockBrokingAccountDetailResponse
    {
        public Guid StockBrokingAccountId { get; set; }
        public Guid CustomerProductId { get; set; }
        public string CscsNumber { get; set; } = string.Empty;
        public string BrokerageFirm { get; set; } = string.Empty;
        public string TradingAccountNumber { get; set; } = string.Empty;
        public DateOnly DateOpened { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}