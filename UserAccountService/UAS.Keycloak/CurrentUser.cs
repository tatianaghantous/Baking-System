using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using UAS.Keycloak.Contracts;
using UAS.Keycloak.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace UAS.Keycloak;

public class CurrentUser : ICurrentUser
{
    private static readonly Claim[] EmptyClaimsArray = Array.Empty<Claim>();
    public virtual bool IsAuthenticated => Id.HasValue;
    public virtual Guid? Id { get; private set; }
    public virtual long? BranchId { get; private set; }
    public virtual string? Username { get; private set; }
    public virtual string? Email { get; private set; }
    public virtual string? Firstname { get; private set; }
    public virtual string? Lastname { get; private set; }
    public virtual string? PhoneNumber { get; private set; }
    public virtual List<string>? Roles { get; private set; }
    public virtual string? Token { get; private set; }
    private ClaimsPrincipal? Prinicpal { get; set; }

    private readonly IdentitySettings _settings;
        
    public CurrentUser(ICurrentContextAccessor contextAccessor, IdentitySettings settings)
    {
        _settings = settings;
        if (contextAccessor.Request != null)
        {
            Token = GetToken(contextAccessor.Request);
            Prinicpal = contextAccessor.Principal;
            SetCurrentUser();
        }
    }

    private void SetCurrentUser()
    {
        var claims = Prinicpal?.Claims;
        if (claims != null) Id = FindUserId(claims);
        Username = FindClaimValue(this, ClaimTypes.UserName);
        Email = FindClaimValue(this,ClaimTypes.Email);
        PhoneNumber = FindClaimValue(this,ClaimTypes.PhoneNumber);
        Firstname = FindClaimValue(this,ClaimTypes.FirstName);
        Lastname = FindClaimValue(this,ClaimTypes.LastName);
        Roles = FindClaims(ClaimTypes.Role).Select(x => x.Value).ToList();
        BranchId = Convert.ToInt64(FindClaimValue(this,ClaimTypes.BranchId));
    }
    public static Guid? FindUserId(IEnumerable<Claim> claims)
    {
        var claimsList = claims.ToList();
        CheckExtensions.NotNull(claimsList, nameof(claimsList));

        var userIdOrNull = claimsList?.FirstOrDefault(c => c.Type == ClaimTypes.UserId);
        if (userIdOrNull == null || string.IsNullOrWhiteSpace(userIdOrNull.Value)) //corrected
        {
            return null;
        }

        return Guid.Parse(userIdOrNull.Value);
    }
    public static string? FindClaimValue(ICurrentUser currentUser, string claimType)
    {
        return currentUser.FindClaim(claimType)?.Value;
    }
    public static class CheckExtensions // by Me, check it
    {
        public static void NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null.");
            }
        }
    }
    public virtual Claim? FindClaim(string claimType)
    {
        return Prinicpal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }

    public virtual Claim[] FindClaims(string claimType)
    {
        return Prinicpal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
    }
        
    public bool IsInRole(string roleName)
    {
        return Roles != null && Roles.Any(c => c == roleName);
    }
    
    public static string GetToken(HttpRequest request)
    {
        if (request != null 
            && request.Headers.ContainsKey("Authorization") 
            && request.Headers["Authorization"].First().StartsWith("Bearer "))
        {
            return request.Headers["Authorization"].First()["Bearer ".Length..];
        }
        return string.Empty;
    }

}