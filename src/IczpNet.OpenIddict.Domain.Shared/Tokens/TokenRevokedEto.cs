namespace IczpNet.OpenIddict.Tokens;

/// <summary>Identifies the committed revocation without exposing token credentials.</summary>
public class TokenRevokedEto
{
    public bool All { get; set; }
    public string TokenId { get; set; }
    public string Subject { get; set; }
    public string AuthorizationId { get; set; }
}
