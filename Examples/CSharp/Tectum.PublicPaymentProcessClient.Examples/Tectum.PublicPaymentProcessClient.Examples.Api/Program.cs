using Tectum.PublicPaymentProcessClient;
using Tectum.PublicPaymentProcessClient.ExtensionMethods;
using Tectum.PublicPaymentProcessClient.Responses.Dtos;

var builder = WebApplication.CreateBuilder(args);

//must be added because in the registration AddPaymentProcessClient uses factory IHttpClientFactory
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Default Registration with options from config PaymentProcessClientConfig
builder.Services.AddPaymentProcessClient(options =>
{
    builder.Configuration.GetSection("PaymentProcessClientConfig").Bind(options);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Endpoint get all active currencies
app.MapGet("/currencies", async (IPaymentProcessApiClient client) =>
{
    try
    {
        var currencies = await client.GetCurrenciesAsync(default);

        if (currencies is null || currencies.HasError)
        {
            return Results.Problem(
                detail: $"Rest API returned error: {currencies?.ToString()}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        return Results.Ok(new
        {
            Success = true,
            Currencies = currencies.Currencies.Select(c => new CurrencyDto()
            {
                Ticker = c.Ticker,
                CurrencyKey = c.CurrencyKey,
                Network = c.Network
            })
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: $"Error fetching currencies: {ex.Message}",
            statusCode: StatusCodes.Status500InternalServerError
        );
    }
})
.WithName("GetCurrencies")
.WithOpenApi();

app.Run();

