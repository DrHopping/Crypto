using CasinoRoyale.RandomGenerators;

namespace CasinoRoyale.Crackers
{
    public class MersenneTwisterCracker : ICracker
    {
        private readonly ulong _seed;
        private readonly MersenneTwister _generator;

        public MersenneTwisterCracker(ulong seed)
        {
            _seed = seed;
            _generator = new MersenneTwister();
            _generator.init_genrand(seed);
        }

        public long Next()
        {
            return (long)_generator.genrand_uint32();
        }
    }
}