﻿using OT.Assessment.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _tableName = typeof(T).Name; // Assumes table name matches class name
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var query = $"SELECT * FROM {_tableName}s WHERE Id = @Id";
            return await _unitOfWork.Queries.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
        }

        public async Task<bool> ExistsAsync(object id)
        {
            var query = $"SELECT COUNT(1) FROM {_tableName} WHERE Id = @Id";
            var count = await _unitOfWork.Commands.ExecuteScalarAsync<int>(query, new { Id = id });
            return count > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var query = $"SELECT * FROM {_tableName}";
            return await _unitOfWork.Queries.QueryAsync<T>(query);
        }

        public async Task<int> CreateAsync(T entity)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id")
                .ToList();

            var columns = string.Join(", ", properties.Select(p => p.Name));
            var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var query = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters})";
            return await _unitOfWork.Commands.ExecuteAsync(query, entity);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id")
                .ToList();

            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
            var query = $"UPDATE {_tableName} SET {setClause} WHERE Id = @Id";

            return await _unitOfWork.Commands.ExecuteAsync(query, entity);
        }

        public async Task<int> DeleteAsync(object id)
        {
            var query = $"DELETE FROM {_tableName} WHERE Id = @Id";
            return await _unitOfWork.Commands.ExecuteAsync(query, new { Id = id });
        }

        public async Task<IEnumerable<T>> RunProcedureAsync(string procName, object parameters = null)
        {
            return await _unitOfWork.Queries.QueryAsync<T>($"EXEC {procName}", parameters);
        }

        public async Task<T> RunProcedureSingleAsync(string procName, object parameters = null)
        {
            return await _unitOfWork.Queries.QueryFirstOrDefaultAsync<T>($"EXEC {procName}", parameters);
        }

        public async Task<int> RunProcedureNonQueryAsync(string procName, object parameters = null)
        {
            return await _unitOfWork.Commands.ExecuteAsync($"EXEC {procName}", parameters);
        }
    }
}