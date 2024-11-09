

using System.ComponentModel.DataAnnotations.Schema;

namespace OT.Assessment.Model.Entities
{
    [Table("Players")]
    public class Player : BaseEntity
    {
        public required string Username { get; set; }
       
    }
}
