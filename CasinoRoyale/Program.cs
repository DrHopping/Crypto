using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasinoRoyale.Crackers;
using CasinoRoyale.Enums;
using CasinoRoyale.Models;
using CasinoRoyale.RandomGenerators;
using CasinoRoyale.Services;

namespace CasinoRoyale
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var casino = await Casino.CreateCasino();
            //await WinLcg(casino);
            await WinMt(casino);
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
            
            Result result;
            do
            {
                result = await casino.Play(GameMode.Lcg, 500, (int)cracker.Next());
            } while (result.Account.Money < 1000000);
            Console.WriteLine(result.Message);
        }

        private static async Task WinMt(Casino casino)
        {
            var seed = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var mtCracker = new MersenneTwisterCracker((ulong)seed);
            Result result;
            do
            {
                result = await casino.Play(GameMode.Mt, 500, (int)mtCracker.Next());
            } while (result.Account.Money < 1000000);
            Console.WriteLine(result.Message);
        }
    }
}