using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class BingNewsDAL
    {
        private readonly System.Net.Http.HttpClient _client;

        public BingNewsDAL(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("x-rapidapi-key", Secret.bingNewsAPIKey);
        }

        public async Task<BingNewsRoot> SearchNewsStoriesAsync(string query)
        {
            var response = await _client.GetAsync($"search?q={query}");
            string jsonData = await response.Content.ReadAsStringAsync();

            BingNewsRoot ro = JsonSerializer.Deserialize<BingNewsRoot>(jsonData);

            return ro;
        }

        public string BuildSearchQuery(string Query)
        {
            string[] words = Query.Split(" ");

            if (words.Length > 1)
            {
                return string.Join("+", words);
            }
            else
            {
                return Query;
            }
        }
    }
}
