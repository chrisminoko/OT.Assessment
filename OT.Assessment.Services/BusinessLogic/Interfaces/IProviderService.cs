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
    public interface IProviderService
    {
        Task<BaseResponse> PublishProviderCreationAsync (ProviderCreateRequest request);
        Task<BaseResponse> ProcessProviderCreationAsync(Provider request);
        Task<BaseResponse> CreateGameAsync(GameCreateRequest request);
        Task<ProviderDto?> GetProviderWithGamesAsync(Guid providerId);
    }
}
