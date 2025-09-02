namespace Tectum.PublicPaymentProcessClient.Responses;

public class ApiKeyAuthResponse : BaseApiResponse
{
    public string JwtToken { get; set; } = string.Empty;
    public DateTime ExpiredAt { get; set; }
}
