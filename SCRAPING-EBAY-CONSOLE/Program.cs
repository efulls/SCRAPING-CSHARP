using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SCRAPING_EBAY_CONSOLE
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();
            Console.ReadLine();
        }

        private static async void GetHtmlAsync()
        {
            var url = "https://www.ebay.com/sch/i.html?_from=R40&_trksid=p2322090.m570.l1313&_nkw=xbox+one&_sacat=0";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var ProductsHtml = htmlDocument.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("srp-results srp-list clearfix")).ToList();

            var ProductListItems = ProductsHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("data-view", "")
                .Contains("mi:1686|iid:")).ToList();

            Console.WriteLine(ProductListItems.Count);

            foreach (var ProductListItem in ProductListItems)
            {
                //id
                Console.WriteLine(ProductListItem.GetAttributeValue("data-view", ""));

                //ProductName
                var productTitle = ProductListItem.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("s-item__title")).FirstOrDefault()?.InnerText.Trim('\r','\n','\t');
                var productTitleHash = ProductListItem.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("s-item__title s-item__title--has-tags")).FirstOrDefault()?.InnerText.Trim('\r', '\n', '\t');
                Console.WriteLine(productTitle?? productTitleHash);

                //Price
                Console.WriteLine(ProductListItem.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("s-item__price")).FirstOrDefault()?.InnerText);

                //Price Using regex
                Console.WriteLine(
                    Regex.Match(
                    ProductListItem.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("s-item__price")).FirstOrDefault()?.InnerText
                    ,@"\d+.\d+")
                    );


                //url
                var productUrl = ProductListItem.Descendants("a")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("s-item__link")).FirstOrDefault()?.GetAttributeValue("href","");
                Console.WriteLine(productUrl);


                Console.WriteLine();
            }

        }
    }
}
