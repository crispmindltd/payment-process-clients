using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tectum.PublicPaymentProcessClient.Options;
using Tectum.PublicPaymentProcessClient.Requests;
using Tectum.PublicPaymentProcessClient.Responses;

namespace Tectum.PublicPaymentProcessClient;

public class PaymentProcessClient : BaseHttpClient, IPaymentProcessApiClient
{
    public PaymentProcessClient(Action<PaymentProcessClientOptions>? optionsDelegate = null)
        : base(null, optionsDelegate)
    {
    }

    public PaymentProcessClient(HttpClient httpClient, Action<PaymentProcessClientOptions>? optionsDelegate = null)
        : base(httpClient, optionsDelegate)
    {
    }

    public PaymentProcessClient(IOptions<PaymentProcessClientOptions> options, JsonSerializerSettings? jsonSerializerSettings = null)
        : base(null, options, jsonSerializerSettings)
    {
    }

    public PaymentProcessClient(HttpClient httpClient, IOptions<PaymentProcessClientOptions> options, JsonSerializerSettings? jsonSerializerSettings = null)
        : base(httpClient, options, jsonSerializerSettings)
    {
    }

    public async Task<GetBalancesResponse?> GetBalancesAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetBalancesResponse>("v1/balances", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<GetCurrenciesResponse?> GetCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetCurrenciesResponse>("v1/currencies", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<GetTransactionsResponse?> GetTransactionsAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetTransactionsResponse>("v1/payments", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<GetTransactionResponse?> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<GetTransactionResponse>($"v1/payments/{transactionId}", HttpMethod.Get, null, cancellationToken);
    }

    public async Task<CreateTransactionInResponse?> CreateTransactionInAsync(CreateTransactionInRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CreateTransactionInResponse>("v1/payments/in", HttpMethod.Post, request, cancellationToken);
    }

    public async Task<CreateTransactionOutResponse?> CreateTransactionOutAsync(CreateTransactionOutRequest request, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<CreateTransactionOutResponse>("v1/payments/out", HttpMethod.Post, request, cancellationToken);
    }

    public async Task<TransactionConfirmResponse?> ConfirmTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<TransactionConfirmResponse>($"v1/payments/{transactionId}/confirm", HttpMethod.Post, null, cancellationToken);
    }
}
