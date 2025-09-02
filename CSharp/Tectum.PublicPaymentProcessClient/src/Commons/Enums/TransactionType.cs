namespace Tectum.PublicPaymentProcessClient.Commons.Enums;

/// <summary>
/// Transaction type enum
/// </summary>
public enum TransactionType
{
    None = 0,

    /// <summary>
    /// Common transaction
    /// </summary>
    Common = 1,

    /// <summary>
    /// Commission transaction
    /// </summary>
    Commission = 2,

    /// <summary>
    /// Reward transaction
    /// </summary>
    Rewards = 3,

    /// <summary>
    /// Swap transaction
    /// </summary>
    Swap = 4
}
