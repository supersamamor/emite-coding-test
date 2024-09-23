using Emite.CCM.ExcelProcessor.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Emite.CCM.ExcelProcessor
{
    public static class ServiceExtensions
    {
        public static void AddExcelProcessor(this IServiceCollection services)
        {
            services.AddTransient<ExcelService>();
        }
    }
}
