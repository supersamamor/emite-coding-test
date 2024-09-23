using Emite.CCM.ChatGPT.Services;
using Emite.CCM.ChatGPT.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Emite.CCM.ChatGPT
{
    public static class ServiceCollectionExtensions
    {
        public static void AddChatGPTApiService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OpenAI>(configuration.GetSection("OpenAI"));      
            services.AddTransient<ChatGPTService>();
            services.AddHttpClient<ChatGPTService>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetValue<string>("OpenAI:ApiUrl")!);
            }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });
        }
    }
}
