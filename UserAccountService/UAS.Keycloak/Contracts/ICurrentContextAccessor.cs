using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UAS.Keycloak.Contracts;

public interface ICurrentContextAccessor
{
    ClaimsPrincipal? Principal { get; }
    HttpRequest? Request { get; }
}