using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuditelWebServer.Server.HTTP
{
    public class UnathorizedResponse
    {
        public class UnauthorizedResponse : Response
        {
            public UnauthorizedResponse()
                : base(StatusCode.Unauthorized)
            {
            }
        }
    }
}
