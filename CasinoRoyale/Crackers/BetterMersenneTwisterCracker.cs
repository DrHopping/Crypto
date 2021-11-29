using System;
using System.Collections.Generic;
using System.Linq;
using CasinoRoyale.RandomGenerators;

namespace CasinoRoyale.Crackers
{
    public class BetterMersenneTwisterCracker : ICracker
    {
        private readonly MersenneTwister _generator;
        
        public BetterMersenneTwisterCracker(List<ulong> numbers)
        {
            if (numbers.Count != 624) throw new ArgumentException("You must pass 624 random numbers");
            var untempered = numbers.Select(Untemper).ToArray();
            _generator = new MersenneTwister(untempered);
        }

        public long Next()
        {
            return (long)_generator.genrand_uint32();
        }
        
        private ulong Untemper(ulong tempered)
        {
            tempered ^= tempered >> 18;
            tempered ^= (tempered << 15) & 0xefc60000;
            
            for (var i = 0; i < 7; i++) {
                tempered ^= (tempered << 7) & 0x9d2c5680;
            }
            
            for (var i = 0; i < 3; i++) {
                tempered ^= (tempered >> 11);
            }

            return tempered;
        }
        
        
    }
}