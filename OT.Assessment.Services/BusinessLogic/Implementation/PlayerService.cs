using AutoMapper;
using Dapper;
using Microsoft.Extensions.Logging;
using OT.Assessment.Core.Enums;
using OT.Assessment.Core.Helpers;
using OT.Assessment.Core.ResponseMessages;
using OT.Assessment.Model.Dto;
using OT.Assessment.Model.Entities;
using OT.Assessment.Model.Request;
using OT.Assessment.Model.Response;
using OT.Assessment.Repository.Implementation;
using OT.Assessment.Repository.Interface;
using OT.Assessment.Services.BusinessLogic.Interfaces;
using OT.Assessment.Services.Producer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Services.BusinessLogic.Implementation
{
    public class PlayerService : IPlayerService
    {
        private readonly GenericRepository<Player> _repository;
        private readonly GenericRepository<Game> _gameRepository;
        private readonly GenericRepository<Provider> _providerRepository;
        private readonly GenericRepository<CasinoWager> _casinoWagerRepository;
        private readonly IMessageProducer _messageProducer;
        private readonly ILogger<PlayerService> _logger;
        private readonly IMapper _mapper;
        public PlayerService(IUnitOfWork unitOfWork, IMessageProducer messageProducer, ILogger<PlayerService> logger, IMapper mapper)
        {
            _repository = new GenericRepository<Player>(unitOfWork);
            _casinoWagerRepository = new GenericRepository<CasinoWager>(unitOfWork);
            _providerRepository = new GenericRepository<Provider>(unitOfWork);
            _gameRepository = new GenericRepository<Game>(unitOfWork);
            _messageProducer = messageProducer;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BaseResponse> PublishCasinoWagerAsync(CasinoWagerRequest request)
        {
            try
            {

                var result = await _messageProducer.SendMessage( request, EventQueue.CasinoWager);

                if (result.IsSuccess) return new BaseResponse { IsSuccessful = false };

                return new BaseResponse { IsSuccessful = result.IsSuccess, Message = Responses.FailedToPublish };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Responses.FailedToPublishCasinoWager);
                return new BaseResponse { IsSuccessful = false };
            }
        }

        public async Task<BaseResponse> CreatePlayerAsync(PlayerCreateRequest request)
        {
            try
            {
                var playerEntity = _mapper.Map<Player>(request);
                playerEntity.AccountId = Guid.NewGuid();
                playerEntity.CreatedDate = DateTime.UtcNow;
                playerEntity.LastModifiedDate = DateTime.UtcNow;

                var result = await _repository.CreateAsync(playerEntity);
                if (result != 1)
                    return new BaseResponse { IsSuccessful = false, Message = Responses.GeneralError };

                return new BaseResponse { IsSuccessful = true, Message = Responses.GeneralSuccess };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Responses.ErrorCreatingPlayer);
                throw new Exception(Responses.GeneralError, ex);
            }
        }


        public async Task<Player> GetPlayerById(Guid id,string collumn)
        {
            return await _repository.GetByIdAsync(id,collumn);
        }

        public async Task<PaginatedResponse<PlayerWagerDto>> GetPlayerCasinoWagersAsync(Guid playerId, int pageSize, int page)
        {
           
            pageSize = pageSize > 0 ? pageSize : 10;
            page = page > 0 ? page : 1;

            var parameters = new DynamicParameters();
            parameters.Add("@PlayerId", playerId);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@Page", page);
            parameters.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                
                var result = await _repository.RunProcedureWithPaginationAsync<PlayerWagerDto>("sp_GetPlayerCasinoWagers", parameters);

                int totalRecords = parameters.Get<int>("@TotalRecords");
                int totalPages = parameters.Get<int>("@TotalPages");

                var data = result?.Data ?? new List<PlayerWagerDto>();

              
                return new PaginatedResponse<PlayerWagerDto>
                {
                    Data = data,
                    TotalPage = totalPages,
                    Total = totalRecords,
                    Page = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching player casino wagers");

                throw new Exception("Failed to retrieve player casino wagers.", ex);
            }
        }


        public async Task<Result<IEnumerable<TopSpenderDto>>> GetTopSpendersAsync(int count)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Count", count);

                var topSpenders = await _repository.RunProcedureAsync<TopSpenderDto>("sp_GetTopSpenders", parameters);

                return Result<IEnumerable<TopSpenderDto>>.Success(topSpenders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top spenders");
                return Result<IEnumerable<TopSpenderDto>>.Failure("Failed to retrieve top spenders");
            }
        }

        private async Task UpdatePlayerStatsAsync(Guid accountId, decimal amount)
        {

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AccountId", accountId);
                parameters.Add("@Amount", amount);

                await _repository.RunProcedureAsync<int>(
                    "sp_UpdatePlayerStats",
                    parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "something went wrong");
             
            }
        }

        public async Task<BaseResponse> ProcessCasinoWagerCreationAsync(CasinoWager casinoWager)
        {
            try
            {
                //var validations = await ValidateCasinoWagerRequest(casinoWager);
                casinoWager.WagerId= Guid.NewGuid();
                casinoWager.LastModifiedDate = DateTime.UtcNow;
                casinoWager.CreatedDate = DateTime.UtcNow;
                var result = await _casinoWagerRepository.CreateAsync(casinoWager);

                if (result == 1)
                {
                   await UpdatePlayerStatsAsync(casinoWager.AccountId,casinoWager.Amount); // This can EOD process or we can be publishing an event everytime we process a casino wager we can immediately update player stats

                    return new BaseResponse { Id = casinoWager.WagerId, IsSuccessful = true, Message = "Very good", StatusCode = "" };
                }
                return new BaseResponse { Id = casinoWager.WagerId, IsSuccessful = false, Message = "", StatusCode = "" };
            }
            catch (Exception ex)
            {

                return new BaseResponse { Id = casinoWager.WagerId, IsSuccessful = false, Message = ex.Message, StatusCode = "" };
            }
    
        }

       

        public async Task<bool> PlayerExists(Guid id,string column)
        {
            return await _repository.ExistsAsync(id, column);
        }

        private async Task<bool> ValidateCasinoWagerRequest(CasinoWager request)
        {
            var IplayerValide = await _repository.ExistsAsync(request.AccountId,"AccountId");
            if (!IplayerValide) return false;

            return true;
        }
       
    }

   
}
