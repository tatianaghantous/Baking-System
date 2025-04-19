  namespace UAS.Keycloak; 

  /// <summary>
  /// Used to get specific claim type names.
  /// </summary>
  public static class ClaimTypes
  {
      /// <summary>
      /// Default: "preferred_username".
      /// </summary>
      public static string UserName { get; set; } = "preferred_username";

      /// <summary>
      /// Default: <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>
      /// </summary>
      public static string UserId { get; set; } = System.Security.Claims.ClaimTypes.NameIdentifier;

      /// <summary>
      /// Default: <see cref="System.Security.Claims.ClaimTypes.Role"/>
      /// </summary>
      public static string Role { get; set; } = System.Security.Claims.ClaimTypes.Role;
        
      public static string Group { get; set; } = "http://schemas.xmlsoap.org/claims/Group";
        
      /// <summary>
      /// Default: <see cref="System.Security.Claims.ClaimTypes.Email"/>
      /// </summary>
      public static string Email { get; set; } = System.Security.Claims.ClaimTypes.Email;

      /// <summary>
      /// Default: "email_verified".
      /// </summary>
      public static string EmailVerified { get; set; } = "email_verified";
        
      /// <summary>
      /// Default: "department".
      /// </summary>
      public static string Department { get; set; } = "department";

      /// <summary>
      /// Default: <see cref="System.Security.Claims.ClaimTypes.GivenName"/>
      /// </summary>
      public static string FirstName { get; set; } = System.Security.Claims.ClaimTypes.GivenName;
        
      /// <summary>
      /// Default: <see cref="System.Security.Claims.ClaimTypes.Surname"/>
      /// </summary>
      public static string LastName { get; set; } = System.Security.Claims.ClaimTypes.Surname;

      /// <summary>
      /// Default: "phone_number".
      /// </summary>
      public static string PhoneNumber { get; set; } = "phone_number";

      /// <summary>
      /// Default: "phone_number_verified".
      /// </summary>
      public static string PhoneNumberVerified { get; set; } = "phone_number_verified";

      /// <summary>
      /// Default: "tenantid".
      /// </summary>
      public static string BranchId { get; set; } = "branch-id";


      /// <summary>
      /// Default: "editionid".
      /// </summary>
      public static string EditionId { get; set; } = "editionid";

      /// <summary>
      /// Default: "client_id".
      /// </summary>
      public static string ClientId { get; set; } = "client_id";
        
      /// <summary>
      /// Default: "organization_id".
      /// </summary>
      public static string OrganizationId { get; set; } = "organization_id";
  }