using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static BuditelWebServer.Server.HTTP.ContentResponses;

namespace BuditelWebServer.Server.HTTP
{
    public class TextResponses
    {
        public class TextResponse : ContentResponse
        {
            public TextResponse(string text)
                : base(text, "text/plain") // Fixed: Using standard MIME type string or your ContentType constant
            {
            }
        }
    }
}
