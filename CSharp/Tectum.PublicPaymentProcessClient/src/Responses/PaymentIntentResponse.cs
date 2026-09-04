using Tectum.PublicPaymentProcessClient.Commons.Enums;

namespace Tectum.PublicPaymentProcessClient.Responses;

/// <summary>
/// Authoritative Payments state used by trusted services before submitting a transfer.
/// </summary>
public sealed class PaymentIntentResponse : BaseApiResponse
{
    public Guid PaymentIntentId { get; set; }
    public TransactionStatus Status { get; set; }
    public string AddressTo { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Networks Network { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
