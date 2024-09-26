using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Npgsql;
using System.Net;

namespace Introduction.Common
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NpgsqlException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = "Internal Server Error",
                };
            }
            else if (context.Exception is InvalidOperationException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = context.Exception.Message,
                };
            }
            else if (context.Exception is not null)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = "An unexpected error occurred. Please try again later.",
                };
            }
            context.ExceptionHandled = true;
        }
    }
}
