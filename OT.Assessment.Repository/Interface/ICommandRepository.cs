using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Repository.Interface
{
    public interface ICommandRepository
    {
        Task<int> ExecuteAsync(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null);
        Task<T> ExecuteScalarAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null);
        Task<int> ExecuteStoredProcedure(string procName, object parameters = null, int? commandTimeout = null);
        Task<T> ExecuteStoredProcedureScalar<T>(string procName, object parameters = null, int? commandTimeout = null);
    }
}
