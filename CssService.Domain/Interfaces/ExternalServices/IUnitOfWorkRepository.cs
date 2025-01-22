namespace CssService.Domain.Interfaces.ExternalServices
{
    public interface IUnitOfWorkRepository
    {

        void BeginTransaction();
        void EndTransaction();
        void DisposeTransaction();
        public void CloseDbConnection();
        public void Rollback();
        public bool IsTransactionActive();
    }
}