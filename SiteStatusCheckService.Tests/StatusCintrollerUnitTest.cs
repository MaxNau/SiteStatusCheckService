using SiteStatusCheckService.Contollers;
using Xunit;

namespace SiteStatusCheckService.Tests
{

    public class StatusCintrollerUnitTest
    {
        [Fact]
        public void CheckServiceStatusStoppedSuccess()
        {
            string[] args = new string[] { };
            using (WebsiteStatusCheckService service = new WebsiteStatusCheckService())
            {
                service.OnDebug(args);
                service.Stop();
                var controller = new StatusController();

                var response = controller.CheckStatus();

                Assert.NotNull(response);
                Assert.False(response);
            }
        }

        [Fact]
        public void CheckServiceStatusRunningSuccess()
        {
            string[] args = new string[] { };
            using (WebsiteStatusCheckService service = new WebsiteStatusCheckService())
            {
                service.OnDebug(args);
                var controller = new StatusController();

                var response = controller.CheckStatus();

                Assert.NotNull(response);
                Assert.True(response);
                service.Stop();
            }
        }

        [Fact]
        public void CheckWebsiteStatusSuccess()
        {
            string[] args = new string[] { };
            using (WebsiteStatusCheckService service = new WebsiteStatusCheckService())
            {
                service.OnDebug(args);
                var controller = new StatusController();
                string uri = "google.com";
                var response = controller.CheckWebsiteStatus(uri);

                Assert.NotNull(response);
                Assert.Contains("google.com: Status Code: OK Description OK", response);
                service.Stop();
            }
        }

        [Fact]
        public void CheckWebsiteStatusFailure()
        {
            string[] args = new string[] { };
            using (WebsiteStatusCheckService service = new WebsiteStatusCheckService())
            {
                service.OnDebug(args);
                var controller = new StatusController();
                string uri = "goo818238gle.com";
                var response = controller.CheckWebsiteStatus(uri);

                Assert.NotNull(response);
                Assert.Contains("No such website or website is down", response);
                service.Stop();
            }
        }
    }
}
