using Mews.CzechNationalBankRateReader.Interfaces;
using Mews.ExchangeRates.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mews.CzechNationalBankRateReader
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeRateReader(this IServiceCollection services)
        {
            services.AddSingleton<IResponseBodyParser, ResponseBodyParser>();
            services.AddSingleton<IFirstLineParser, FirstLineParser>();
            services.AddSingleton<IExchangeRateContentParser, ExchangeRateContentParser>();
            services.AddSingleton<IExchangeRateReader, ExchangeRateReader>();
            services.AddSingleton<IExchangeRateSaver, ExchangeRateSaver>();
            services.TryAddSingleton<HttpClient>();

            return services;
        }
    }
}
