using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace VqSoft.Framework.Common.Cache
{
    public class MemoryCacheVq<TK, TV> : ICacheVq<TK, TV>
    {
        private ObjectCache memoryCache;

        public static MemoryCacheVq<TK, TV> Instance
        {
            get
            {
                return SingletonProvider<MemoryCacheVq<TK, TV>>.Instance;
            }
        }

        public MemoryCacheVq() : this(null) { }
        public MemoryCacheVq(string name)
        {
            memoryCache = new MemoryCache(string.Format("{0}-{1}-{2}", typeof(TK).Name, typeof(TV).Name, name));
        }

        public TV Get<TV>(TK cacheKey, Func<TV> getUncachedValue, DateTimeOffset dateTimeOffset)
        {
            if (memoryCache.Contains(ParseKey(cacheKey)))
            {
                return (TV)memoryCache[ParseKey(cacheKey)];
            }
            else
            {
                var v = getUncachedValue();
                object o = v;
                memoryCache.Set(ParseKey(cacheKey), o, dateTimeOffset);
                return v;
            }
        }

        public TV Get<TV>(TK cacheKey, Func<TV> getUncachedValue, TimeSpan timeSpan)
        {
            return Get(cacheKey, getUncachedValue, new DateTimeOffset(DateTime.UtcNow + timeSpan));
        }

        public void Remove(TK cacheKey)
        {
            memoryCache.Remove(ParseKey(cacheKey));
        }

        private string ParseKey(TK key)
        {
            return key.GetHashCode().ToString();
        }

    }//class
}
