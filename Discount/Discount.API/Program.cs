
using Common.Logging;
using Discount.Infrastructure.Extensions;
using Serilog;

namespace Discount.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();*/

            var host = CreateHostBuilder(args).Build();

            host.MigrateDatabase<Program>();
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseStartup<Startup>();
                   }).UseSerilog(Logging.configureLogger);
    }
}
