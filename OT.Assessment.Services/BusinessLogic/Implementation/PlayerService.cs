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
        private readonly IMessageProducer _messageProducer;
        public PlayerService(IUnitOfWork unitOfWork, IMessageProducer messageProducer)
        {
            _repository = new GenericRepository<Player>(unitOfWork);
            _messageProducer = messageProducer;
        }

        public async Task<BaseResponse> CreateCasinoWagerAsync(CasinoWagerRequest request)
        {
            try
            {
                var results = await _messageProducer.SendMessage(request, EventQueue.CasinoWager);
                return new BaseResponse { Id = request.WagerId, IsSuccessful = true };
            }
            catch (Exception)
            {
                return new BaseResponse { Id= Guid.Empty,IsSuccessful= false };
               
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

        public async Task<bool> PlayerExists(Guid id)
        {
            return await _repository.ExistsAsync(id);
        }

       
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
