using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Entities
{
    public class Provider : BaseEntity
    {
        public Guid ProviderId { get; set; }
        public required string ProviderName { get; set; }

        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
