using Tectum.PublicPaymentProcessClient.Commons.Enums;

namespace Tectum.PublicPaymentProcessClient.Responses.Dtos;

public sealed class TransactionDto
{
    /// <summary>
    /// Unique identifier for the transaction
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Transaction amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency identifier ticker
    /// </summary>
    public string? Ticker { get; set; }

    /// <summary>
    /// Network identifier
    /// </summary>
    public Networks Network { get; set; }

    /// <summary>
    /// Timestamp when transaction was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// External transaction ID (from payment processor or blockchain)
    /// </summary>
    public string? ExternalId { get; set; }

    /// <summary>
    /// Transaction type: Common (regular) or Commission (fee transaction)
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Transaction status
    /// </summary>
    public TransactionStatus Status { get; set; }

    /// <summary>
    /// Sender address (blockchain address)
    /// </summary>
    public string? AddressFrom { get; set; }

    /// <summary>
    /// Recipient address (blockchain address)
    /// </summary>
    public string? AddressTo { get; set; }

    /// <summary>
    /// Fee amount for the transaction
    /// </summary>
    public decimal? FeeAmount { get; set; }

    /// <summary>
    /// Currency of the fee ticker
    /// </summary>
    public string? FeeTicker { get; set; }

    /// <summary>
    /// Transaction direction: In (deposit) or Out (withdrawal)
    /// </summary>
    public TransactionDirection Direction { get; set; }

    /// <summary>
    /// Group ID for transactions that belong together (like multi-step payments)
    /// </summary>
    public Guid? GroupId { get; set; }

    /// <summary>
    /// Blockchain transaction hash
    /// </summary>
    public string? Hash { get; set; }

    /// <summary>
    /// Error code if transaction failed
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Error message if transaction failed
    /// </summary>
    public string? ErrorMsg { get; set; }
}
