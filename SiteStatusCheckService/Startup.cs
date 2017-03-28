using Owin;
using System.Web.Http;
using SiteStatusCheckService.Middleware;

namespace SiteStatusCheckService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<WebsiteStatusCheckerMiddleware>();
            
            app.UseWebApi(ConfigureWebApi());
        }

        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{action}/{uri}",
                new { uri = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();
            return config;
        }   
    }
}
