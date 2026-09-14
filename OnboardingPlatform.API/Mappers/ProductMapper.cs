using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Models;
using System.Text.Json;

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
            AdditionalFieldsSchema = product.AdditionalFieldsSchema != null
            ? JsonSerializer.Deserialize<object>(product.AdditionalFieldsSchema)
            : null
        };
    }
}
