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
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.Configure<CacheSettings>(Configuration.GetSection("CacheSettings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<CacheSettings>>().Value);
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
