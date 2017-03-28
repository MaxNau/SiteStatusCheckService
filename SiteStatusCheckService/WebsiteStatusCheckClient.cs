using System;
using System.Net.Http;

namespace SiteStatusCheckService
{
    public class WebsiteStatusCheckClient
    {
        HttpClient client;

        public WebsiteStatusCheckClient(string uri)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(uri);
        }

        public void GetWebsiteStatus(string apiUri)
        {
            client.GetAsync(apiUri);
        }
    }
}
