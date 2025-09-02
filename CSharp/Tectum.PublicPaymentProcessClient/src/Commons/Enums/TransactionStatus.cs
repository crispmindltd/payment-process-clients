namespace Tectum.PublicPaymentProcessClient.Commons.Enums;

/// <summary>
/// Transaction status
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// Transaction has been initialized
    /// </summary>
    Init = 0,

    /// <summary>
    /// Wait complete transaction into the external environment
    /// </summary>
    Waiting = 1,

    /// <summary>
    /// Transaction was success finished
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Transaction failed
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Transaction was cancelled
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Transaction sending data to provider
    /// </summary>
    SendingToProvider = 5,
}
