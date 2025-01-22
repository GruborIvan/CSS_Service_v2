using CssService.Domain.Interfaces;
using CssService.Infrastructure.Transactions;
using Dapper;
using Microsoft.Extensions.Logging;

namespace CssService.Infrastructure.Repositories
{
    public class SignatureRepository : ISignatureRepository
    {
        private readonly ILogger<SignatureRepository> _logger;
        private readonly TransactionManagerService _transactionManager;

        public SignatureRepository(
            TransactionManagerService transactionManager,
            ILogger<SignatureRepository> logger)
        {
            _transactionManager = transactionManager;
            _logger = logger;
        }

        public async Task SaveSignatureAsync(string acKey, string signature)
        {
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(acKey))
                 return;

            string query = $@"
                IF NOT EXISTS (
                    SELECT 1 FROM _css_Signature WHERE acKey = @acKey
                )
                BEGIN
                    INSERT INTO _css_Signature (acKey, acSignature)
                    VALUES (@acKey, @signature);
                END; 
            ";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var parameters = new DynamicParameters();
            parameters.Add("@acKey", acKey);
            parameters.Add("@signature", signature);

            try
            {
                await connection.ExecuteAsync(query, parameters, transaction: transaction);
                _logger.LogInformation($"Saving signature for acKey:{acKey} has succeeded.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Saving signature for acKey:{acKey} has failed. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }
    }
}