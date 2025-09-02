using Tectum.PublicPaymentProcessClient.Requests;
using Tectum.PublicPaymentProcessClient.Responses;

namespace Tectum.PublicPaymentProcessClient;

public interface IPaymentProcessApiClient : IDisposable
{
    /// <summary>
    /// Get all active currencies
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<GetCurrenciesResponse?> GetCurrenciesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Get all balances for user
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<GetBalancesResponse?> GetBalancesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Get all transactions for user
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<GetTransactionsResponse?> GetTransactionsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Get one user's transaction
    /// </summary>
    /// <param name="transactionId">Transaction identificator</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<GetTransactionResponse?> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken);

    /// <summary>
    /// Create input transaction
    /// </summary>
    /// <param name="request">Data of transaction</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<CreateTransactionInResponse?> CreateTransactionInAsync(CreateTransactionInRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Create outgoing transaction
    /// </summary>
    /// <param name="request">Data of outgoing transaction</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<CreateTransactionOutResponse?> CreateTransactionOutAsync(CreateTransactionOutRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Confirm one transaction and start waiting
    /// </summary>
    /// <param name="transactionId">Transaction identificator</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<TransactionConfirmResponse?> ConfirmTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default);
}
