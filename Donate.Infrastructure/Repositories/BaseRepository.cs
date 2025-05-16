using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Donate.Infrastructure.Repositories
{
    public class BaseRepository
    {
        protected readonly IMemoryCache MemoryCache;

        public BaseRepository(IConfiguration configuration, IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }
    }
}