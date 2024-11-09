﻿using Dapper;
using OT.Assessment.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Repository.Implementation
{
    public class QueryRepository : IQueryRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public QueryRepository(IDbConnection connection, IDbTransaction transaction = null)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            return await _connection.QueryAsync<T>(query, parameters, _transaction, commandTimeout, commandType);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(query, parameters, _transaction, commandTimeout, commandType);
        }

        public async Task<T> QuerySingleAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            return await _connection.QuerySingleAsync<T>(query, parameters, _transaction, commandTimeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryStoredProcedure<T>(string procName, object parameters = null, int? commandTimeout = null)
        {
            return await QueryAsync<T>(procName, parameters, CommandType.StoredProcedure, commandTimeout);
        }
    }
}