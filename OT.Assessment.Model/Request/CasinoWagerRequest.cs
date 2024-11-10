using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Request
{
    public class CasinoWagerRequest
    {
        public Guid WagerId { get; set; }
        public string Theme { get; set; } = null!;
        public string Provider { get; set; } = null!;
        public Guid ProviderId { get; set; }
        public Guid GameId { get; set; }
        public string GameName { get; set; } = null!;
        public Guid TransactionId { get; set; }
        public Guid BrandId { get; set; }
        public Guid AccountId { get; set; }
        public string Username { get; set; } = null!;
        public Guid? ExternalReferenceId { get; set; }
        public Guid? TransactionTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int NumberOfBets { get; set; }
        public string? CountryCode { get; set; }
        public string? SessionData { get; set; }
        public int? Duration { get; set; }
    }
}
