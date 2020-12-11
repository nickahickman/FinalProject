using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public static string CallAPIToGetCategories(string subwiki, string title)
        {
            string endpoint = $"https://en.{subwiki}.org/w/api.php?action=query&format=json&prop=categories&titles={title}";

            HttpWebRequest request = WebRequest.CreateHttp(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static CategoriesRoot GetCategories(string subwiki, string title)
        {
            string data = CallAPIToGetCategories(subwiki, title);
            CategoriesRoot cr = JsonConvert.DeserializeObject<CategoriesRoot>(data);

            JToken t = JToken.Parse(data);
            string conId = t["continue"]["clcontinue"].ToString();
            string[] con = conId.Split('|');
            string id = con[0];
            string pageString = t["query"]["pages"][id].ToString();
            CatPage c = JsonConvert.DeserializeObject<CatPage>(pageString);
            cr.query.pages.page = c;

            return cr;
        }

        public static string CallAPIToGetCategoryMembers(string subwiki, string category)
        {
            string endpoint = $"https://en.{subwiki}.org/w/api.php?action=query&format=json&list=categorymembers&cmtitle={category}&cmlimit=5";

            HttpWebRequest request = WebRequest.CreateHttp(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static CategoryMembersRoot GetCategoryMembers(string subwiki, string title)
        {
            string data = CallAPIToGetCategoryMembers(subwiki, title);
            CategoryMembersRoot cmr = JsonConvert.DeserializeObject<CategoryMembersRoot>(data);
            return cmr;
        }
    }
}
