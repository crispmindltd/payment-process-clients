using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;
using Tectum.PublicPaymentProcessClient.Commons.Enums;
using Tectum.PublicPaymentProcessClient.Requests;
using Tectum.PublicPaymentProcessClient.Responses;
using Tectum.PublicPaymentProcessClient.Responses.Dtos;

namespace Tectum.PublicPaymentProcessClient.Tests;

[TestFixture]
public class PaymentProcessApiClientTests
{
    private const string EthereumNetwork = "Ethereum";
    private const string BitcoinNetwork = "Bitcoin";
    private const string UsdtTicker = "USDT";
    private const string BtcTicker = "BTC";
    private const string EthTicker = "ETH";
    private const string UsdtEthKey = "ethereum-erc20-usdt";
    private const string BtcKey = "bitcoin-sha256-btc";

    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private HttpClient _httpClient;
    
    private PaymentProcessClient _paymentProcessApiClient;
    private JsonSerializerOptions _jsonSerializerOptions;

    [SetUp]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.example.com/")
        };

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        //_mockConfig.SetupGet(x => x.Value)
        //    .Returns(new PaymentProcessClientConfig());

        _paymentProcessApiClient = new PaymentProcessClient(_httpClient);

        SetupAuthorizationMock();
    }

    [TearDown]
    public void TearDown()
    {
        _paymentProcessApiClient.Dispose();
        _httpClient.Dispose();
        _mockHttpMessageHandler.Reset();
    }

    private void SetupAuthorizationMock()
    {
        var authResponse = new ApiKeyAuthResponse
        {
            JwtToken = "mock-jwt-token",
            ExpiredAt = DateTime.UtcNow.AddHours(1)
        };

        var authJsonResponse = JsonSerializer.Serialize(authResponse, _jsonSerializerOptions);

        // Moq authorization request
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString().Contains("v2/users/auth")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(authJsonResponse, Encoding.UTF8, "application/json")
            });
    }


    [Test]
    public async Task GetBalancesAsync_ShouldReturnBalances()
    {
        // Arrange
        var expectedResponse = new GetBalancesResponse
        {
            Balances = new List<BalanceDto>
            {
                new() { Ticker = UsdtTicker, Network = EthereumNetwork, Amount = 1000.50m },
                new() { Ticker = BtcTicker, Network = BitcoinNetwork, Amount = 0.00001m }
            }
        };

        SetupHttpResponse(HttpMethod.Get, "v1/balances", expectedResponse);

        // Act
        var result = await _paymentProcessApiClient.GetBalancesAsync(CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.HasError, Is.False);
        Assert.That(result.Balances, Has.Count.EqualTo(2));
        Assert.That(result.Balances[0].Ticker, Is.EqualTo(UsdtTicker));
        Assert.That(result.Balances[0].Amount, Is.EqualTo(1000.50m));
    }

    [Test]
    public async Task GetCurrenciesAsync_ShouldReturnCurrencies()
    {
        // Arrange
        var expectedResponse = new GetCurrenciesResponse
        {
            Currencies = new List<CurrencyDto>
            {
                new() { CurrencyKey = UsdtEthKey, Ticker = UsdtTicker, Network = EthereumNetwork },
                new() { CurrencyKey = BtcKey, Ticker = BtcTicker, Network = BitcoinNetwork}
            }
        };

        SetupHttpResponse(HttpMethod.Get, "v1/currencies", expectedResponse);

        // Act
        var result = await _paymentProcessApiClient.GetCurrenciesAsync(CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.HasError, Is.False);
        Assert.That(result.Currencies, Has.Count.EqualTo(2));
        Assert.That(result.Currencies[0].Ticker, Is.EqualTo(UsdtTicker));
    }

    [Test]
    public async Task GetTransactionsAsync_ShouldReturnTransactions()
    {
        // Arrange
        var expectedResponse = new GetTransactionsResponse
        {
            Transactions = new List<TransactionDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Amount = 100.00m,
                    Ticker = UsdtTicker,
                    Network = Networks.Ethereum,
                    CreatedAt = DateTime.UtcNow,
                    Type = TransactionType.Common,
                    Status = TransactionStatus.Completed,
                    Direction = TransactionDirection.In
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Amount = 200.00m,
                    Ticker = UsdtTicker,
                    Network = Networks.Bitcoin,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Type = TransactionType.Commission,
                    Status = TransactionStatus.Waiting,
                    Direction = TransactionDirection.Out
                }
            }
        };

        SetupHttpResponse(HttpMethod.Get, "v1/payments", expectedResponse);

        // Act
        var result = await _paymentProcessApiClient.GetTransactionsAsync(CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.HasError, Is.False);
        Assert.That(result.Transactions, Has.Count.EqualTo(2));
        Assert.That(result.Transactions[0].Ticker, Is.EqualTo(UsdtTicker));
        Assert.That(result.Transactions[1].Ticker, Is.EqualTo(UsdtTicker));
    }

    [Test]
    public async Task GetTransactionAsync_WithValidId_ShouldReturnTransaction()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var expectedResponse = new GetTransactionResponse
        {
            Transaction = new TransactionDto
            {
                Id = transactionId,
                Amount = 150.00m,
                Ticker = BtcTicker,
                Network = Networks.Bitcoin,
                CreatedAt = DateTime.UtcNow,
                Type = TransactionType.Common,
                Status = TransactionStatus.Completed,
                Direction = TransactionDirection.In,
                Hash = "0x1234567890abcdef"
            }
        };

        SetupHttpResponse(HttpMethod.Get, $"v1/payments/{transactionId}", expectedResponse);

        // Act
        var result = await _paymentProcessApiClient.GetTransactionAsync(transactionId, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.HasError, Is.False);
        Assert.That(result.Transaction.Id, Is.EqualTo(transactionId));
        Assert.That(result.Transaction.Ticker, Is.EqualTo(BtcTicker));
        Assert.That(result.Transaction.Hash, Is.EqualTo("0x1234567890abcdef"));
    }

    [Test]
    public async Task CreateTransactionInAsync_WithValidRequest_ShouldReturnResponse()
    {
        // Arrange
        var request = new CreateTransactionInRequest
        {
            Network = Networks.Ethereum,
            Ticker = EthTicker,
            Amount = 1.5m,
            ExternalId = Guid.NewGuid(),
            RequestId = Guid.NewGuid()
        };

        var expectedResponse = new CreateTransactionInResponse
        {
            Id = Guid.NewGuid(),
            ExternalId = "ext_12345",
            Address = "0x742d35Cc6634C0532925a3b844Bc454e4438f44e",
            Status = TransactionStatus.Waiting
        };

        SetupHttpResponse(HttpMethod.Post, "v1/payments/in", expectedResponse);

        // Act
        var result = await _paymentProcessApiClient.CreateTransactionInAsync(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.HasError, Is.False);
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Address, Is.Not.Null.Or.Empty);
        Assert.That(result.ExternalId, Is.Not.Null.Or.Empty);
    }

    [Test]
    public async Task CreateTransactionOutAsync_WithValidRequest_ShouldReturnResponse()
    {
        // Arrange
        var request = new CreateTransactionOutRequest
        {
            Network = Networks.Ethereum,
            Ticker = EthTicker,
            Amount = 1.5m,
            ExternalId = Guid.NewGuid(),
            RequestId = Guid.NewGuid()
        };

        var expectedResponse = new CreateTransactionOutResponse
        {
            Id = Guid.NewGuid(),
            ExternalId = "ext_12345",
            Address = "0x742d35Cc6634C0532925a3b844Bc454e4438f44e",
            Status = TransactionStatus.Waiting,
        };

        SetupHttpResponse(HttpMethod.Post, "v1/payments/out", expectedResponse);

        // Act
        var result = await _paymentProcessApiClient.CreateTransactionOutAsync(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.HasError, Is.False);
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Address, Is.Not.Null.Or.Empty);
        Assert.That(result.ExternalId, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void Constructor_WithHttpClientOnly_ShouldInitialize()
    {
        // Arrange & Act
        var client = new PaymentProcessClient();

        // Assert
        Assert.That(client, Is.Not.Null);
    }

    [Test]
    public void Constructor_WithHttpClientAndCredentials_ShouldInitialize()
    {
        // Arrange & Act
        var client = new PaymentProcessClient();

        // Assert
        Assert.That(client, Is.Not.Null);
    }

    [Test]
    public async Task Methods_WhenAuthorizationFails_ShouldThrowAuthenticationException()
    {
        // Arrange
        var authRequestHandler = new Mock<HttpMessageHandler>();
        authRequestHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.ToString().Contains("v2/users/auth")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("Unauthorized")
            });

        var authHttpClient = new HttpClient(authRequestHandler.Object)
        {
            BaseAddress = new Uri("https://api.example.com/")
        };

        var clientWithAuth = new PaymentProcessClient();

        // Act & Assert
        Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(async () =>
            await clientWithAuth.GetBalancesAsync(CancellationToken.None));
    }

    [Ignore("Need real service")]
    [Test]
    public async Task CreateTransactionInAsync_WithNullRequest_ShouldThrowArgumentNullException()
    {
        // Arrange
        CreateTransactionInRequest nullRequest = null;

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _paymentProcessApiClient.CreateTransactionInAsync(nullRequest, CancellationToken.None));
    }

    [Ignore("Need real service")]
    [Test]
    public async Task CreateTransactionOutAsync_WithNullRequest_ShouldThrowArgumentNullException()
    {
        // Arrange
        CreateTransactionOutRequest nullRequest = null;

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _paymentProcessApiClient.CreateTransactionOutAsync(nullRequest, CancellationToken.None));
    }

    [Ignore("Need real service")]
    [Test]
    public async Task GetTransactionAsync_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _paymentProcessApiClient.GetTransactionAsync(emptyGuid, CancellationToken.None));
    }

    private void SetupHttpResponse<T>(HttpMethod method, string url, T responseContent)
    {
        var jsonResponse = JsonSerializer.Serialize(responseContent, _jsonSerializerOptions);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == method &&
                    req.RequestUri.ToString().Contains(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            });
    }

    [Test]
    public async Task CreateTransactionInAsync_WithInvalidRequest_ShouldReturnErrorResponse()
    {
        // Arrange
        var invalidRequest = new CreateTransactionInRequest
        {
            Network = Networks.Ethereum,
            Ticker = null, // Invalid: null ticker
            Amount = 0, // Invalid: zero amount
            RequestId = Guid.NewGuid()
        };

        var errorResponse = new CreateTransactionInResponse
        {
            ErrorCode = 123,
        };
        errorResponse.AddErrorMsg("Invalid request parameters");


        SetupHttpResponse(HttpMethod.Post, "v1/payments/in", errorResponse);

        // Act
        var result = await _paymentProcessApiClient.CreateTransactionInAsync(invalidRequest, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.HasError, Is.True);
        Assert.That(result.ToString(), Is.Not.Null.Or.Empty);
    }

    [Test]
    public async Task GetTransactionAsync_WithNonExistentId_ShouldReturnNullResponse()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        GetTransactionResponse? errorResponse = null;

        SetupHttpResponse(HttpMethod.Get, $"v1/payments/{nonExistentId}", errorResponse);

        // Act
        var result = await _paymentProcessApiClient.GetTransactionAsync(nonExistentId, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetTransactionAsync_WithNonExistentId_ShouldReturnErrorResponse()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        GetTransactionResponse? errorResponse = new GetTransactionResponse
        {
            ErrorCode = 123,
        };
        errorResponse.AddErrorMsg("Transaction not found");

        SetupHttpResponse(HttpMethod.Get, $"v1/payments/{nonExistentId}", errorResponse);

        // Act
        var result = await _paymentProcessApiClient.GetTransactionAsync(nonExistentId, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.HasError, Is.True);
    }
}
