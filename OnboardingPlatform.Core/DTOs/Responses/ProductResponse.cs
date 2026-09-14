namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public List<string> RequiredIdentifiers { get; set; } = new();
        public object? AdditionalFieldsSchema { get; set; }
    }
}
