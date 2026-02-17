using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BuditelWebServer.Server.Common;
namespace BuditelWebServer.Server.HTTP
{
    public class HtmlResponse
    {
        private string htmlForm;

        public HtmlResponse(string htmlForm)
        {
            this.htmlForm = htmlForm;
        }

        public class ContentResponse : Response
        {
            public ContentResponse(string content, string contentType)
                : base(StatusCode.OK)
            {
                Guard.AgainstNull(content);
                Guard.AgainstNull(contentType);

                this.Headers.Add(Header.ContentType, contentType);

                this.Body = content;
            }
        }
    }
}
