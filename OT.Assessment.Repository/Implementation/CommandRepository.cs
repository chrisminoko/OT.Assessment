using Dapper;
using OT.Assessment.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Repository.Implementation
{
    public class CommandRepository : ICommandRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public CommandRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }



        public async Task<int> ExecuteAsync(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            return await _connection.ExecuteAsync(query, parameters, _transaction, commandTimeout, commandType);
        }

        public async Task<T> ExecuteScalarAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            return await _connection.ExecuteScalarAsync<T>(query, parameters, _transaction, commandTimeout, commandType);
        }

        public async Task<int> ExecuteStoredProcedure(string procName, object parameters = null, int? commandTimeout = null)
        {
            return await ExecuteAsync(procName, parameters, CommandType.StoredProcedure, commandTimeout);
        }

        public async Task<T> ExecuteStoredProcedureScalar<T>(string procName, object parameters = null, int? commandTimeout = null)
        {
            return await ExecuteScalarAsync<T>(procName, parameters, CommandType.StoredProcedure, commandTimeout);
        }
    }
}
