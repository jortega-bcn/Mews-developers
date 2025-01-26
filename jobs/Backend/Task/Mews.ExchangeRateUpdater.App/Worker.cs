using Mews.ExchangeRates.Domain;
using Mews.ExchangeRates.Domain.Configuration;
using Mews.ExchangeRates.Domain.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mews.ExchangeRateUpdater.App
{
    public class Worker(IExchangeRateProvider provider,
        IHostApplicationLifetime hostApplicationLifetime,
        IOptions<ExchangeRateOptions> options,
        ILogger<Worker> logger) : IHostedService
    {
        private readonly IExchangeRateProvider _provider = provider;
        private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;
        private readonly ILogger<Worker> _logger = logger;
        private readonly ExchangeRateOptions _options = options.Value;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(nameof(StartAsync));
            var configuredCurrencies = _options.Currencies.Select(c => new Currency(c));
            await DisplayExchangeRates(configuredCurrencies);
            _hostApplicationLifetime.StopApplication();
        }

        private async Task DisplayExchangeRates(IEnumerable<Currency> currencies)
        {
            var currencyRates = await _provider.GetExchangeRatesAsync(currencies);
            foreach (var rate in currencyRates)
            {
                Console.WriteLine(rate.ToString());
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug(nameof(StopAsync));
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
