using System.Net;
using System.Web.Http;

namespace SiteStatusCheckService.Contollers
{
    public class StatusController : ApiController
    {
        // Checks website status
        // Takes uri of website that needs to be checked as a parameter 
        // Returns website status code and description
        // api/status/checkwebsitestatus/{uri}
        [HttpGet]
        public string CheckWebsiteStatus(string uri)
        {
            // Create web request for the specified website uri
            WebRequest request = WebRequest.Create("https://" + uri);

            try
            {
                // Get response to the request and return it status code and description
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                { 
                    return uri + ": Status Code: " + response.StatusCode + " Description " + response.StatusDescription;
                }
            }
            catch (WebException e)
            {
                using (WebResponse resp = e.Response)
                {
                    var response = (HttpWebResponse)resp;
                    if (response != null)
                        return uri + ": Status Code: " + response.StatusCode + " Description " + response.StatusDescription;
                    else
                        return "No such website or website is down";
                }
            }
        }

        // Checks the service status
        // returns true if service is running and false if it's not
        // api/status/checkstatus
        [HttpGet]
        public bool CheckStatus()
        {
            return (WebsiteStatusCheckService.Status == Status.Running) ? true : false;
        }
    }
}
