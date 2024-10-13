namespace TimeOrganizer_net_core.helper;

public enum ServiceResultErrorType
{
    AuthenticationFailed,
    UserLockedOut,
    EmailNotConfirmed,
    IdentityError,
    NotFound,
    BadRequest,
    Conflict,
    InternalServerError,
    TwoFactorAuthRequired,
    InvalidTwoFactorAuthToken,
}