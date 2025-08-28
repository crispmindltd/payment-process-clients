using Tectum.PublicPaymentProcessClient;

// REST 
var restClient = new PaymentProcessClient();
var currencies = await restClient.GetCurrenciesAsync(default);

if (currencies is null || currencies.HasError) 
{
    Console.WriteLine($"Rest API reture error. Message {currencies.Message}");
}

Console.WriteLine("Rest api response all currencies");
foreach (var currency in currencies.Currencies)
{
    Console.WriteLine(currency.Ticker);
}
Console.ReadLine(); 
