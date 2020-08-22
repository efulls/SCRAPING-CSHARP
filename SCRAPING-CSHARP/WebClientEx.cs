using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCRAPING_CSHARP
{
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
