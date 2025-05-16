using Shared.Models.Responses;
using System.Net;

namespace User.Application.Helper.Implementation
{
    public class ResponseHelper
    {
        public static ApiResponse Success()
        {
            return new ApiResponse()
            {
                Title = "Success",
                StatusCode = (int)HttpStatusCode.OK,
                Result = new ServiceResult
                {
                    IsError = false
                }
            };
        }

        public static ApiResponse Success(dynamic data)
        {
            return new ApiResponse()
            {
                Title = "Success",
                StatusCode = (int)HttpStatusCode.OK,
                Result = new ServiceResult
                {
                    IsError = false,
                    Content = data
                }
            };
        }

        public static ApiResponse Success(string message, dynamic data)
        {
            return new ApiResponse()
            {
                Title = "Success",
                StatusCode = (int)HttpStatusCode.OK,
                Result = new ServiceResult
                {
                    IsError = false,
                    Content = data,
                    Message = message
                }
            };
        }

        public static ApiResponse SuccessWithError(string message)
        {
            return new ApiResponse()
            {
                Title = "Success",
                StatusCode = (int)HttpStatusCode.OK,
                Result = new ServiceResult
                {
                    IsError = true,
                    Message = message
                }
            };
        }

        public static ApiResponse Failed(string errorMessage)
        {
            return new ApiResponse()
            {
                Title = "Error",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Result = new ServiceResult
                {
                    Message = errorMessage,
                    IsError = true,
                }
            };
        }

        public static ApiResponse Failed(string errorMessage, dynamic dataerror)
        {
            return new ApiResponse()
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

        public static ApiResponse Failed(string errorMessage, List<string> listErrors)
        {
            return new ApiResponse()
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

        public static ApiResponse Failed(ServiceResult serviceResult)
        {
            return new ApiResponse()
            {
                Title = "Error",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Result = serviceResult
            };
        }
    }
}