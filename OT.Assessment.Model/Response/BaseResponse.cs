using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Response
{
    public class BaseResponse
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string StatusCode  { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
