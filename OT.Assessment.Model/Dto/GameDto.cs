using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Dto
{
    public class GameDto
    {
        public Guid GameId { get; set; }
        public string GameName { get; set; } = null!;
        public string? Theme { get; set; }
    }
}
