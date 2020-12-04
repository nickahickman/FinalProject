using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class WikipediaDAL
    {
        public static string CallAPIToSearch(string query)
        {
            string endpoint = $"https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch={query}&format=json";

            HttpWebRequest request = WebRequest.CreateHttp(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static WikipediaSearchRoot SearchOnWikipedia(string query)
        {
            string data = CallAPIToSearch(query);
            WikipediaSearchRoot wsr = JsonConvert.DeserializeObject<WikipediaSearchRoot>(data);
            return wsr;
        }

        public static string CallAPIToExtract(string title)
        {
            string endpoint = $"https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles={title}";

            HttpWebRequest request = WebRequest.CreateHttp(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static WikipediaExtractRoot ExtractWikiPageSummary(string title)
        {
            string data = CallAPIToExtract(title);
            WikipediaExtractRoot wer = JsonConvert.DeserializeObject<WikipediaExtractRoot>(data);
            return wer;
        }
    }
}
