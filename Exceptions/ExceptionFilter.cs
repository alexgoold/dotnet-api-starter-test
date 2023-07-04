using dotnet_api_test.Exceptions.ExceptionHandlers;
using dotnet_api_test.Exceptions.ExceptionResponses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotnet_api_test.Exceptions
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            ExceptionResponse response;

            switch (exception)
            {
                case HttpExceptionResponse e:
                    response = HttpResponseHandler.Respond(e);
                    break;

                default:
                    Console.WriteLine($"[{exception.GetType().FullName}] {exception.Message}");
                    Console.WriteLine(exception.StackTrace);
                    Console.WriteLine(exception.InnerException?.StackTrace);
                    response = DefaultExceptionResponse();
                    break;
            }

            context.ExceptionHandled = true;
            context.Result = response.CreateObjectResult();
        }

        /**
         * The default fallback exception.
         */
        private static ExceptionResponse DefaultExceptionResponse()
        {
            return new(
                StatusCodes.Status500InternalServerError,
                "Unspecified error",
                ExceptionTypes.DatabaseError
            );
        }
    }
}