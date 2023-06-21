using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteWebcrawler.Models
{
    public class CreateExecutionResponse
    {
        public Response Data { get; set; }
    }

    public class Response
    {
        public int ExecutionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumbers { get; set; }
        public int? LineNumbers { get; set; }
        public string? JsonFile { get; set; }
    }
}
