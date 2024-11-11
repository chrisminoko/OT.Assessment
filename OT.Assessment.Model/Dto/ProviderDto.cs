using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Dto
{
    public class ProviderDto
    {
        public Guid ProviderId { get; set; }
        public required string ProviderName { get; set; }
        public List<GameDto> Games { get; set; } = new List<GameDto>();
    }
}
