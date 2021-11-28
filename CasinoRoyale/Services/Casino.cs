using System;
using System.Net.Http;
using System.Threading.Tasks;
using CasinoRoyale.Enums;
using CasinoRoyale.Models;
using Newtonsoft.Json;


namespace CasinoRoyale.Services
{
    public class Casino
    {
        private const string Url = "http://95.217.177.249/casino";
        private readonly HttpClient _httpClient = new();
        private readonly int _accountId;
        public Account Account { get; private set; }

            
        private Casino(int accountId)
        {
            _accountId = accountId;
        }

        public static async Task<Casino> CreateCasino()
        {
            var rand = new Random();
            var id = rand.Next();
            var casino = new Casino(id);
            var account = await casino.CreateAccount();
            casino.Account = account;
            return casino;
        }

        public async Task<Result> Play(GameMode gameMode, long bet, long betNumber)
        {
            var url = $"{Url}/play{gameMode}?id={_accountId}&bet={bet}&number={betNumber}";
            var result = await GetDeserializedAsync<Result>(url);
            result.IsVictory = result.RealNumber == betNumber;
            return result;
        }
        
        private async Task<Account> CreateAccount()
        {
            var url = $"{Url}/createacc?id={_accountId}";
            return await GetDeserializedAsync<Account>(url);
        }
        
        private async Task<T> GetDeserializedAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) throw new InvalidOperationException("Request failed");
            var message = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(message);
        }
        
    }
}