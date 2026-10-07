using System;
using System.Web;

namespace GeneralAuxiliary
{
    public static class GlobalCacheService
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Retrieves an item from the application cache or adds it using a thread-safe cache-aside pattern.
        /// </summary>
        public static T GetOrAdd<T>(string cacheKey, Func<T> fetchData, TimeSpan absoluteExpiration)
        {
            if (HttpRuntime.Cache == null)
            {
                return fetchData();
            }

            // 1. Fast path: Check cache
            object cachedValue = HttpRuntime.Cache.Get(cacheKey);
            if (cachedValue != null)
            {
                return (T)cachedValue;
            }

            // 2. Slow path: Acquire lock and fetch
            lock (_lock)
            {
                // Double-check inside lock
                cachedValue = HttpRuntime.Cache.Get(cacheKey);
                if (cachedValue != null)
                {
                    return (T)cachedValue;
                }

                T data = fetchData();
                if (data != null)
                {
                    HttpRuntime.Cache.Insert(
                        cacheKey,
                        data,
                        null,
                        DateTime.Now.Add(absoluteExpiration),
                        System.Web.Caching.Cache.NoSlidingExpiration
                    );
                }
                return data;
            }
        }

        /// <summary>
        /// Clears an item from the cache.
        /// </summary>
        public static void Clear(string cacheKey)
        {
            if (HttpRuntime.Cache != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }
    }
}
