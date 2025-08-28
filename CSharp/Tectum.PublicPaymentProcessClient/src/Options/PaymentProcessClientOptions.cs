namespace Tectum.PublicPaymentProcessClient.Options;

/// <summary>
/// Configuration of connection to payment process API
/// </summary>
public class PaymentProcessClientOptions
{
    public static PaymentProcessClientOptions Default { get; } = new PaymentProcessClientOptions();

    /// <summary>
    /// Base url to payment process API
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.payments.softnote.com/";

    /// <summary>
    /// Client id for login
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Client secret key
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// Timespan in second to wait before the request times out.
    /// </summary>
    public int TimeoutInSeconds { get; set; } = 30;

    /// <summary>
    /// Authoization retry delay in seconds
    /// </summary>
    public int AuthRetryDelayInSeconds { get; set; } = 30;

    public PaymentProcessClientOptions Copy()
    {
        return new PaymentProcessClientOptions
        {
            BaseUrl = BaseUrl,
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            TimeoutInSeconds = TimeoutInSeconds,
            AuthRetryDelayInSeconds = AuthRetryDelayInSeconds
        };
    }
}
