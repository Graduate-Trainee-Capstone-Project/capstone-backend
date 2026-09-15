using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Models;
using System.Text.Json;

namespace OnboardingPlatform.Core.Mappers
{
    public static class DraftApplicationMapper
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static DraftApplicationResponse ToResponse(this DraftApplication draft)
        {
            return new DraftApplicationResponse
            {
                DraftId = draft.DraftId,
                ProductId = draft.ProductId,
                CustomerId = draft.CustomerId,
                PrimaryIdentifierType = draft.PrimaryIdentifierType.ToString(),
                SecondaryIdentifierType = draft.SecondaryIdentifierType?.ToString(),
                CurrentStep = draft.CurrentStep.ToString(),
                FormData = DeserializeFormData(draft.FormDataJson),
                Channel = draft.Channel.ToString(),
                Status = draft.Status.ToString(),
                CreatedAt = draft.CreatedAt,
                LastUpdatedAt = draft.LastUpdatedAt,
                ExpiresAt = draft.ExpiresAt
            };
        }

        public static SaveDraftResponse ToSaveResponse(this DraftApplication draft)
        {
            return new SaveDraftResponse
            {
                DraftId = draft.DraftId,
                CurrentStep = draft.CurrentStep.ToString(),
                Status = draft.Status.ToString(),
                LastUpdatedAt = draft.LastUpdatedAt
            };
        }

        // Merges incoming fields onto the existing FormDataJson instead of
        // overwriting it — each wizard step only sends its own fields.
        public static void ApplySave(this DraftApplication draft, SaveDraftRequest request)
        {
            var existing = DeserializeFormData(draft.FormDataJson);
            foreach (var kvp in request.FormData)
                existing[kvp.Key] = kvp.Value;

            draft.FormDataJson = JsonSerializer.Serialize(existing, JsonOptions);

            if (!string.IsNullOrWhiteSpace(request.CurrentStep) &&
                Enum.TryParse<DraftStep>(request.CurrentStep, true, out var step))
                draft.CurrentStep = step;

            if (!string.IsNullOrWhiteSpace(request.Channel) &&
                Enum.TryParse<Channel>(request.Channel, true, out var channel))
                draft.Channel = channel;

            draft.LastUpdatedAt = DateTime.Now;
        }

        public static Dictionary<string, object?> DeserializeFormData(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new Dictionary<string, object?>();
            return JsonSerializer.Deserialize<Dictionary<string, object?>>(json, JsonOptions)
                   ?? new Dictionary<string, object?>();
        }
    }
}
