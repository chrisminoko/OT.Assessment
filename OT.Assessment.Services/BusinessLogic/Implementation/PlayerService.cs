using AutoMapper;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;
        public PlayerService(IUnitOfWork unitOfWork, IMessageProducer messageProducer, ILogger<PlayerService> logger, IMapper mapper, IMemoryCache cache)
        {
            _repository = new GenericRepository<Player>(unitOfWork);
            _casinoWagerRepository = new GenericRepository<CasinoWager>(unitOfWork);
            _providerRepository = new GenericRepository<Provider>(unitOfWork);
            _gameRepository = new GenericRepository<Game>(unitOfWork);
            _messageProducer = messageProducer;
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<BaseResponse> PublishCasinoWagerAsync(CasinoWagerRequest request)
        {
            try
            {

                var result = await _messageProducer.SendMessage( request, EventQueue.CasinoWager);

                if (result.IsSuccess) return new BaseResponse { IsSuccessful = false, Message = Responses.GeneralSuccess };

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
                if (result.IsSuccessful)
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
                _logger.LogError(ex, Responses.GeneralError);

                throw new Exception(Responses.GeneralError, ex);
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
                _logger.LogError(ex, Responses.GeneralError);
                return Result<IEnumerable<TopSpenderDto>>.Failure(Responses.GeneralError);
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
               
                var isUserAccountValid = await _repository.ExistsAsync(casinoWager.AccountId, "AccountId");
                var isGameValid = await _gameRepository.ExistsAsync(casinoWager.GameId, "GameId");
                var isDuplicateWager = await _casinoWagerRepository.ExistsAsync(casinoWager.TransactionId, "TransactionId");

                if (isDuplicateWager)
                {
                    _logger.LogError(Responses.Duplicatewager + $"{casinoWager.TransactionId}");
                    return new BaseResponse { IsSuccessful = false, Message = Responses.Duplicatewager };
                }

                if (!isUserAccountValid || !isGameValid)
                {
                    _logger.LogWarning(Responses.InvalidAccountOrGameID);
                    return new BaseResponse { IsSuccessful = false, Message = Responses.InvalidAccountOrGameID };
                }

                if (!_cache.TryGetValue($"GameDetail_{casinoWager.GameId}", out Game gameDetail))
                {
                
                    gameDetail = await _gameRepository.GetByIdAsync(casinoWager.GameId, "GameId");

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                    };

                    _cache.Set($"GameDetail_{casinoWager.GameId}", gameDetail, cacheOptions);
                }

                casinoWager.GameName = gameDetail.GameName;

               
                casinoWager.WagerId = Guid.NewGuid();
                casinoWager.LastModifiedDate = DateTime.UtcNow;
                casinoWager.CreatedDate = DateTime.UtcNow;

               
                var result = await _casinoWagerRepository.CreateAsync(casinoWager);

                if (result.IsSuccessful)
                {
                    await UpdatePlayerStatsAsync(casinoWager.AccountId, casinoWager.Amount);
                    return new BaseResponse  { Id = casinoWager.WagerId, IsSuccessful = true,Message = Responses.GeneralSuccess, StatusCode = "201" };
                }
                else
                {
                    _logger.LogError(result.Message, "something went wrong");
                    return new BaseResponse{Id = casinoWager.WagerId,IsSuccessful = false,Message = Responses.GeneralError,  StatusCode = "500"  };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Responses.GeneralError);
                return new BaseResponse   { IsSuccessful = false, Message = Responses.GeneralError,  StatusCode = "500" };
            }
        }

       
    }

   
}
