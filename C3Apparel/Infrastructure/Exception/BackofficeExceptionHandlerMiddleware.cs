using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace C3Apparel.Infrastructure.Exception;

public class BackofficeExceptionHandlerMiddleware : AbstractExceptionHandlerMiddleware
{
    public BackofficeExceptionHandlerMiddleware(RequestDelegate next) : base(next)
    {
    }

    public override (HttpStatusCode code, string message) GetResponse(System.Exception exception)
    {
        HttpStatusCode code;
        switch (exception)
        {
            case KeyNotFoundException
                or FileNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                break;
            case ArgumentException
                or InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
                break;
        }
        return (code, JsonConvert.SerializeObject(exception.Message));
    }

}