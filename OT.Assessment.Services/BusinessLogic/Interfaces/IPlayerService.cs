using OT.Assessment.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Services.BusinessLogic.Interfaces
{
    public interface IPlayerService
    {
        Task CreatePlayerAsync(Player user);
        Task<Player> GetUserAsync(Guid id);
    }
}
