using System;
using System.Collections.Generic;
using System.Linq;

namespace Crypto.Library.Helpers
{
    public class WeightedRandom<T>
    {
        private static System.Random _rnd = new();
        private readonly SortedList<double, T> _objects;
        public WeightedRandom(SortedList<double, T> objects)
        {
            _objects = objects;
        }

        private KeyValuePair<double, T> NextObj(SortedList<double, T> objects)
        {
            var totalWeight = objects.Sum(o => o.Key);
            var rand = _rnd.NextDouble() * totalWeight;
            foreach (var obj in objects)
            {
                if (rand <= obj.Key) return obj;
                rand -= obj.Key;
            }

            throw new Exception("Random is broken");
        }

        public T Next() => NextObj(_objects).Value;
        

        public List<T> NextN(int amount)
        {
            var result = Enumerable.Range(0, amount).Select(_ =>
            {
                var next = NextObj(_objects);
                return next.Value;
            }).ToList();
            return result;
        }
    }
}