using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Domain.Models.NarudzbinaCollections;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Transactions;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace CssService.Infrastructure.Repositories
{
    public class SkladisteRepository : ISkladisteRepository
    {
        private readonly DapperContext _context;
        private readonly TransactionManagerService _transactionManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SkladisteRepository> _logger;
        private readonly IMapper _mapper;

        public SkladisteRepository(
            DapperContext context, 
            TransactionManagerService transactionManager,
            ILogger<SkladisteRepository> logger,
            IServiceProvider serviceProvider,
            IMapper mapper)
        {
            _context = context;
            _transactionManager = transactionManager;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Skladiste> GetSkladisteByDocTypeAsync(string acDocType)
        {
            var query = $"SELECT acIssuer,acReceiver FROM tPA_SetDocType WHERE acDocType = '{acDocType}'";

            using var connection = _context.CreateConnection();
            
            var skladisteDbo = await connection.QuerySingleOrDefaultAsync<SkladisteDbo>(query);
            var skladiste = _mapper.Map<Skladiste>(skladisteDbo);
            return skladiste;
        }

        public async Task SaveDokumentPrenosaAsync(string acKeySkladiste, string adDate, string docTypeStockTranfer, string docTypeWarehouseIssuer, string docTypeWarehouseReceiver)
        {
            string query = $@"
                INSERT INTO tHE_Move (acKey,acDocType,adDate,acReceiver,acIssuer,acReceiverStock,acIssuerStock,acPrsn3,acWayOfSale,acPriceRate,acCurrency)
                VALUES
			        (@acKey,@acDocType,@Date,@acReceiver,@acIssuer,@acReceiverStock,@acIssuerStock,@acPrsn3,@acWayOfSale,@acPriceRate,@acCurrency)
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@acKey", acKeySkladiste);
            parameters.Add("@acDocType", docTypeStockTranfer);
            parameters.Add("@Date", adDate);
            parameters.Add("@acReceiver", docTypeWarehouseReceiver);
            parameters.Add("@acIssuer", docTypeWarehouseIssuer);
            parameters.Add("@acReceiverStock", "Y");
            parameters.Add("@acIssuerStock", "Y");
            parameters.Add("@acPrsn3", docTypeWarehouseReceiver);
            parameters.Add("@acWayOfSale", "Z");
            parameters.Add("@acPriceRate", "1");
            parameters.Add("@acCurrency", "RSD");

            try
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                await connection.ExecuteAsync(query, parameters, transaction: transaction);
                _logger.LogInformation($"Saved dokument prenosa for acKey:{acKeySkladiste} skladiste.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Save dokument prenosa for acKeySkladiste:{acKeySkladiste} has failed. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }

        public async Task<string> SaveSkladisteAsync(string docType, string adDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@cPoslDog", docType);
            parameters.Add("@dDatum", adDate);
            parameters.Add("@cKljuc", dbType: DbType.String, size: 13, direction: ParameterDirection.Output);

            try
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                await connection.ExecuteAsync("pHE_MoveGetNewKey", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                string key = parameters.Get<string>("@cKljuc");

                _logger.LogInformation($"Saved skladiste for acKey:{key}.");

                return key;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Saving skladiste has failed. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }

        public async Task SaveStavkaDokumentaPrenosaAsync(string acKeySkladiste, NarudzbinaItemPost stavka)
        {
            var query = @$"
                DECLARE @anNo INT;
                SELECT @anNo = ISNULL(MAX(anNo), 0) + 
                    CASE 
                        WHEN MAX(anNo) IS NULL THEN 0 
                        ELSE 1 
                    END
                FROM tHE_MoveItem 
                WHERE acKey = @acKey;
                
                INSERT INTO 
                tHE_MoveItem (acKey,anNo,acIdent,acName,anQty,anQtyTemp,acUM,anPrice,anRebate,acVATCode,anVAT,anStockPrice)
                VALUES
                (@acKey,@anNo,@acIdent,@acName,@anQty,@anQtyTemp,@acUM,@anPrice,@anRebate,@acVATCode,@anVAT,@anStockPrice)
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@acKey", acKeySkladiste);
            parameters.Add("@acIdent", stavka.AcIdent);
            parameters.Add("@acName", stavka.AcName);
            parameters.Add("@anQty", stavka.AnQty);
            parameters.Add("@anQtyTemp", stavka.AnQty);
            parameters.Add("@acUM", stavka.AcUm);
            parameters.Add("@anPrice", stavka.AnRTPrice);
            parameters.Add("@anRebate", 0);
            parameters.Add("@acVATCode", stavka.AcVatCode);
            parameters.Add("@anVAT", stavka.AnVat);
            parameters.Add("@anStockPrice", stavka.AnSalePrice);

            try
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                await connection.ExecuteAsync(query, parameters, transaction: transaction);
                _logger.LogInformation($"Saved stavka dokumenta prenosa.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Saving stavka dokumenta prenosa has failed. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }
    }
}