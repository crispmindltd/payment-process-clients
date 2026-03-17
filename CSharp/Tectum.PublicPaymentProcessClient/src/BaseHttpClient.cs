using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Specialized;
using System.Security.Authentication;
using System.Text;
using System.Web;
using Tectum.PublicPaymentProcessClient.Converters;
using Tectum.PublicPaymentProcessClient.Options;
using Tectum.PublicPaymentProcessClient.Requests;
using Tectum.PublicPaymentProcessClient.Responses;

namespace Tectum.PublicPaymentProcessClient;

public class BaseHttpClient : IDisposable
{
    private readonly bool _disposeHttpClient;
    private readonly HttpClient _httpClient;
    private readonly PaymentProcessClientOptions _options;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private readonly SemaphoreSlim _authSemaphore = new SemaphoreSlim(1, 1);

    private string ClientId { get; set; }
    private string ClientSecretKey { get; set; }
    private string? AuthToken { get; set; }
    private DateTime AuthExpiredAt { get; set; }
    private Task<bool> _authTask;

    /// <summary>
    /// Base HTTP client for payment processing API operations
    /// </summary>
    /// <param name="httpClient">HttpClient instance (optional - if null, a new instance will be created)</param>
    /// <param name="optionsDelegate">Delegate for configuring client options via Action</param>
    protected BaseHttpClient(HttpClient? httpClient, Action<PaymentProcessClientOptions> optionsDelegate)
    {
        _options = ApplyOptionsDelegate(optionsDelegate);
        _httpClient = httpClient ?? CreateDefaultHttpClient();
        _disposeHttpClient = httpClient is null;
        ConfigurePaymentProcessApi(_options);
        _jsonSerializerSettings = CreateJsonSerializerSettings();

        if (httpClient is null)
        {
            ConfigureHttpClient(_httpClient, _options);
        }
    }

    /// <summary>
    /// Base HTTP client for payment processing API operations
    /// </summary>
    /// <param name="httpClient">HttpClient instance (optional - if null, a new instance will be created)</param>
    /// <param name="options">Client configuration options via IOptions pattern</param>
    /// <param name="jsonSerializerSettings">JSON serialization settings (optional)</param>
    protected BaseHttpClient(HttpClient? httpClient, IOptions<PaymentProcessClientOptions> options, JsonSerializerSettings? jsonSerializerSettings = null)
    {
        _options = options.Value;
        _httpClient = httpClient ?? CreateDefaultHttpClient();
        _disposeHttpClient = httpClient is null;
        ConfigurePaymentProcessApi(_options);
        _jsonSerializerSettings = jsonSerializerSettings ?? CreateJsonSerializerSettings();

        if (httpClient is null)
        {
            ConfigureHttpClient(_httpClient, _options);
        }
    }

    /// <summary>
    /// Base HTTP client for payment processing API operations
    /// </summary>
    /// <param name="httpClient">HttpClient instance (optional - if null, a new instance will be created)</param>
    /// <param name="clientId">Client identifier for authentication</param>
    /// <param name="clientSecretKey">Client secret key for authentication</param>
    protected BaseHttpClient(HttpClient? httpClient, string clientId, string clientSecretKey)
    {
        _options = ApplyOptionsDelegate(null);
        _options.ClientId = clientId;
        _options.ClientSecret = clientSecretKey;

        _httpClient = httpClient ?? CreateDefaultHttpClient();
        _disposeHttpClient = httpClient is null;
        ConfigurePaymentProcessApi(_options);
        _jsonSerializerSettings = CreateJsonSerializerSettings();

        if (httpClient is null)
        {
            ConfigureHttpClient(_httpClient, _options);
        }
    }

    private static HttpClient CreateDefaultHttpClient()
    {
        return new HttpClient();
    }

