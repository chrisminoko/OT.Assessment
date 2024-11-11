using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace OT.Assessment.Repository.Interface
{
    public interface IQueryRepository
    {
        Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null);
        Task<GridReader> QueryMultipleAsync(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null);
        Task<T> QuerySingleAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null);
        Task<IEnumerable<T>> QueryStoredProcedure<T>(string procName, object parameters = null, int? commandTimeout = null);
    }
}
