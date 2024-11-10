using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Request
{
    public class ProviderCreateRequest
    {
        [Required]
        public string ProviderName { get; set; }
    }
}
