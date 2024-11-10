

using System.ComponentModel.DataAnnotations.Schema;

namespace OT.Assessment.Model.Entities
{
    [TableAttribute("Players")]
    public class Player : BaseEntity
    {
        public Guid AccountId { get; set; }
        public required string Username { get; set; }
        public string? CountryCode { get; set; }

        public virtual ICollection<CasinoWager> Wagers { get; set; } = new List<CasinoWager>();
        public virtual PlayerStats? PlayerStats { get; set; }

    }
}
