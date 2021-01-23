using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api
{
    public static class Utils
    {
        public static Guid GetUserId(this HttpContext httpContext)
        {
            return Guid.Parse(httpContext.User.Claims.Single(x => x.Type == "id").Value);
        }
    }
}
