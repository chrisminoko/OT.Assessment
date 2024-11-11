using Dapper;
using OT.Assessment.Core.Helpers;
using OT.Assessment.Core.ResponseMessages;
using OT.Assessment.Model.Dto;
using OT.Assessment.Model.Response;
using OT.Assessment.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Repository.Implementation
{
    public class GenericRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _tableName;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tableName = GetTableName(); 
        }

        public async Task<T> GetByIdAsync(object id  ,string column)
        {
            var query = $"SELECT * FROM {_tableName} WITH(NOLOCK) WHERE {column} = @Id"; //I dont like this , it too expensive
            return await _unitOfWork.Queries.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
        }

        public async Task<bool> ExistsAsync(object id, string column)
        {
            try
            {
                var query = $"SELECT COUNT(1) FROM {_tableName} WITH(NOLOCK) WHERE {column} = @Id";
                var count = await _unitOfWork.Commands.ExecuteScalarAsync<int>(query, new { Id = id });

                return count > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse> CreateAsync(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(NotMappedAttribute))
                && !p.GetMethod!.IsVirtual
                && p.GetValue(entity) != null)
    .           ToList();

                var columns = string.Join(", ", properties.Select(p => p.Name));
                var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));

                var query = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters})";
                await _unitOfWork.Commands.ExecuteAsync(query, entity);
                _unitOfWork.Commit();
                return new BaseResponse { IsSuccessful = true ,Message=Responses.GeneralSuccess};
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new BaseResponse { IsSuccessful = false, Message = ex.Message };
             
            }

        }

        public async Task<ProviderDto?> GetProviderWithGamesAsync(Guid providerId)
        {
           

            using var multi = await _unitOfWork.Queries.QueryMultipleAsync(
                "sp_GetProviderWithGames",
                new { ProviderId = providerId },
                commandType: CommandType.StoredProcedure);

            var provider = await multi.ReadFirstOrDefaultAsync<ProviderDto>();

            if (provider == null)
                return null;

            var games = await multi.ReadAsync<GameDto>();
            provider.Games = games.ToList();

            return provider;
        }

        public async Task<IEnumerable<T>> RunProcedureAsync<T>(string procName, DynamicParameters parameters )
        {
            return await _unitOfWork.Queries.QueryAsync<T>(procName, parameters,commandType:CommandType.StoredProcedure);
        }

        public async Task<PaginatedResponse<T>> RunProcedureWithPaginationAsync<T>( string procName, DynamicParameters parameters)
        {
            try
            {
                var data = await _unitOfWork.Queries.QueryAsync<T>(
                    procName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return new PaginatedResponse<T>
                {
                    Data = data,
                    Total = parameters.Get<int>("@TotalRecords"),
                    TotalPage = parameters.Get<int>("@TotalPages")
                };
            }
            catch (Exception)
            {
                throw; 
            }
        }

        private string GetTableName()
        {
            var type = typeof(T);
            var tableAttribute = type.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();

            // If the attribute exists, use its Name property, otherwise fall back to the class name
            return tableAttribute?.Name ?? type.Name;
        }
    }
}
