using Tectum.PublicPaymentProcessClient.Commons.Enums;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class CreateTransactionOutResponse : BaseResponse
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; }

    /// <summary>
    /// Master address witch was blocked
    /// </summary>
    public string Address { get; set; }

    public TransactionStatus Status { get; set; }
}
