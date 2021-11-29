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
            await WinLcg(casino);
            await WinMt(casino);
            await WinBetterMt(casino);
        }

        private static async Task WinLcg(Casino casino)
        {
            var randomNumbers = new List<long>
            {
                (await casino.Play(GameMode.Lcg, 1, 123)).RealNumber,
                (await casino.Play(GameMode.Lcg, 1, 123)).RealNumber,
                (await casino.Play(GameMode.Lcg, 1, 123)).RealNumber
            };
            var cracker = new LcgCracker(randomNumbers);
            await WinCasino(cracker, casino, GameMode.Lcg);
        }

        private static async Task WinMt(Casino casino)
        {
            var seed = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var cracker = new MersenneTwisterCracker((ulong)seed);
            await WinCasino(cracker, casino, GameMode.Mt);
        }

        private static async Task WinBetterMt(Casino casino)
        {
            var numbers = new List<ulong>();
            for (var i = 0; i < 624; i++) numbers.Add((ulong)(await casino.Play(GameMode.BetterMt, 1, 123)).RealNumber);
            var cracker = new BetterMersenneTwisterCracker(numbers);
            await WinCasino(cracker, casino, GameMode.BetterMt);
        }

        private static async Task WinCasino(ICracker cracker, Casino casino, GameMode gameMode)
        {
            Result result;
            do
            {
                result = await casino.Play(gameMode, 100, cracker.Next());
            } while (result.Account.Money < 1000000);
            Console.WriteLine(result.Message);
        }
    }
}