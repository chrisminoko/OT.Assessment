using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Request
{
    public class PlayerCreateRequest
    {
        public required string Username { get; set; }
        public string? CountryCode { get; set; }
    }
}
