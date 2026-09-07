using Tectum.PublicPaymentProcessClient.Commons.Enums;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class TransactionConfirmResponse : BaseApiResponse
{
    /// <summary>
    /// Transaction identificator
    /// </summary>
    public Guid Id { get; set; }

    public Guid PaymentIntentId { get; set; }

    public string AddressTo { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Transaction status
    /// </summary>
    public TransactionStatus Status { get; set; }
}
