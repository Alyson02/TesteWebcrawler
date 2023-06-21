using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteWebcrawler.Models
{
    public class UpdateExecutionModel
    {
        public int ExecutionId { get; set; }
        public int LineNumbers { get; set; }
        public string JsonFile { get; set; }
    }
}