    private static JsonSerializerSettings CreateJsonSerializerSettings()
    {
        return new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Include,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>
            {
                new JsonPropertyEnumConverter()
            }
        };
    }

    private void ConfigureHttpClient(HttpClient client, PaymentProcessClientOptions options)
    {
        if (string.IsNullOrEmpty(options.BaseUrl))
        {
            throw new ArgumentException("BaseUrl is required");
        }

        client.BaseAddress = new Uri(options.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(options.TimeoutInSeconds);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    private void ConfigurePaymentProcessApi(PaymentProcessClientOptions options)
    {
        ClientId = options.ClientId;
        ClientSecretKey = options.ClientSecret;
    }

    private static PaymentProcessClientOptions ApplyOptionsDelegate(Action<PaymentProcessClientOptions>? optionsDelegate)
    {
        var options = PaymentProcessClientOptions.Default.Copy();
        optionsDelegate?.Invoke(options);
        return options;
    }

    protected async Task<T?> GetAsync<T>(string url,
        IEnumerable<KeyValuePair<string, string>>? parameters = default,
        CancellationToken cancellationToken = default)
        where T : BaseApiResponse
    {
        NameValueCollection? queryString = null;
        if (parameters != null)
        {
            queryString = HttpUtility.ParseQueryString(string.Empty);

            foreach (var parameter in parameters)
            {
                queryString.Add(parameter.Key, parameter.Value);
            }
        }

        using var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url + (queryString != null ? "?" + queryString : ""), UriKind.Relative)
        };

        var response = await SendRequestAsync(requestMessage, cancellationToken: cancellationToken);
        return JsonConvert.DeserializeObject<T>(response, _jsonSerializerSettings);
    }

    protected async Task<T?> GetAsync<T>(string authToken, string url,
    IEnumerable<KeyValuePair<string, string>>? parameters = default,
    CancellationToken cancellationToken = default)
    where T : BaseApiResponse
    {
        NameValueCollection? queryString = null;
        if (parameters != null)
        {
            queryString = HttpUtility.ParseQueryString(string.Empty);

            foreach (var parameter in parameters)
            {
                queryString.Add(parameter.Key, parameter.Value);
            }
        }

        using var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url + (queryString != null ? "?" + queryString : ""), UriKind.Relative)
        };

        var response = await SendRequestWithAuthTokenAsync(authToken, requestMessage, cancellationToken: cancellationToken);
        return JsonConvert.DeserializeObject<T>(response, _jsonSerializerSettings);
    }

    protected Task<T?> SendRequestAsync<T>(string url,
        HttpMethod method,
        object? request = default,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return SendRequestAsync<T>(url, method, new Dictionary<string, string>(), request, cancellationToken);
    }

    protected Task<T?> SendRequestAsync<T>(string authToken, string url,
    HttpMethod method,
    object? request = default,
    CancellationToken cancellationToken = default)
    where T : class
    {
        return SendRequestAsync<T>(authToken, url, method, new Dictionary<string, string>(), request, cancellationToken);
    }

    /// <summary>
    /// Send request to outside by httlcient
    /// </summary>
    /// <param name="url">Url</param>
    /// <param name="method">http method</param>
    /// <param name="headers">Headers of http client</param>
    /// <param name="request">Request for sending to </param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="T">BaseApiResponse </typeparam>
    /// <returns></returns>
    protected Task<T?> SendRequestAsync<T>(string url,
        HttpMethod method,
        IReadOnlyDictionary<string, string> headers,
        object? request = default,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return SendRequestAsync<T>(url, method, headers, UriKind.Relative, request, cancellationToken);
    }

    /// <summary>
    /// Send request to outside by httlcient
    /// </summary>
    /// <param name="authToken">authorization token</param>
    /// <param name="url">Url</param>
    /// <param name="method">http method</param>
    /// <param name="headers">Headers of http client</param>
    /// <param name="request">Request for sending to </param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="T">BaseApiResponse </typeparam>
    /// <returns></returns>
    protected Task<T?> SendRequestAsync<T>(string authToken, string url,
        HttpMethod method,
        IReadOnlyDictionary<string, string> headers,
        object? request = default,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return SendRequestAsync<T>(authToken, url, method, headers, UriKind.Relative, request, cancellationToken);
    }

    /// <summary>
    /// Send request to outside by httlcient
    /// </summary>
    /// <param name="url">Url</param>
    /// <param name="method">http method</param>
    /// <param name="headers">Headers of http client</param>
    /// <param name="uriKind">Uri kind</param>
    /// <param name="request">Request for sending to </param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="T">BaseApiResponse </typeparam>
    /// <returns></returns>
    protected async Task<T?> SendRequestAsync<T>(string url,
        HttpMethod method,
        IReadOnlyDictionary<string, string> headers,
        UriKind uriKind,
        object? request = default,
        CancellationToken cancellationToken = default)
        where T : class
    {
        StringContent? content = null;

        if (request != null)
        {
            var jsonRequest = JsonConvert.SerializeObject(request, _jsonSerializerSettings);

            content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        }

        using (var requestMessage = new HttpRequestMessage())
        {
            requestMessage.Content = content;
            requestMessage.Method = method;
            requestMessage.RequestUri = new Uri(url, uriKind);

            foreach (var header in headers)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }

            var response = await SendRequestAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(response, _jsonSerializerSettings);
        }
    }

    /// <summary>
    /// Send request to outside by httlcient
    /// </summary>
    /// <param name="authToken">authorization token</param>
    /// <param name="url">Url</param>
    /// <param name="method">http method</param>
    /// <param name="headers">Headers of http client</param>
    /// <param name="uriKind">Uri kind</param>
    /// <param name="request">Request for sending to </param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="T">BaseApiResponse </typeparam>
    /// <returns></returns>
    protected async Task<T?> SendRequestAsync<T>(string authToken, string url,
        HttpMethod method,
        IReadOnlyDictionary<string, string> headers,
        UriKind uriKind,
        object? request = default,
        CancellationToken cancellationToken = default)
        where T : class
    {
        StringContent? content = null;

        if (request != null)
        {
            var jsonRequest = JsonConvert.SerializeObject(request, _jsonSerializerSettings);

            content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        }

        using (var requestMessage = new HttpRequestMessage())
        {
            requestMessage.Content = content;
            requestMessage.Method = method;
            requestMessage.RequestUri = new Uri(url, uriKind);

            foreach (var header in headers)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }

            var response = await SendRequestWithAuthTokenAsync(authToken, requestMessage, cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(response, _jsonSerializerSettings);
        }
    }

    /// <summary>
    /// Send request to outside services
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken">Token</param>
    private async Task<string> SendRequestAsync(HttpRequestMessage message, CancellationToken cancellationToken)
    {
        // Ensure we have valid authentication
        if (!await EnsureAuthorizationAsync(cancellationToken))
        {
            throw new InvalidOperationException("Authorization failed");
        }

        // Add Authorization header if token is set
        message.Headers.Add("Authorization", AuthToken);
        //message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(null, AuthToken);

        using var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Send request to outside services with Auth Token
    /// </summary>
    /// <param name="authToken"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken">Token</param>
    private async Task<string> SendRequestWithAuthTokenAsync(string authToken,HttpRequestMessage message, CancellationToken cancellationToken)
    {
        // Add Authorization header if token is set
        message.Headers.Add("Authorization", authToken);
        //message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(null, AuthToken);

        using var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Ensures valid authentication token is available
    /// </summary>
    private async Task<bool> EnsureAuthorizationAsync(CancellationToken cancellationToken)
    {
        // Fast path: check if token is already valid without locking
        if (IsTokenValid())
        {
            return true;
        }

        // Acquire lock for auth operations
        await _authSemaphore.WaitAsync(cancellationToken);
        try
        {
            // Double-check after acquiring lock
            if (IsTokenValid())
            {
                return true;
            }

            // Create new auth task if needed
            if (_authTask is null || _authTask.IsCompleted)
            {
                _authTask = PerformAuthorizationAsync(cancellationToken);
            }

            return await _authTask;
        }
        finally
        {
            _authSemaphore.Release();
        }
    }

    /// <summary>
    /// Perform actual authorization request
    /// </summary>
    private async Task<bool> PerformAuthorizationAsync(CancellationToken cancellationToken)
    {
        try
        {
            var request = new AuthorizationRequest(ClientId, ClientSecretKey, Commons.Enums.GrantType.ClientCredentials);

            var authResult = await SendRequestAuthAsync<ApiKeyAuthResponse>(
                "v2/users/auth",
                HttpMethod.Post,
                request,
                cancellationToken
            );

            if (authResult is null || authResult.HasError || string.IsNullOrEmpty(authResult.JwtToken))
            {
                throw new AuthenticationException("Authorization failed: " + (authResult?.ToString() ?? "Unknown error"));
            }

            AuthToken = authResult.JwtToken;
            AuthExpiredAt = authResult.ExpiredAt;

            return true;
        }
        catch (Exception ex)
        {
            // Reset auth task on failure to allow retry
            _authTask = null;
            throw new AuthenticationException("Authorization process failed", ex);
        }
    }

    /// <summary>
    /// Check if current token is valid
    /// </summary>
    private bool IsTokenValid()
    {
        return !string.IsNullOrEmpty(AuthToken) &&
               DateTime.UtcNow <= AuthExpiredAt;
    }

    protected async Task<T?> SendRequestAuthAsync<T>(string url,
        HttpMethod method,
        object? request = default,
        CancellationToken cancellationToken = default)
        where T : class
    {
        StringContent? content = null;

        if (request != null)
        {
            var jsonRequest = JsonConvert.SerializeObject(request, _jsonSerializerSettings);

            content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        }

        using (var requestMessage = new HttpRequestMessage())
        {
            requestMessage.Content = content;
            requestMessage.Method = method;
            requestMessage.RequestUri = new Uri(url, UriKind.Relative);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(result, _jsonSerializerSettings);
        }
    }

    public void Dispose()
    {
        if (_disposeHttpClient)
        {
            _httpClient?.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}
