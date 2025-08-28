using Tectum.PublicPaymentProcessClient.Commons.Enums;

namespace Tectum.PublicPaymentProcessClient.Requests;

/// <summary>
/// Create output transaction
/// </summary>
public class CreateTransactionOutRequest
{
    /// <summary>
    /// Network 
    /// </summary>
    public Networks Network { get; set; }

    /// <summary>
    /// Currency ticker
    /// </summary>
    public string? Ticker { get; set; }

    /// <summary>
    /// Amount transaction
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// External id
    /// </summary>
    public Guid? ExternalId { get; set; }

    /// <summary>
    /// Request id
    /// </summary>
    public Guid RequestId { get; set; }
}
