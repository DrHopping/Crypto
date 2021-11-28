using System;
using System.Threading.Tasks;
using CasinoRoyale.Enums;
using CasinoRoyale.Services;

namespace CasinoRoyale
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var casino = await Casino.CreateCasino();
            var result = await casino.Play(GameMode.Lcg, 100, 100);
            
            
        }
    }
}