using Mews.ExchangeRates.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.ExchangeRates.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeRatesDomain( this IServiceCollection services)
        {
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddSingleton<IFaultTolerantExchangeRateReader, FaultTolerantExchangeRateReader>();
            return services;
        }
    }
}
