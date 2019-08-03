using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeRunner
{
    internal static class Utils
    {
        public static async IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource,Task<TResult>> selector)
        {
            foreach (var v in sources)
                yield return await selector(v);
        }
    }
}
