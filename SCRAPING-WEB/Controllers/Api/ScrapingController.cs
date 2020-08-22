using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCRAPING_WEB.Controllers.Api
{
    public class ScrapingController : ApiController
    {

        private const string Selector = ".table > tbody:nth-child(1) > tr:nth-child(2) > td:nth-child(2)";
        private const string BaseURL = "https://kamuslengkap.com/kamus/sinonim/arti-kata/";



        private static WebClient CreateClient()
        {
            var client = new WebClientEx();
            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml,application/json");
            client.Headers.Add("Accept-Encoding", "gzip, deflate");
            client.Headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0");

            return client;
        }

        private static string GetSynonyms(string kata)
        {
            // Memanggil method CreateClient untuk membuat object WebClientEx baru.
            var client = CreateClient();

            // Mendapatkan response berupa HTML string
            var response = client.DownloadString(BaseURL + kata);

            // Parse response menggunakan HtmlParser (AngleSharp)
            var parser = new AngleSharp.Parser.Html.HtmlParser();
            var parsed = parser.Parse(response);

            // Select element menggunakan selector dan ambil text content
            var sinonim = parsed.QuerySelector(Selector)?.TextContent;

            return sinonim ?? "Maaf, sinonim tidak ditemukan";
        }
    }


    public class WebClientEx : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            if (!(base.GetWebRequest(address) is HttpWebRequest request))
                return base.GetWebRequest(address);

            request.AutomaticDecompression =
                DecompressionMethods.Deflate | DecompressionMethods.GZip;

            return request;
        }
    }
}


