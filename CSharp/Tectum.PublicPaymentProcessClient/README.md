# Welcome to Tectum Payment Process Client API  #

## Description ##

Tectum Payment Process client this is special nuget packages to interact with the service Payment Process


## Endpoints ##

Tectum Payment Process Client:

### Transactions: ###

-   **[GET /v1/payments](docs/Payments/GET_payments.md)**: Get all transactions for user
-   **[GET /v1/payments/{transactionId}](docs/Payments/GET_payments_id.md)**: Get one user's transaction
-   **[POST /v1/payments/in](docs/Payments/POST_payments_in.md)**: Create input transaction
-   **[POST /v1/payments/out](docs/Payments/POST_payments_out.md)**: Create outgoing transaction
-   **[POST /v1/payments/{transactionId}/confirm](docs/Payments/POST_payments_id_confirm.md)**: Confirm one transaction and start waiting


### Balances: ###

-   **[GET /v1/balances](docs/Balances/GET_balances.md)**: Get all balances for user

### Currencies: ###

-   **[GET /v1/currencies](docs/Currencies/GET_currencies.md)**: Get all active currencies


## How to use
*REST Endpoints*  

```csharp
// Get the all active currencies rest request
var restClient = new PaymentProcessClient();
var currencies = await restClient.GetCurrenciesAsync(default);
foreach (var currency in currencies.Currencies)
{
    Console.WriteLine(currency.Ticker);
}
```

## How to registrate and use in project 
*Registration DI*

```csharp
builder.Services.AddPaymentProcessClient(options => options);
```

*How to use in class*

```csharp
app.MapGet("/currencies", async (IPaymentProcessApiClient client) =>
{
	await client.GetCurrenciesAsync(default);
});
```