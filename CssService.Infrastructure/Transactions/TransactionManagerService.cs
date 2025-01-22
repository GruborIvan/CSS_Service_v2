using CssService.Infrastructure.Models;
using System.Data;

namespace CssService.Infrastructure.Transactions
{
    public class TransactionManagerService
    {
        private readonly DapperContext _context;
        private IDbTransaction _transaction;

        public TransactionManagerService(DapperContext context)
        {
            _context = context;
        }

        public bool IsTransactionActive => _transaction != null;

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                var connection = _context.CreateConnection();
                connection.Open();
                _transaction = connection.BeginTransaction();
            }
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            finally
            {
                DisposeTransaction();
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                DisposeTransaction();
            }
        }

        public void DisposeTransaction()
        {
            _transaction?.Connection?.Dispose();
            _transaction?.Dispose();
            _transaction = null;
        }

        public IDbTransaction GetCurrentTransaction()
        {
            if (!IsTransactionActive)
            {
                throw new InvalidOperationException("No active transaction. Nije aktivna transakcija.");
            }
            return _transaction;
        }
    }
}