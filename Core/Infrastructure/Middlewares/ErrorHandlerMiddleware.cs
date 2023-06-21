using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.Infrastructure.Helper;

namespace Core.Infrastructure.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                Response objResult = new Response();
                objResult.Succeeded = false;

                switch (error)
                {
                    case AppException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        objResult.Message = e.Message;
                        objResult.Errors.Add("Exception", e?.InnerException?.Message ?? "Um ou mais erros foram encontrados");
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        objResult.Message = "Recurso não encontrado";
                        objResult.Errors.Add("Exception", $"{e?.Message} - {e?.InnerException}");
                        break;
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        objResult.Message = "Um ou mais erros foram encontrados";
                        objResult.Errors = e.Errors.ToDictionary(error => error.PropertyName, error => error.ErrorMessage);                       
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        objResult.Message = "Erro inesperado do sistema";
                        objResult.Errors.Add("Exception", $"{error?.Message} - {error?.InnerException}");
                        break;
                }

                var result = JsonSerializer.Serialize(objResult);
                await response.WriteAsync(result);
            }
        }
    }
}
