using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Newtonsoft.Json;
using Tectum.PublicPaymentProcessClient.Options;

namespace Tectum.PublicPaymentProcessClient.ExtensionMethods;

/// <summary>
/// Extensions for DI
/// </summary>
public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Registers PaymentProcessClient as Singleton with configuration via delegate
    /// Uses IHttpClientFactory for optimal HTTP client management
    /// </summary>
    public static IServiceCollection AddPaymentProcessClient(
        this IServiceCollection services,
        Action<PaymentProcessClientOptions> configureOptions)
    {
        if (configureOptions is null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        // Register Options as Singleton
        services.Configure(configureOptions);

        // Register client as Singleton with HttpClient factory
        services.AddSingleton<IPaymentProcessApiClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<PaymentProcessClientOptions>>();
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

            var httpClient = httpClientFactory.CreateClient();
            var optionsValue = options.Value;
            ConfigureHttpClient(httpClient, optionsValue);

            return new PaymentProcessClient(httpClient, options);
        });

        return services;
    }

    /// <summary>
    /// Registers PaymentProcessClient as Singleton with configuration from IConfiguration
    /// </summary>
    public static IServiceCollection AddPaymentProcessClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "PaymentProcessClient")
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        // Register Options from configuration section
        services.Configure<PaymentProcessClientOptions>(configuration.GetSection(sectionName));

        // Register client as Singleton
        services.AddSingleton<IPaymentProcessApiClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<PaymentProcessClientOptions>>();
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

            var httpClient = httpClientFactory.CreateClient();
            var optionsValue = options.Value;
            ConfigureHttpClient(httpClient, optionsValue);

            return new PaymentProcessClient(httpClient, options);
        });

        return services;
    }

    /// <summary>
    /// Registers PaymentProcessClient as Singleton with custom JsonSerializerOptions
    /// </summary>
    public static IServiceCollection AddPaymentProcessClient(
        this IServiceCollection services,
        Action<PaymentProcessClientOptions> configureOptions,
        JsonSerializerSettings jsonSerializerSettings)
    {
        if (configureOptions is null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }
        // Register Options
        services.Configure(configureOptions);

        // Register custom JsonSerializerOptions as singleton
        services.TryAddSingleton(jsonSerializerSettings);

        // Register client as Singleton with custom JSON options
        services.AddSingleton<IPaymentProcessApiClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<PaymentProcessClientOptions>>();
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var jsonSettings = provider.GetService<JsonSerializerSettings>();

            var httpClient = httpClientFactory.CreateClient();
            var optionsValue = options.Value;
            ConfigureHttpClient(httpClient, optionsValue);

            return new PaymentProcessClient(httpClient, options, jsonSettings);
        });

        return services;
    }

    /// <summary>
    /// Registers PaymentProcessClient as Singleton with named HttpClient
    /// </summary>
    public static IServiceCollection AddPaymentProcessClient(
        this IServiceCollection services,
        Action<PaymentProcessClientOptions> configureOptions,
        string httpClientName)
    {
        if (configureOptions is null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        if (string.IsNullOrEmpty(httpClientName))
        {
            throw new ArgumentException("HttpClient name cannot be null or empty", nameof(httpClientName));
        }

        // Register Options
        services.Configure(configureOptions);

        // Configure named HttpClient
        services.AddHttpClient(httpClientName, (provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<PaymentProcessClientOptions>>().Value;
            ConfigureHttpClient(client, options);
        });

        // Register client as Singleton using named HttpClient
        services.AddSingleton<IPaymentProcessApiClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<PaymentProcessClientOptions>>();
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

            var httpClient = httpClientFactory.CreateClient(httpClientName);
            return new PaymentProcessClient(httpClient, options);
        });

        return services;
    }

    /// <summary>
    /// Registers PaymentProcessClient as Singleton with custom HttpClient configuration
    /// </summary>
    public static IServiceCollection AddPaymentProcessClient(
        this IServiceCollection services,
        Action<PaymentProcessClientOptions> configureOptions,
        Action<HttpClient> configureHttpClient)
    {
        if (configureOptions is null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        if (configureHttpClient is null)
        {
            throw new ArgumentNullException(nameof(configureHttpClient));
        }

        // Register Options
        services.Configure(configureOptions);

        // Register client as Singleton with custom HttpClient configuration
        services.AddSingleton<IPaymentProcessApiClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<PaymentProcessClientOptions>>();

            var httpClient = new HttpClient();
            var optionsValue = options.Value;
            ConfigureHttpClient(httpClient, optionsValue);
            configureHttpClient(httpClient);

            return new PaymentProcessClient(httpClient, options);
        });

        return services;
    }

    /// <summary>
    /// Configures HttpClient with PaymentProcessClientOptions
    /// </summary>
    /// <param name="client">HttpClient to configure</param>
    /// <param name="options">Configuration options</param>
    private static void ConfigureHttpClient(HttpClient client, PaymentProcessClientOptions options)
    {
        if (string.IsNullOrEmpty(options.BaseUrl))
        {
            throw new ArgumentException("BaseUrl is required");
        }

        client.BaseAddress = new Uri(options.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(options.TimeoutInSeconds);
    }
}
