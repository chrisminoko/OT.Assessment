using OT.Assessment.Core.Enums;
using OT.Assessment.Core.Helpers;
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
        public PlayerService(IUnitOfWork unitOfWork, IMessageProducer messageProducer)
        {
            _repository = new GenericRepository<Player>(unitOfWork);
            _casinoWagerRepository = new GenericRepository<CasinoWager>(unitOfWork);
            _providerRepository = new GenericRepository<Provider>(unitOfWork);
            _gameRepository = new GenericRepository<Game>(unitOfWork);
            _messageProducer = messageProducer;
        }

        public async Task<BaseResponse> CreateCasinoWagerAsync(CasinoWagerRequest request)
        {
            try
            {
                request.CreatedDateTime = DateTime.Now;
                var results = await _messageProducer.SendMessage(request, EventQueue.CasinoWager);
                return new BaseResponse { Id = request.WagerId, IsSuccessful = true };
            }
            catch (Exception)
            {
                return new BaseResponse { Id= Guid.Empty,IsSuccessful= false };
               
            }
        }

        public async Task<BaseResponse> CreateGameAsync(GameCreateRequest request)
        {
            try
            {
               
                var results = await _messageProducer.SendMessage(request, EventQueue.CreateGame);
                return new BaseResponse { IsSuccessful = true };
            }
            catch (Exception)
            {
                return new BaseResponse { Id = Guid.Empty, IsSuccessful = false };

            }
        }

        public async Task<BaseResponse> CreatePlayerAsync(PlayerCreateRequest request)
        {
            try
            {

                var results = await _messageProducer.SendMessage(request, EventQueue.CreatePlayer);
                return new BaseResponse { IsSuccessful = true };
            }
            catch (Exception)
            {
                return new BaseResponse { Id = Guid.Empty, IsSuccessful = false };

            }
        }

        public async Task<BaseResponse> CreateProviderAsync(ProviderCreateRequest request)
        {
            try
            {

                var results = await _messageProducer.SendMessage(request, EventQueue.CreateProvider);
                return new BaseResponse { IsSuccessful = true };
            }
            catch (Exception)
            {
                return new BaseResponse { Id = Guid.Empty, IsSuccessful = false };

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

        public async Task<BaseResponse> InserProvider(Provider provider)
        {  try
            {

                provider.LastModifiedDate = DateTime.UtcNow;
                provider.CreatedDate = DateTime.UtcNow;
                provider.ProviderId = Guid.NewGuid();
                var result = await _providerRepository.CreateAsync(provider);

                if (result == 1)
                {
                    return new BaseResponse { Id = provider.ProviderId, IsSuccessful = true, Message = "", StatusCode = "" };
                }
                return new BaseResponse { IsSuccessful = false, Message = "", StatusCode = "" };
            }
            catch (Exception ex)
            {

                return new BaseResponse { IsSuccessful = false, Message = ex.Message, StatusCode = "" };
            }
        }

        public async Task<BaseResponse> InsertCasinoWagerAsync(CasinoWager casinoWager)
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

        public async Task<BaseResponse> InsertGame(Game game)
        {
            try
            {
                
                game.LastModifiedDate = DateTime.UtcNow;
                game.CreatedDate = DateTime.UtcNow;
                game.GameId = Guid.NewGuid();
                var result = await _gameRepository.CreateAsync(game);

                if (result == 1)
                {
                    return new BaseResponse { Id = game.GameId, IsSuccessful = true, Message = "", StatusCode = "" };
                }
                return new BaseResponse { IsSuccessful = false, Message = "", StatusCode = "" };
            }
            catch (Exception ex)
            {

                return new BaseResponse {  IsSuccessful = false, Message = ex.Message, StatusCode = "" };
            }
        }

        public async Task<BaseResponse> InsertPlayer(Player player)
        {
            try
            {

                player.LastModifiedDate = DateTime.UtcNow;
                player.CreatedDate = DateTime.UtcNow;
                player.AccountId = Guid.NewGuid();
                var result = await _repository.CreateAsync(player);

                if (result == 1)
                {
                    return new BaseResponse { Id = player.AccountId, IsSuccessful = true, Message = "hey", StatusCode = "" };
                }
                return new BaseResponse { IsSuccessful = false, Message = "", StatusCode = "" };
            }
            catch (Exception ex)
            {

                return new BaseResponse { IsSuccessful = false, Message = ex.Message, StatusCode = "" };
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

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
