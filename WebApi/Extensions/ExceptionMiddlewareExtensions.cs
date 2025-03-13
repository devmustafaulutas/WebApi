using System.Net;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;

namespace WebApi.Extentions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app,
            ILoggerService logger)
        {
            // Bu syntax öğrenilicek
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context => 
                {
                    // 500 şu an
                    // context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    // json
                    context.Response.ContentType = "application/json";
                    // IExceptionHandlerFeature var mı yok mu diye bakıyoruz var ise hata vardır demektir
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature is not null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"Something went wrong: {contextFeature}");
                        await context.Response.WriteAsync(new ErrorDetails(){
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}