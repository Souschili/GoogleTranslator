using Microsoft.Extensions.Caching.Memory;

namespace TranslationDemo.Api.Services
{
    public class TranslationCacheService
    {
         private readonly IMemoryCache _cache;

    public TranslationCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T GetOrCreate<T>(string cacheKey, Func<T> createItem, TimeSpan slidingExpiration, TimeSpan absoluteExpiration)
    {
        if (_cache.TryGetValue(cacheKey, out T cachedValue))
        {
            return cachedValue;
        }

        var newValue = createItem();

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(slidingExpiration)
            .SetAbsoluteExpiration(absoluteExpiration);

        _cache.Set(cacheKey, newValue, cacheEntryOptions);

        return newValue;
    }
    }
}
