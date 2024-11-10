using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Dto
{
    public class TopSpenderDto
    {
        public Guid AccountId { get; set; }
        public decimal TotalAmountSpend { get; set; }
    }
}
