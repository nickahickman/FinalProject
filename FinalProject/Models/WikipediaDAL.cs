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
        public static string CallAPIToSearch(string subwiki, string query)
        {
            string endpoint = $"https://en.{subwiki}.org/w/api.php?action=query&list=search&srsearch={query}&format=json";

            HttpWebRequest request = WebRequest.CreateHttp(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static WikipediaSearchRoot SearchOnWiki(string subwiki, string query)
        {
            string data = CallAPIToSearch(subwiki, query);
            WikipediaSearchRoot wsr = JsonConvert.DeserializeObject<WikipediaSearchRoot>(data);
            return wsr;
        }

        public static string CallAPIToParse(string subwiki, string title)
        {
            string endpoint = $"https://en.{subwiki}.org/w/api.php?action=parse&format=json&page={title}&prop=wikitext&formatversion=2";

            HttpWebRequest request = WebRequest.CreateHttp(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static WikipediaParseRoot ParseWikitext(string subwiki, string title)
        {
            string data = CallAPIToParse(subwiki, title);
            WikipediaParseRoot war = JsonConvert.DeserializeObject<WikipediaParseRoot>(data);
            return war;
        }
    }
}
