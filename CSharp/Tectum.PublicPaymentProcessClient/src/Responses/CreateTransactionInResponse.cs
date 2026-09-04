using Tectum.PublicPaymentProcessClient.Commons.Enums;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class CreateTransactionInResponse : BaseApiResponse
{
    /// <summary>
    /// Payment intent identifier. Id is retained for backward compatibility.
    /// </summary>
    public Guid PaymentIntentId { get; set; }

    public Guid Id { get; set; }
    public string ExternalId { get; set; }

    /// <summary>
    /// Authoritative destination address reserved by Payments.
    /// Address is retained for backward compatibility.
    /// </summary>
    public string AddressTo { get; set; }

    /// <summary>
    /// Master address witch was blocked
    /// </summary>
    public string Address { get; set; }

    public DateTime ExpiresAt { get; set; }

    public TransactionStatus Status { get; set; }
}
