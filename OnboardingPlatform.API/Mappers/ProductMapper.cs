using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Models;
using System.Text.Json;
using OnboardingPlatform.Utilities;

namespace OnboardingPlatform.API.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponse ToResponse(Product product) => new()
        {
            ProductId = product.ProductId,
            ProductCode = product.ProductCode.ToString(),
            ProductName = product.ProductName,
            RequiredIdentifiers = JsonSerializer.Deserialize<List<string>>(product.RequiredIdentifiers) ?? new(),
            AdditionalFieldsSchema = string.IsNullOrWhiteSpace(product.AdditionalFieldsSchema)
                ? null
                : JsonDocument.Parse(product.AdditionalFieldsSchema).RootElement.ToObject()
        };
    }
}