using OT.Assessment.Model.Dto;
using OT.Assessment.Model.Entities;
using OT.Assessment.Model.Request;
using OT.Assessment.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Services.BusinessLogic.Interfaces
{
    public interface IPlayerService
    {
        Task<BaseResponse> PublishCasinoWagerAsync(CasinoWagerRequest request);
        Task<BaseResponse> ProcessCasinoWagerCreationAsync(CasinoWager casinoWager);

        Task<BaseResponse> CreatePlayerAsync(PlayerCreateRequest player);
        Task<PaginatedResponse<PlayerWagerDto>> GetPlayerCasinoWagersAsync(Guid playerId, int pageSize, int page);
        Task<Result<IEnumerable<TopSpenderDto>>> GetTopSpendersAsync(int count);
        Task<Player> GetPlayerById(Guid id, string column);
       
    }
}
