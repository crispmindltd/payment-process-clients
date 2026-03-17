using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tectum.PublicPaymentProcessClient.Options;
using Tectum.PublicPaymentProcessClient.Requests;
using Tectum.PublicPaymentProcessClient.Responses;

namespace Tectum.PublicPaymentProcessClient;

public class PaymentProcessClient : BaseHttpClient, IPaymentProcessApiClient
{
    /// <summary>
    /// Payment process client with options configuration via delegate
    /// </summary>
    /// <param name="optionsDelegate">Delegate for configuring client options</param>
    public PaymentProcessClient(Action<PaymentProcessClientOptions> optionsDelegate)
        : base(null, optionsDelegate)
    {
    }

    /// <summary>
    /// Payment process client with explicit authentication credentials
    /// </summary>
    /// <param name="clientId">Client identifier for authentication</param>
    /// <param name="clientSecretKey">Client secret key for authentication</param>
    public PaymentProcessClient(string clientId, string clientSecretKey) : base (null, clientId, clientSecretKey)
    {
    }


    /// <summary>
    /// Payment process client with custom HttpClient and options configuration
    /// </summary>
    /// <param name="httpClient">Custom HttpClient instance</param>
    /// <param name="optionsDelegate">Delegate for configuring client options</param>
    public PaymentProcessClient(HttpClient httpClient, Action<PaymentProcessClientOptions> optionsDelegate)
        : base(httpClient, optionsDelegate)
    {
    }

    /// <summary>
    /// Payment process client with IOptions configuration
    /// </summary>
    /// <param name="options">Client configuration options</param>
    /// <param name="jsonSerializerSettings">Custom JSON serialization settings (optional)</param>
    public PaymentProcessClient(IOptions<PaymentProcessClientOptions> options, JsonSerializerSettings? jsonSerializerSettings = null)
        : base(null, options, jsonSerializerSettings)
    {
    }

    /// <summary>
    /// Payment process client with custom HttpClient and IOptions configuration
    /// </summary>
    /// <param name="httpClient">Custom HttpClient instance</param>
    /// <param name="options">Client configuration options</param>
    /// <param name="jsonSerializerSettings">Custom JSON serialization settings (optional)</param>
    public PaymentProcessClient(HttpClient httpClient, IOptions<PaymentProcessClientOptions> options, JsonSerializerSettings? jsonSerializerSettings = null)
        : base(httpClient, options, jsonSerializerSettings)
    {
    }

    public async Task<GetBalancesResponse?> GetBalancesAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetBalancesResponse>("v1/balances", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<GetBalancesResponse?> GetBalancesAsync(string authToken, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetBalancesResponse>(authToken, "v1/balances", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<GetCurrenciesResponse?> GetCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetCurrenciesResponse>("v1/currencies", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<GetTransactionsResponse?> GetTransactionsAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetTransactionsResponse>("v1/payments", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<GetTransactionsResponse?> GetTransactionsAsync(string authToken, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetTransactionsResponse>(authToken, "v1/payments", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<GetTransactionResponse?> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetTransactionResponse>($"v1/payments/{transactionId}", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<GetTransactionResponse?> GetTransactionAsync(string authToken, Guid transactionId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetTransactionResponse>(authToken, $"v1/payments/{transactionId}", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<CreateTransactionInResponse?> CreateTransactionInAsync(CreateTransactionInRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CreateTransactionInResponse>("v1/payments/in", HttpMethod.Post, request, cancellationToken);
    }

    public async Task<CreateTransactionInResponse?> CreateTransactionInAsync(string authToken, CreateTransactionInRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CreateTransactionInResponse>(authToken, "v1/payments/in", HttpMethod.Post, request, cancellationToken);
    }

    public async Task<CreateTransactionOutResponse?> CreateTransactionOutAsync(CreateTransactionOutRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CreateTransactionOutResponse>("v1/payments/out", HttpMethod.Post, request, cancellationToken);
    }

    public async Task<CreateTransactionOutResponse?> CreateTransactionOutAsync(string authToken, CreateTransactionOutRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CreateTransactionOutResponse>(authToken, "v1/payments/out", HttpMethod.Post, request, cancellationToken);
    }

    public async Task<TransactionConfirmResponse?> ConfirmTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<TransactionConfirmResponse>($"v1/payments/{transactionId}/confirm", HttpMethod.Post, null, cancellationToken);
    }

    public async Task<TransactionConfirmResponse?> ConfirmTransactionAsync(string authToken, Guid transactionId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<TransactionConfirmResponse>(authToken, $"v1/payments/{transactionId}/confirm", HttpMethod.Post, null, cancellationToken);
    }
}
