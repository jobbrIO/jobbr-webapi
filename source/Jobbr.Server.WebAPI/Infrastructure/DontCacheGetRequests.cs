using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    public class DontCacheGetRequests : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            // Without this header, IE11 would cache get requests, which leads to problems when GETting data from rest api
            // difference to cache-control: no cache => forward/back button still work with expires -1
            if (actionExecutedContext?.HttpContext?.Response?.Headers == null)
            {
                return;
            }

            actionExecutedContext.HttpContext.Response.Headers.Expires = DateTimeOffset.MinValue.ToString();
        }
    }
}
