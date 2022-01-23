using System.Net;

namespace TestTheRest.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public HttpStatusCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Route { get; set; }

        public Response (bool success, HttpStatusCode errorCode, string errorMessage, string route)
        {
            Success = success;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Route = route;
        }
    }
}
