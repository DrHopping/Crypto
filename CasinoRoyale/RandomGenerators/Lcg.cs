namespace CasinoRoyale.RandomGenerators
{
    public class Lcg
    { 
        public const long M = 4294967296;
        private readonly long _a;
        private readonly long _b;
        private long _last;
        
        public Lcg(long a, long b, long seed)
        {
            _a = a;
            _b = b;
            _last = seed;
        }

        public long Next()
        {
            _last = (_a * _last + _b) % M;
            return _last;
        }
    }
}