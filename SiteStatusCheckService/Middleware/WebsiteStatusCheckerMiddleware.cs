using Microsoft.Owin;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SiteStatusCheckService.Middleware
{
    public class WebsiteStatusCheckerMiddleware : OwinMiddleware
    {
        public WebsiteStatusCheckerMiddleware(OwinMiddleware next) : base(next) { }

        /// <summary>
        /// Gets response body and writes it's content ot the service log
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            var responseBody = "";

            using (var responseBodyReader = new ResponseBodyReader(context))
            {
                await Next.Invoke(context);
                responseBody = await responseBodyReader.GetResponseBody();
                Console.WriteLine(responseBody);
            }

            try
            {
                System.Xml.Linq.XElement.Parse(responseBody);
                var xmlResponseBodyValue = System.Xml.Linq.XElement.Parse(responseBody).Value;
                ServiceLog.WriteLog(xmlResponseBodyValue);
            }
            catch (System.Xml.XmlException ex)
            {
                ServiceLog.WriteLog(responseBody);
            }
        }

        private class ResponseBodyReader : IDisposable
        {

            private readonly Stream stream;
            private readonly MemoryStream buffer;

            /// <summary>
            /// Reads the body into strean
            /// </summary>
            /// <param name="context"></param>
            public ResponseBodyReader(IOwinContext context)
            {
                stream = context.Response.Body;
                buffer = new MemoryStream();
                context.Response.Body = buffer;
            }

            /// <summary>
            /// Gets response body from the memory stream
            /// </summary>
            /// <returns></returns>
            public async Task<string> GetResponseBody()
            {
                buffer.Seek(0, SeekOrigin.Begin);
                var smReader = new StreamReader(buffer);
                return await smReader.ReadToEndAsync();
            }

            public async void Dispose()
            {
                await GetResponseBody();

                buffer.Seek(0, SeekOrigin.Begin);
                await buffer.CopyToAsync(stream);
            }
        }
    }
}
