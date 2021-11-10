using System.Collections.Generic;
using System.Linq;

namespace Crypto.Library.Helpers
{
    public class Combinator
    {
        public static IEnumerable<T[]> GetCombinations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new[] { t });
            return GetCombinations(list, length - 1)
                .SelectMany(t => list, 
                    (t1, t2) => t1.Concat(new[] { t2 }).ToArray());
        }
    }
}