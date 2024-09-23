using Emite.Common.Services.Shared.Interfaces;
using Emite.CCM.EmailSending.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Emite.CCM.EmailSending
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEmailSendingAService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            if (configuration["MailSettings:SendingType"]?.ToUpper() == SendingType.SMTP.ToString().ToUpper())
            {
                services.AddTransient<IMailService, SMTPEmailService>();
            }
            else if (configuration["MailSettings:SendingType"]?.ToUpper() == SendingType.OneMessage.ToString().ToUpper())
            {
                services.AddTransient<IMailService, OneMessageEmailServiceApi>();
                services.AddHttpClient<IMailService, OneMessageEmailServiceApi>(c =>
                {
                    c.BaseAddress = new Uri(configuration.GetValue<string>("MailSettings:EmailApiUrl"));
                });
            }
        }
    }
}
