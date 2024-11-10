using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Request
{
    public class PaginationRequest
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
