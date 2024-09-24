using Emite.Common.Core.Settings;

namespace Emite.CCM.Application
{
    public class CacheSettings : ICacheSettings
    {
        public int DefaultCacheDurationMinutes { get; set; } = 5;
    }
}
