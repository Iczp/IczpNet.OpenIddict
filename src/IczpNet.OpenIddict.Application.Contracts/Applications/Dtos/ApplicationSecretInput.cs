namespace IczpNet.OpenIddict.Applications.Dtos;

public class ApplicationSecretInput
{
    /// <summary>
    /// Gets or sets the client identifier associated with the application.
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Gets or sets the client secret associated with the application.
    /// Note: depending on the application manager used when creating it,
    /// this property may be hashed or encrypted for security reasons.
    /// </summary>
    public string ClientSecret { get; set; }
}

