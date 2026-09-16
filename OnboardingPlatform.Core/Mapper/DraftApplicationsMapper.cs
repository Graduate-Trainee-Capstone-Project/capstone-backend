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
                //CustomerId = draft.CustomerId,
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

        public static void ApplySave(this DraftApplication draft, InternalSaveDraftRequest request)
        {
            var existing = DeserializeFormData(draft.FormDataJson);
            var incoming = request.FormData;

            existing.FirstName = incoming.FirstName ?? existing.FirstName;
            existing.MiddleName = incoming.MiddleName ?? existing.MiddleName;
            existing.LastName = incoming.LastName ?? existing.LastName;
            existing.DateOfBirth = incoming.DateOfBirth ?? existing.DateOfBirth;
            existing.Gender = incoming.Gender ?? existing.Gender;
            existing.Email = incoming.Email ?? existing.Email;
            existing.PhoneNumber = incoming.PhoneNumber ?? existing.PhoneNumber;
            existing.Address = incoming.Address ?? existing.Address;
            existing.AccountType = incoming.AccountType ?? existing.AccountType;
            existing.InitialDeposit = incoming.InitialDeposit ?? existing.InitialDeposit;
            existing.Currency = incoming.Currency ?? existing.Currency;
            existing.PreferredBranch = incoming.PreferredBranch ?? existing.PreferredBranch;
            existing.Documents = incoming.Documents ?? existing.Documents;

            draft.FormDataJson = JsonSerializer.Serialize(existing, JsonOptions);

            if (!string.IsNullOrWhiteSpace(request.CurrentStep) &&
                Enum.TryParse<DraftStep>(request.CurrentStep, true, out var step))
                draft.CurrentStep = step;

            if (!string.IsNullOrWhiteSpace(request.Channel) &&
                Enum.TryParse<Channel>(request.Channel, true, out var channel))
                draft.Channel = channel;

            draft.LastUpdatedAt = DateTime.Now;
        }

        public static DraftFormData DeserializeFormData(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new DraftFormData();
            try
            {
                return JsonSerializer.Deserialize<DraftFormData>(json, JsonOptions) ?? new DraftFormData();
            }
            catch (JsonException)
            {
                return new DraftFormData(); // old/incompatible shape — ignore rather than 500
            }
        }
    }
}