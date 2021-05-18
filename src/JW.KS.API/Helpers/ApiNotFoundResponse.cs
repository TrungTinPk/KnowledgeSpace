using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JW.KS.API.Helpers
{
    public class ApiNotFoundResponse : ApiResponse
    {
        public ApiNotFoundResponse(string message)
           : base(404, message)
        {
        }
    }
}
