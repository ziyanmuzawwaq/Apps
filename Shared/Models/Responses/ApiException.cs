namespace Shared.Models.Responses
{
    public class ApiException : Exception
    {
        public ApiException()
        {
        }

        public string Title { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        public ServiceResult Result { get; set; } = new ServiceResult();
    }
}