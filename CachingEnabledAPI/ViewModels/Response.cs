using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingEnabledAPI.ViewModels
{
    public class Response 
    {
        public Response()
        {
            Status = Status.Success;
            Message = Status.Success.ToString();
        }
        public Status Status { get; set; }
        public string Message { get; set; }
        public long TotalRecords { get; set; }
        public object Data { get; set; }
        public object Error { get; set; }

       
    }

    public enum Status
    {
        Success = 200,
        Created = 201,
        NoDataAvailable = 204,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        InternalServerError = 500
    }
}
