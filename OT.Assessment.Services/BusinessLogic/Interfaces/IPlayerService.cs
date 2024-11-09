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
        Task<Player> GetPlayerById(Guid id);
        Task<bool> PlayerExists(Guid id);
        Task<IEnumerable<Player>> GetAllPlayers();
        Task<int> CreatePlayer(Player player);
        Task<int> UpdatePlayer(Player player);
        Task<int> DeletePlayer(Guid id);
    }
}
