using CssService.Domain.Interfaces.ExternalServices;
using CssService.Infrastructure.Transactions;

namespace CssService.Infrastructure.Repositories.ExternalServicesRepository
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly TransactionManagerService _unitOfWorkService;

        public UnitOfWorkRepository(TransactionManagerService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        public void BeginTransaction()
        {
            _unitOfWorkService.BeginTransaction();
        }

        public void DisposeTransaction()
        {
            _unitOfWorkService.DisposeTransaction();
        }

        public void EndTransaction()
        {
            _unitOfWorkService.Commit();
        }

        public void Rollback()
        {
            _unitOfWorkService.Rollback();
        }

        public void CloseDbConnection()
        {
            var transaction = _unitOfWorkService.GetCurrentTransaction();
            var connection = transaction.Connection;
            connection.Close();
        }

        public bool IsTransactionActive()
        {
            return _unitOfWorkService.IsTransactionActive;
        }
    }
}
