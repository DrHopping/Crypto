using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasinoRoyale.Crackers;
using CasinoRoyale.Enums;
using CasinoRoyale.RandomGenerators;
using CasinoRoyale.Services;

namespace CasinoRoyale
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var casino = await Casino.CreateCasino();
            await WinLcg(casino);
        }

        private static async Task WinLcg(Casino casino)
        {
            var randomNumbers = new List<int>
            {
                (await casino.Play(GameMode.Lcg, 1, 123)).RealNumber,
                (await casino.Play(GameMode.Lcg, 1, 123)).RealNumber,
                (await casino.Play(GameMode.Lcg, 1, 123)).RealNumber
            };

            var cracker = new LcgCracker(randomNumbers);
            Console.WriteLine((await casino.Play(GameMode.Lcg, 500, (int) cracker.Next())).Message);
        }
    }
}