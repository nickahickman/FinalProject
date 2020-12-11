using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class TrendingTopicsDAL
    {
        private readonly System.Net.Http.HttpClient _client;

        public TrendingTopicsDAL(System.Net.Http.HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("x-rapidapi-key", Secret.rapidAPIKey);
        }
        
        public async Task<List<string>> GetTrendingTopicsAsync()
        {
            var response = await _client.GetAsync("hotTopicsofToday");
            string jsonData = await response.Content.ReadAsStringAsync();


            TrendingRoot tr = JsonSerializer.Deserialize<TrendingRoot>(jsonData);
            List<string> topics = tr.hottopics.ToList();

            return RemoveSportsResults(topics);
        }

        public List<string> RemoveSportsResults(List<string> results)
        {
            List<string> filtered = new List<string>();

            foreach (String s in results)
            {
                if (!s.Contains("vs"))
                {
                    filtered.Add(s);
                }
            }

            return filtered;
        }
    }
}
