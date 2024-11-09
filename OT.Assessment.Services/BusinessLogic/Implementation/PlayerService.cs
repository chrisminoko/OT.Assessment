using OT.Assessment.Core.Enums;
using OT.Assessment.Core.Helpers;
using OT.Assessment.Model.Entities;
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

        public async Task<int> CreatePlayer(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            await _messageProducer.SendMessage(player,EventQueue.CreatePlayer);
            return await _repository.CreateAsync(player);
        }

        public async Task<int> DeletePlayer(Guid id)
        {
            var players = await _repository.GetByIdAsync(id);
            if (!await PlayerExists(id))
              
                await _messageProducer.SendMessage(players, EventQueue.CasinoWager);
            throw new NotFoundException($"Player with ID {id} not found");

            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Player> GetPlayerById(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> PlayerExists(Guid id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<int> UpdatePlayer(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            if (!await PlayerExists(player.Id))
                throw new NotFoundException($"Player with ID {player.Id} not found");

            return await _repository.UpdateAsync(player);
        }

       
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
