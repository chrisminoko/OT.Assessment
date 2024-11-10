using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Request
{
    public class GameCreateRequest
    {
        public Guid ProviderId { get; set; }
        public string GameName { get; set; }
        public string Theme { get; set; }
    }
}
