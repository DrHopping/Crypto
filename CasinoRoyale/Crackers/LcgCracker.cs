using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CasinoRoyale.Enums;
using CasinoRoyale.Models;
using CasinoRoyale.RandomGenerators;
using CasinoRoyale.Services;

namespace CasinoRoyale.Crackers
{
    public class LcgCracker : ICracker
    {
        private readonly Lcg _generator;

        public LcgCracker(List<long> numbers)
        {
            if (numbers.Count != 3) throw new ArgumentException("You must pass 3 random numbers");
            var (a, b) = Crack(numbers);
            _generator = new Lcg(a, b, numbers[2]);
        }

        public long Next() => _generator.Next();
        
        private (long a, long b) Crack(List<long> numbers)
        {
            var a = (numbers[1] - numbers[2]) * ModInverse(numbers[1] - numbers[0], Lcg.M) * Math.Sign(numbers[0] - numbers[1]) % Lcg.M;
            var b = (numbers[1] - numbers[0] * a) % Lcg.M;
            return (a, b);
        }

        private long ModInverse(long n, long m)
        {
             if (m == 1) return 0;
             var m0 = m;
             var x = 1L;
             var y = 0L;
             n = Math.Abs(n);
             
             while (n > 1)
             {
                 var q = n / m;
                 
                 (n, m) = (m, n % m);
                 (x, y) = (y, x - q * y);
             }
             
             return x < 0 ? x + m0 : x;
             
        }
    }
}