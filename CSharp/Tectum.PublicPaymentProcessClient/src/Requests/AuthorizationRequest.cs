using Tectum.PublicPaymentProcessClient.Commons.Enums;

namespace Tectum.PublicPaymentProcessClient.Requests;

/// <summary>
/// Authorization request
/// </summary>
public class AuthorizationRequest
{
    public AuthorizationRequest() 
    {
    }

    public AuthorizationRequest(string clientId, string apiKey, GrantType grantType)
    {
        ClientId = clientId;
        ApiKey = apiKey;
        GrantType = grantType;
    }

    /// <summary>
    /// Client id
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Secret key for user
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// User type
    /// </summary>
    public GrantType GrantType { get; set; }
}
