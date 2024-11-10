using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Entities
{
    public class Game : BaseEntity
    {
        public Guid GameId { get; set; }
        public Guid ProviderId { get; set; }
        public string GameName { get; set; } = null!;
        public string? Theme { get; set; }

        public virtual Provider Provider { get; set; } = null!;
        public virtual ICollection<CasinoWager> Wagers { get; set; } = new List<CasinoWager>();
    }
}
