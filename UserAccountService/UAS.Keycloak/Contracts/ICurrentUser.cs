using System.Security.Claims;

namespace UAS.Keycloak.Contracts;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    Guid? Id { get; }
    long? BranchId { get; }
    string? Username { get; }
    string? Email { get; }
    string? Token { get; }
    List<string>? Roles { get; }
    Claim? FindClaim(string claimType);
    Claim[]? FindClaims(string claimType);

}