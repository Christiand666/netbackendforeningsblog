namespace netbackendforeningsblog.Helpers;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    //Handles error handling for duplicated errors 
    //proces the http request if not null pproceed to the next request or function
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            //error handling depended on the error cast default 500
            var response = context.Response;
            response.ContentType = "application/json";

            switch(error)
            {
                case AppException e:
                    // custom application error (400)
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error (404)
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error (500)
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            //cast the error message to json format
            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}