using Shared.Models.Responses;
using System.Net;

namespace User.Application.Helper.Implementation
{
    public class ExceptionHelper : Exception
    {
        public static ApiException Failed(string errorMessage)
        {
            return new ApiException()
            {
                Title = "Error",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Result = new ServiceResult
                {
                    Message = errorMessage,
                    IsError = true
                }
            };
        }

        public static ApiException Failed(string errorMessage, dynamic dataerror)
        {
            return new ApiException()
            {
                Title = "Error",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Result = new ServiceResult
                {
                    Message = errorMessage,
                    IsError = true,
                    Content = dataerror
                }
            };
        }

        public static ApiException Failed(string errorMessage, List<string> listErrors)
        {
            return new ApiException()
            {
                Title = "Error",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Result = new ServiceResult
                {
                    Message = errorMessage,
                    IsError = true,
                    Content = listErrors
                }
            };
        }

        public static ApiException Failed(ServiceResult serviceResult)
        {
            return new ApiException()
            {
                Title = "Error",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Result = serviceResult
            };
        }
    }
}