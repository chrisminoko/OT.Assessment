using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Entities
{
    [TableAttribute("PlayerStats")]
    public class Provider : BaseEntity
    {
        public Guid ProviderId { get; set; }
        public required string ProviderName { get; set; }

        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
