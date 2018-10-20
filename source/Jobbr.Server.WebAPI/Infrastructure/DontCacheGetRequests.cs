using System;
using System.Web.Http.Filters;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    public class DontCacheGetRequests : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // Without this header, IE11 would cache get requests, which leads to problems when GETting data from rest api
            // difference to cache-control: no cache => forward/back button still work with expires -1
            if (actionExecutedContext.Response?.Content?.Headers == null)
            {
                return;
            }

            actionExecutedContext.Response.Content.Headers.Expires = DateTimeOffset.MinValue;
        }
    }
}
