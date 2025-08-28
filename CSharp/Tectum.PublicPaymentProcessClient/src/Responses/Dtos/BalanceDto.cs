namespace Tectum.PublicPaymentProcessClient.Responses.Dtos;

/// <summary>
/// Object of balance
/// </summary>
public sealed class BalanceDto
{
    /// <summary>
    /// Name crypto: ETH, USDT
    /// </summary>
    public string Ticker { get; set; }

    /// <summary>
    /// Network
    /// </summary>
    public string Network { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    public decimal Amount { get; set; }
}
