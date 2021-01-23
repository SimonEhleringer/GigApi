using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public static string GetLocationHeader(this HttpContext httpContext, IUrlHelper url, Guid id)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}{url.RouteUrl(id)}/{id}";
        }
    }
}
