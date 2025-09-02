using Tectum.PublicPaymentProcessClient;

// REST 
var restClient = new PaymentProcessClient(options =>
{
    options.BaseUrl = "https://test.api.payments.softnote.com/";
    options.ClientId = "Long ID";
    options.ClientSecret = "SecretKey";
});

var currencies = await restClient.GetCurrenciesAsync(default);

if (currencies is null || currencies.HasError) 
{
    Console.WriteLine($"Rest API reture error. Message {currencies.ToString()}");
    Console.ReadLine(); 
}

Console.WriteLine("Rest api response all currencies");
foreach (var currency in currencies.Currencies)
{
    Console.WriteLine(currency.Ticker);
}
Console.ReadLine(); 
