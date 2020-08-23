using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SCRAPING_EXCHANGE_RATE_CONSOLE
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetHtmlAsync();
            Console.WriteLine(GetClient("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml"));
            Console.ReadLine();
        }

        private static async void GetHtmlAsync()
        {
            var url = "https://www.exchange-rates.org/converter/SGD/IDR/1";
            var url2 = "https://www.x-rates.com/table/?from=IDR&amount=1";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var ProductsHtml = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("ctl00_M_pnlResult")).ToList();

            var ProductFrom = ProductsHtml[0].Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("col-xs-6 result-cur1")).FirstOrDefault();

            var ProductTo = ProductsHtml[0].Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("col-xs-6 result-cur2")).FirstOrDefault();

            var fromAmt = ProductFrom.Descendants("span").Where(node => node.GetAttributeValue("id", "").Equals("ctl00_M_lblFromAmount")).FirstOrDefault()?.InnerText;
            var fromCode = ProductFrom.Descendants("span").Where(node => node.GetAttributeValue("id", "").Equals("ctl00_M_lblFromIsoCode")).FirstOrDefault()?.InnerText;

            var toAmt = ProductTo.Descendants("span").Where(node => node.GetAttributeValue("id", "").Equals("ctl00_M_lblToAmount")).FirstOrDefault()?.InnerText;
            var toCode = ProductTo.Descendants("span").Where(node => node.GetAttributeValue("id", "").Equals("ctl00_M_lblToIsoCode")).FirstOrDefault()?.InnerText;



            //foreach (var curr in ProductListItems)
            //{
            //    var fromAmt = curr.Descendants("span").Where(node => node.GetAttributeValue("id", "").Equals("ctl00_M_lblFromAmount")).FirstOrDefault();
            //    Console.WriteLine(fromAmt);
            //}

            //var ProductsHtmlList = htmlDocument.DocumentNode.Descendants("div")
            //    .Where(node => node.GetAttributeValue("id", "")
            //    .Equals("ctl00_M_pnlResult")).ToList();


            //var fromAmt = ProductsHtml.Descendants("span")
            //    .Where(node => node.GetAttributeValue("id", "")
            //    .Equals("s-ctl00_M_lblFromAmount")).FirstOrDefault();

            //var fromCode = ProductsHtml.Descendants("span")
            //    .Where(node => node.GetAttributeValue("id", "")
            //    .Equals("s-ctl00_M_lblFromIsoCode")).FirstOrDefault()?.InnerText.Trim('\r', '\n', '\t');

            //var toAmt = ProductsHtml.Descendants("span")
            //    .Where(node => node.GetAttributeValue("id", "")
            //    .Equals("s-ctl00_M_lblToAmount")).FirstOrDefault()?.InnerText.Trim('\r', '\n', '\t');

            //var toCode = ProductsHtml.Descendants("span")
            //    .Where(node => node.GetAttributeValue("id", "")
            //    .Equals("s-ctl00_M_lblToIsoCode")).FirstOrDefault()?.InnerText.Trim('\r', '\n', '\t');


            Console.WriteLine($"From Amount: {fromAmt}");
            Console.WriteLine($"From Code: {fromCode}");
            Console.WriteLine($"To Amount: {toAmt}");
            Console.WriteLine($"To Code: {toCode}");

            Console.WriteLine();

        }

        private static string GetClient(string uri)
        {
            try
            {

                HttpResponseMessage HttpResponseMessage = null;
                var GoResponse = "";
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                    HttpResponseMessage = httpClient.GetAsync(uri).Result;

                    GoResponse = HttpResponseMessage.Content.ReadAsStringAsync().Result;

                }
                return GoResponse;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
    }
}
