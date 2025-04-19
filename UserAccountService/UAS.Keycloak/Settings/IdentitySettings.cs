using System.ComponentModel.DataAnnotations;
using NHibernate.Classic;

namespace UAS.Keycloak.Settings;

/// <summary>
///     Class to read identity settings from the appsettings file
/// </summary>
public class IdentitySettings : IValidatable
{
    [Required] public string BaseUrl { get; set; }
    [Required] public string Realm { get; set; }
    [Required] public string Audience { get; set; }

    public string MetadataAddress => $"{Authority}/.well-known/openid-configuration";
   // public string Authority => $"{BaseUrl.RemovePostFix("/")}/auth/realms/{Realm}";
   public string Authority => $"{BaseUrl.TrimEnd('/')}/realms/{Realm}"; // corrected, original one is the previous one


    /// <inherit />
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}