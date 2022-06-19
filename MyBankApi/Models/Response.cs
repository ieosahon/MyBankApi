using System;

namespace MyBankApi.Models
{
    public class Response
    {
        public string RequestId => $"{Guid.NewGuid()}";
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Data { get; set; }
    }
}
