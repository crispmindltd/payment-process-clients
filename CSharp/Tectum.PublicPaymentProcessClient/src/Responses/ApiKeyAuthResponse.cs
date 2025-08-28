namespace Tectum.PublicPaymentProcessClient.Responses;

public class ApiKeyAuthResponse : BaseResponse
{
    public string JwtToken { get; set; } = string.Empty;
    public DateTime ExpiredAt { get; set; }
}
