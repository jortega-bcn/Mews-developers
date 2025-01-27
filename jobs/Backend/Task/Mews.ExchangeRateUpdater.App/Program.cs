using Mews.ExchangeRates.Domain;
using Mews.ExchangeRates.Domain.Configuration;
using Mews.ExchangeRateUpdater.App;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Hosting;
using Serilog;
using Serilog.Settings.Configuration;
using Mews.CzechNationalBankRateReader;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices( 
    (context,services) =>
        {
            services.AddHostedService<Worker>();
            services.AddOptions<ExchangeRateOptions>()
                .Bind(context.Configuration.GetSection(nameof(ExchangeRateOptions)))
                .ValidateDataAnnotations();
            services.AddExchangeRatesDomain();
            services.AddExchangeRateReader();
        });

builder.UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration));
IHost host = builder.Build();

await host.RunAsync();


