using OT.Assessment.Model.Entities;
using OT.Assessment.Repository.Interface;
using OT.Assessment.Services.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Services.BusinessLogic.Implementation
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlayerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreatePlayerAsync(Player user)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await _unitOfWork.Commands.ExecuteAsync(
                    "INSERT INTO Players (Id, UserName, CreatedDate,LastModifiedDate) VALUES (@Id, @UserName,@CreatedDate,@LastModifiedDate)",
                    user
                );

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<Player> GetUserAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.Queries.QueryFirstOrDefaultAsync<Player>(
                              "SELECT * FROM Players WHERE Id = @Id",
                              new { Id = id }
                          );
            }
            catch (Exception ex )
            {

                throw;
            }
        }
    }
}
