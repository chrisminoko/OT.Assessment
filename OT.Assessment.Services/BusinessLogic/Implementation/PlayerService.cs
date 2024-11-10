using AutoMapper;
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

                var result = await _messageProducer.SendMessage(
                    request,
                    EventQueue.CasinoWager);

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


        public async Task<Player> GetPlayerById(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public Task<PaginatedResponse<PlayerWagerDto>> GetPlayerCasinoWagersAsync(Guid playerId, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TopSpenderDto>> GetTopSpendersAsync(int count)
        {
            throw new NotImplementedException();
        }


        public async Task<BaseResponse> ProcessCasinoWagerCreationAsync(CasinoWager casinoWager)
        {
            try
            {
                //var validations = await ValidateCasinoWagerRequest(casinoWager);
                casinoWager.LastModifiedDate = DateTime.UtcNow;
                casinoWager.CreatedDate = DateTime.UtcNow;
                var result = await _casinoWagerRepository.CreateAsync(casinoWager);

                if (result == 1)
                {
                    return new BaseResponse { Id = casinoWager.WagerId, IsSuccessful = true, Message = "Very good", StatusCode = "" };
                }
                return new BaseResponse { Id = casinoWager.WagerId, IsSuccessful = false, Message = "", StatusCode = "" };
            }
            catch (Exception ex)
            {

                return new BaseResponse { Id = casinoWager.WagerId, IsSuccessful = false, Message = ex.Message, StatusCode = "" };
            }
    
        }

       

        public async Task<bool> PlayerExists(Guid id)
        {
            return await _repository.ExistsAsync(id);
        }

        private async Task<bool> ValidateCasinoWagerRequest(CasinoWager request)
        {
            var IplayerValide = await _repository.ExistsAsync(request.AccountId);
            if (!IplayerValide) return false;

            return true;
        }
       
    }

   
}
