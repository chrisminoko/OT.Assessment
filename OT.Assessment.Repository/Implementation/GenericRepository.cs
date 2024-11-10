using Dapper;
using OT.Assessment.Core.Helpers;
using OT.Assessment.Model.Response;
using OT.Assessment.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
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

        public async Task<T> GetByIdAsync(object id  ,string Collumn)
        {
            var query = $"SELECT * FROM {_tableName} WITH(NOLOCK) WHERE Id = @Id"; //I dont like this , it too expensive
            return await _unitOfWork.Queries.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
        }

        public async Task<bool> ExistsAsync(object id , string Collumn)
        {
            var query = $"SELECT COUNT(1) FROM {_tableName} WITH(NOLOCK) WHERE {Collumn} = @Id ";
            var count = await _unitOfWork.Commands.ExecuteScalarAsync<int>(query, new { Collumn = id });
        
           
            return count > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var query = $"SELECT * FROM {_tableName} WITH(NOLOCK)";
            return await _unitOfWork.Queries.QueryAsync<T>(query);
        }

        public async Task<int> CreateAsync(T entity)
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
                return 1;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return -1;
                throw;
            }

        }


        public async Task<IEnumerable<T>> RunProcedureAsync(string procName, DynamicParameters parameters )
        {
            return  await _unitOfWork.Queries.QueryAsync<T>( procName, parameters, commandType: CommandType.StoredProcedure);
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
