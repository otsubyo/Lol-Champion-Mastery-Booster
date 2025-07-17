using System.Text.Json;

namespace Lol_Champion_Mastery_Booster.Services
{
    public class RiotApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _riotApiKey;

        public RiotApiService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _riotApiKey = configuration["RiotApiKey"];
        }

        public async Task<string?> GetPuuidAsync(string gameName, string tagLine, string region)
        {
            var url = $"https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}?api_key={_riotApiKey}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("puuid").GetString();
        }

        public async Task<List<ChampionMasteryDto>> GetMasteryAsync(string puuid, string region)
        {
            var url = $"https://{region}.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-puuid/{puuid}?api_key={_riotApiKey}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new List<ChampionMasteryDto>();

            var json = await response.Content.ReadAsStringAsync();
            var masteries = JsonSerializer.Deserialize<List<ChampionMasteryDto>>(json);
            return masteries ?? new List<ChampionMasteryDto>();
        }
    }

    public class ChampionMasteryDto
    {
        public int championId { get; set; }
        public int championLevel { get; set; }
        public int championPoints { get; set; }
    }
}
