using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Enums;

public enum IdentifierType { BVN, NIN, EMAIL, PHONE }

public enum ProductCode { SAVINGS, CURRENT, PENSION_RSA, STOCKBROKING, INSURANCE }

public enum DraftStep
{
    IDENTIFIER_CAPTURE,
    IDENTITY_CHECK,
    PERSONAL_INFO,
    PRODUCT_SPECIFIC_INFO,
    SECURITY_VERIFICATION,
    DOCUMENT_UPLOAD,
    REVIEW,
    SUBMITTED
}

public enum DraftStatus { IN_PROGRESS, SUBMITTED, ABANDONED, EXPIRED }

public enum Channel { WEB, MOBILE, USSD, BRANCH_ASSISTED }

public enum CustomerProductStatus { PENDING, ACTIVE, REJECTED, CLOSED }

public enum CustomerStatus { ACTIVE, SUSPENDED, CLOSED }

public enum SecurityCheckType { SECURITY_QUESTION, FACIAL_RECOGNITION, OTP }

public enum SecurityCheckStatus { PENDING, PASSED, FAILED }

public enum AccountStatus { Active, Dormant, Closed }

public enum ContributionScheme { MandatoryCPS, Voluntary, MicroPensionPlan }

public enum DocumentType
{
    NATIONAL_ID_CARD,
    PASSPORT,
    DRIVERS_LICENSE,
    UTILITY_BILL,
    PASSPORT_PHOTOGRAPH,
    SIGNATURE
}
