using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class WikipediaSearchDAL
    {
        public static string CallAPI(string query)
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
            string data = CallAPI(query);
            WikipediaSearchRoot wsr = JsonConvert.DeserializeObject<WikipediaSearchRoot>(data);
            return wsr;
        }
    }
}
