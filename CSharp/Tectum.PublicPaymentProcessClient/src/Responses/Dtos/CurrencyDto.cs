namespace Tectum.PublicPaymentProcessClient.Responses.Dtos;

public sealed class CurrencyDto
{
    /// <summary>
    /// Currency key
    /// </summary>
    public string CurrencyKey { get; set; }

    /// <summary>
    /// Ticker
    /// </summary>
    public string Ticker { get; set; }

    /// <summary>
    /// Network
    /// </summary>
    public string Network { get; set; }
}
