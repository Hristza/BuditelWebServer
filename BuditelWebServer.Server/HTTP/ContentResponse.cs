using BuditelWebServer.Server.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuditelWebServer.Server.HTTP
{
    public class ContentResponses
    {
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
