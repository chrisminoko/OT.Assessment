using OT.Assessment.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ICommandRepository _commandRepository;
        private IQueryRepository _queryRepository;
        private bool _disposed;

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }
        public ICommandRepository Commands =>
                    _commandRepository ??= new CommandRepository(_connection, _transaction);

        public IQueryRepository Queries =>
            _queryRepository ??= new QueryRepository(_connection, _transaction);

        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }

            _disposed = true;
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }
    }
}
