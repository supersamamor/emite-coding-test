global using NUnit.Framework;
using Emite.CCM.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Emite.CCM.UnitTest
{
    public abstract class TestBase
    {
        protected ServiceProvider ServiceProvider { get; private set; }
        protected IConfiguration Configuration { get; private set; }

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            // Build configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory) // Ensure the base path is correct
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Set up dependency injection
            var services = new ServiceCollection();

            // Register IMemoryCache
            services.AddMemoryCache();

            // Configure CacheSettings
            services.Configure<CacheSettings>(Configuration.GetSection("CacheSettings"));

            // Optionally, register CacheSettings as a singleton for direct access
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<CacheSettings>>().Value);

            // Register other services if needed
            // Example: services.AddTransient<IMyService, MyService>();

            // Build the service provider
            ServiceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

}
