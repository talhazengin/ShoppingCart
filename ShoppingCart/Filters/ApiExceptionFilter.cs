using System;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using ShoppingCart.Core.Exceptions;

namespace ShoppingCart.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case NotFoundException notFoundException:

                    // handle explicit 'known' API errors
                    context.ExceptionHandled = true;

                    context.Result = new JsonResult(notFoundException.Message);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case BadRequestException badRequestException:

                    // handle explicit 'known' API errors
                    context.ExceptionHandled = true;

                    context.Result = new JsonResult(badRequestException.Message);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException _:
                    context.Result = new JsonResult(context.Exception.Message);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case ForbiddenException _:
                    context.Result = new JsonResult(context.Exception.Message);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
            }

            base.OnException(context);
        }
    }
}