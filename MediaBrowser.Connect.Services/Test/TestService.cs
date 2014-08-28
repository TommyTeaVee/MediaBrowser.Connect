using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace MediaBrowser.Connect.Services.Test
{
    [Route("/test", "GET")]
    public class TestRequest
    {
        
    }

    public class TestService : Service
    {
        public object Get(TestRequest request)
        {
            return "Ok";
        }
    }
}
