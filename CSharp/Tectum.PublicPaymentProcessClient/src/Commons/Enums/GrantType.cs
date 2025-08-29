using Newtonsoft.Json;

namespace Tectum.PublicPaymentProcessClient.Commons.Enums;

/// <summary>
/// Jwt user type enum
/// </summary>
public enum GrantType
{
    [JsonProperty("client_credentials")]
    ClientCredentials = 0,
}
