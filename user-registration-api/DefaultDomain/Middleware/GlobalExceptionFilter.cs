using EscuelaPolitecnicaNacional.DgipCommonsLib.Exceptions;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Models.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using InvalidDataException = EscuelaPolitecnicaNacional.DgipCommonsLib.Exceptions.InvalidDataException;

namespace user_registration_api.DefaultDomain.Middleware;

public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger = logger;

    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case InvalidDataException exception:
            {
                var response =
                    ControllerResult<object>.GetErrorResultFromInvalidDataException(
                        exception);
            
                context.Result = new JsonResult(response)
                {
                    StatusCode = 400
                };
                return;
            }
            case NotFoundException exception:
            {
                var response =
                    ControllerResult<object>.GetErrorResultFromNotFoundException(
                        exception);
            
                context.Result = new JsonResult(response)
                {
                    StatusCode = 404
                };
                return;
            }
            case RepositoryException exception:
            {
                _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);
                
                var response =
                    ControllerResult<object>.GetErrorResultFromRepositoryException(
                        exception);
            
                context.Result = new JsonResult(response)
                {
                    StatusCode = 500
                };
                return;
            }
            default:
            {
                var ex = context.Exception;
                _logger.LogError(ex, "An exception occurred: {Message}", ex.Message);
                
                var response =
                    ControllerResult<object>.GetErrorResult("CODE", "MESSAGE", new KeyValuePair<string, string>("An error occurred while processing your request.", context.Exception.Message), ex.ToString());

                context.Result = new JsonResult(response)
                {
                    StatusCode = 500
                };
                break;
            }
        }
    }
}