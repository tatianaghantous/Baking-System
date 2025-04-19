using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UAS.Keycloak.Contracts;

namespace UAS.Keycloak;

public class CurrentContextAccessor : ICurrentContextAccessor
{
    private readonly IHttpContextAccessor _accessor;
    public CurrentContextAccessor(IHttpContextAccessor accessor) {
        _accessor = accessor;
    }
    public virtual ClaimsPrincipal? Principal => _accessor?.HttpContext?.User;
    public virtual HttpRequest? Request => _accessor?.HttpContext?.Request;
}