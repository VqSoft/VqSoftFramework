using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VqSoft.Framework.Common.Cache
{
    public interface ICacheVq<TK,TV>
    {
        TV Get<TV>(TK cacheKey, Func<TV> getUncachedValue, DateTimeOffset datetimeOffset);

        TV Get<TV>(TK cacheKey, Func<TV> getUncachedValue, TimeSpan timeSpan);

        void Remove(TK cacheKey);

    }//ICacheVq
}
