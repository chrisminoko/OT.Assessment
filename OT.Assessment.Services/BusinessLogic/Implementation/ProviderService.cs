using AutoMapper;
using Microsoft.Extensions.Logging;
using OT.Assessment.Core.Enums;
using OT.Assessment.Core.ResponseMessages;
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
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Services.BusinessLogic.Implementation
{
    public class ProviderService : IProviderService
    {
        private readonly GenericRepository<Game> _gameRepository;
        private readonly GenericRepository<Provider> _providerRepository;
        private readonly IMessageProducer _messageProducer;
        private readonly ILogger<ProviderService> _logger;
        private readonly IMapper _mapper;
        public ProviderService(IUnitOfWork unitOfWork, IMessageProducer messageProducer, ILogger<ProviderService> logger, IMapper mapper)
        {
            _providerRepository = new GenericRepository<Provider>(unitOfWork);
            _gameRepository = new GenericRepository<Game>(unitOfWork);
            _messageProducer = messageProducer;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BaseResponse> CreateGameAsync(GameCreateRequest request)
        {
            try
            {
                var gameEntity = _mapper.Map<Game>(request);
                gameEntity.GameId = Guid.NewGuid();
                gameEntity.CreatedDate = DateTime.UtcNow;
                gameEntity.LastModifiedDate = DateTime.UtcNow;

                var result = await _gameRepository.CreateAsync(gameEntity);
                if (result != 1)
                    return new BaseResponse { IsSuccessful = true, Message = Responses.FailedToPublish };

                return new BaseResponse { IsSuccessful = true, Message = Responses.GeneralSuccess };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Responses.ErrorCreatingProvider);
                throw new Exception(Responses.GeneralError, ex);
            }
        }

        public Task<bool> IsValid(Guid providerId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> ProcessProviderCreationAsync(Provider request)
        {
            try
            {
                request.ProviderId = Guid.NewGuid();
                request.CreatedDate = DateTime.UtcNow;
                request.LastModifiedDate = DateTime.UtcNow;

                var result = await _providerRepository.CreateAsync(request);
                if (result != 1)
                    return new BaseResponse { IsSuccessful = true, Message = Responses.FailedToPublish };

                return new BaseResponse { IsSuccessful = true, Message = Responses.GeneralSuccess };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Responses.ErrorCreatingProvider);
                throw new Exception(Responses.GeneralError, ex);
            }
        }

        public async Task<BaseResponse> PublishProviderCreationAsync(ProviderCreateRequest request)
        {
            try
            {

                var result = await _messageProducer.SendMessage(
                    request,
                    EventQueue.CreateProvider);

                if (result.IsSuccess) return new BaseResponse { IsSuccessful = false };

                return new BaseResponse { IsSuccessful = result.IsSuccess , Message = Responses.FailedToPublish};

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Responses.FailedToPublishProvider);
                return  new BaseResponse {IsSuccessful = false};
            }
        }
    }
}
