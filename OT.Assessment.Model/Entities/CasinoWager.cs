using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Entities
{
    [TableAttribute("CasinoWagers")]
    public class CasinoWager : BaseEntity
    {
        public Guid WagerId { get; set; }
        public Guid AccountId { get; set; }
        public Guid GameId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid BrandId { get; set; }
        public Guid? ExternalReferenceId { get; set; }
        public Guid? TransactionTypeId { get; set; }
        public string GameName { get; set; }
        public decimal Amount { get; set; }
        public int NumberOfBets { get; set; }
        public string? SessionData { get; set; }
        public long Duration { get; set; }

        public virtual Player Player { get; set; } = null!;
        public virtual Game Game { get; set; } = null!;
    }
}
