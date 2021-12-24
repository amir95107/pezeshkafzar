using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplication100.Areas.Admin.Api
{
    public class PriceController : ApiController
    {
        // GET: api/Price

        public Task<string> Get(string name)
        {
            var price = StartCrawlerAsync(name);
            return price;
        }

        private async Task<string> StartCrawlerAsync(string name)
        {
            var url = "https://torob.com/search/?category=&query="+name;

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var divs = htmlDocument.DocumentNode.Descendants("a")
                .Where(node => node.GetAttributeValue("class", "")
                    .Equals("jsx-2021336621"));

            var firstElement = divs.FirstOrDefault();
            string price = "";
            if (!firstElement.FirstChild.FirstChild.LastChild.FirstChild.InnerText.Contains("تومان"))
            {
                firstElement.FirstChild.FirstChild.LastChild.FirstChild.Remove();
            }
            price = firstElement.FirstChild.FirstChild.LastChild.FirstChild.InnerText.Replace("از", "").Replace("تومان", "").Trim();

            return price;
        }

        // GET: api/Price/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Price
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Price/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Price/5
        public void Delete(int id)
        {
        }
    }
}
