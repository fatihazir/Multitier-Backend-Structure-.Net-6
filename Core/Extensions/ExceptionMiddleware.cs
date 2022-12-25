using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Core.Utilities.Results;
using Core.Utilities.Constants;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";
        
            if (e.GetType() == typeof(ValidationException))
            {
                var errors = ((ValidationException)e).Errors.ToList();
                string tempErrorMessage = String.Empty;

                foreach (var error in errors)
                {
                    tempErrorMessage += error;
                    tempErrorMessage += " ";
                }

                message = tempErrorMessage;
                httpContext.Response.StatusCode = 400;

                return httpContext.Response.WriteAsync(new ErrorDetails(message)
                {
                    StatusCode = 400

                }.ToString());

            }
         
            if(e.GetType().Name == "UnauthorizedAccessException")
            {
                httpContext.Response.StatusCode = 401;

                return httpContext.Response.WriteAsync(new ErrorDetails(Messages.AuthorizationDenied)
                {
                    StatusCode = 401

                }.ToString());
            }
            
         
            return httpContext.Response.WriteAsync(new ErrorDetails(message)
            {
                StatusCode = httpContext.Response.StatusCode
               
            }.ToString());


        }
    }
}
