using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Entities
{
    public class PlayerStats : BaseEntity
    {
        public Guid AccountId { get; set; }
        public decimal TotalAmount { get; set; }
        public int WagerCount { get; set; }
        public DateTime? LastWagerDateTime { get; set; }
        public DateTime LastCalculatedDateTime { get; set; }

        public virtual Player Player { get; set; } = null!; // I want to enforce that every player to have a stats
    }
}
