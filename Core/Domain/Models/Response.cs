using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Models
{
    public class Response
    {
        public Response()
        {
            Errors = new Dictionary<string, string>();
        }
        public Response(string message, object data = null)
        {
            Succeeded = true;
            Message = message;
            Errors = new Dictionary<string, string>();
            Data = data;
        }

        public Response(object data = null)
        {
            Succeeded = true;
            Message = "";
            Errors = new Dictionary<string, string>();
            Data = data;
        }

        public object Data { get; set; }
        public bool Succeeded { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public string Message { get; set; }
    }
}
