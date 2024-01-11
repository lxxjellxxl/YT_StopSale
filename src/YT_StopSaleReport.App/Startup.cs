using Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App
{
    public class Startup
    {
        public void ConfigureApplicationAsync()
        {
            var environment = Environment.GetEnvironmentVariable("Environment");
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables();

            IConfiguration config = builder.Build();

            IServiceCollection services = new ServiceCollection();

            services.AddLogging(logging =>
            {
                logging.AddDebug();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            });
            services.AddSingleton<Main>();
            services.AddTransient<IFTIProcess, FTIProcess>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<LogMsg>();

            services.Configure<InputConfig>(config.GetSection("InputConfig"));
            services.Configure<SmtpConfig>(config.GetSection("SmtpConfig"));

            var serviceProvider = services.BuildServiceProvider();
            var entry = serviceProvider.GetRequiredService<Main>();
            entry.StartMainProcessAsync();

        }
    }
}