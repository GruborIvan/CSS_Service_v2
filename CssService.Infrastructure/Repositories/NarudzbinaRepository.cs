using AutoMapper;
using CssService.Domain.Exceptions;
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
    public class NarudzbinaRepository : INarudzbinaRepository
    {
        private readonly IMapper _mapper;
        private readonly TransactionManagerService _transactionManager;
        private readonly ILogger<NarudzbinaRepository> _logger;

        public NarudzbinaRepository(
            TransactionManagerService unitOfWorkRepository,
            ILogger<NarudzbinaRepository> logger,
            IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _transactionManager = unitOfWorkRepository;
        }

        public async Task<IEnumerable<Narudzbina>> GetNarudzbineByDocTypeAsync(string docType)
        {
            var query = $"SELECT acKey,acReceiver,acDocType,adDate,adDateValid,anClerk,anValue,anForPay,acWarehouse,acStatus FROM tHE_Order WHERE acDocType = '{docType}'";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var narudzbineDbo = await connection.QueryAsync<NarudzbinaDbo>(query, transaction: transaction);
            var narudzbine = _mapper.Map<IEnumerable<Narudzbina>>(narudzbineDbo);
            return narudzbine.OrderBy(x => x.AdDate);
        }

        public async Task<IEnumerable<NarudzbinaItem>> GetNarudzbinaItemsByDocTypeAsync(string docType)
        {
            var query = @$"
                SELECT ord.acKey,anNo,acIdent,acName,anRTPrice,anSalePrice,acUM,acVATCode,ordItm.anVAT,anQty,acUM2
                FROM tHE_OrderItem ordItm 
                LEFT JOIN tHE_Order ord ON ordItm.acKey = ord.acKey  
                WHERE acDocType = '{docType}'
            ";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var narudzbinaItemDbo = await connection.QueryAsync<NarudzbinaItemDbo>(query, transaction: transaction);
            var narudzbinaItems = _mapper.Map<IEnumerable<NarudzbinaItem>>(narudzbinaItemDbo);
            return narudzbinaItems;
        }

        public async Task<string> PostNarudzbinaAsync(string acReciever, string adDate, int userId, string acDocType)
        {
            try
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                var parameters = new DynamicParameters();
                parameters.Add("@cNarocnik", acReciever);
                parameters.Add("@cPrejemnik", acReciever);
                parameters.Add("@cSkladisce", string.Empty);
                parameters.Add("@cPoslDog", acDocType);
                parameters.Add("@dDatum", adDate);
                parameters.Add("@nUserId", userId);
                parameters.Add("@cOddelek", string.Empty);
                parameters.Add("@cKljuc", dbType: DbType.String, size: 13, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("pHE_OrderCreHead", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                string key = parameters.Get<string>("@cKljuc");

                if (key is null) throw new NarudzbinaInsertException("There was an issue inserting order in database.");

                _logger.LogInformation($"Narudzbina with acKey: {key} has been added.");

                return key;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Post narudzbina has failed. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }

        public async Task PostNarudzbinaItemAsync(NarudzbinaItemPost orderItem)
        {
            double anPVVATBase = orderItem.AnRTPrice * orderItem.AnQty;
            double anPVVAT = anPVVATBase * orderItem.AnVat / 100;
            double anPVForPay = anPVVATBase + anPVVAT;

            string query = $@"
                DECLARE @anNo INT;
                SELECT @anNo = ISNULL(MAX(anNo), 0) FROM tHE_OrderItem WHERE acKey = '{orderItem.AcKey}';
                
                IF @anNo IS NOT NULL
                    SET @anNo = @anNo + 1;
                ELSE
                    SET @anNo = 1;
                
                SELECT @anNo;

                INSERT INTO tHE_OrderItem 
                (anNo,acKey,acIdent,anQty,acName,anRTPrice,anPrice,anSalePrice,acUM,acVATCode,anVAT,acUMConverted,anPackQty,anQtyConverted,anRetailPrice,anPVValue,anPVVATBase,anPVVAT,anPVOCVAT,anPVForPay,anPVOCForPay,anPVOCValue,anPVOCVATBase,acUM2) 
                VALUES 
                (@anNo,@acKey,@acIdent,@anQty,@acName,@anRTPrice,@anRTPrice,@anSalePrice,@acUm,@acVatCode,@anVat,@acUMConverted,@anQty,@anQty,@anSalePrice,@anPVValue,@anPVValue,@anPVVAT,@anPVVAT,@anPVForPay,@anPVForPay,@anPVValue,@anPVValue,@anUMToUM2)
            ";

            var parameters = new
            {
                acKey = orderItem.AcKey,
                acIdent = orderItem.AcIdent,
                anQty = orderItem.AnQty,
                acName = orderItem.AcName,
                anRTPrice = orderItem.AnRTPrice,
                anSalePrice = orderItem.AnSalePrice,
                acUm = orderItem.AcUm,
                acVatCode = orderItem.AcVatCode,
                anVat = orderItem.AnVat,
                acUMConverted = orderItem.AcUm,
                anPVValue = anPVVATBase,
                anPVVAT = anPVVAT,
                anPVForPay = anPVForPay,
                anUMToUM2 = orderItem.AnUMToUM2
            };

            try
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                await connection.ExecuteAsync(query, parameters, transaction: transaction);
                _logger.LogInformation($"Narudzbina item {orderItem.AcIdent} has been added.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Post narudzbina item has failed. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }

        public async Task UpdateNarudzbinaPriceAsync(string acKey, double anValueIncrease, double anForPayIncrease)
        {
            string query = @"
                WITH CurrentOrder AS (
                    SELECT anValue, anForPay 
                    FROM tHE_Order 
                    WHERE acKey = @acKey
                )
                UPDATE tHE_Order 
                SET 
                    anValue = (SELECT anValue FROM CurrentOrder) + @anValueIncrease, 
                    anForPay = (SELECT anForPay FROM CurrentOrder) + @anForPayIncrease 
                WHERE 
                    acKey = @acKey;
            ";

            var parameters = new
            {
                acKey,
                anValueIncrease,
                anForPayIncrease
            };

            try
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                await connection.ExecuteAsync(query, parameters, transaction: transaction);
                _logger.LogInformation($"Narudzbina price acKey:{acKey} has been updated.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Narudzbina price update failed. \n {ex}");
                _transactionManager.Rollback();
                throw;
            }
        }

        public async Task UpdateNarudzbina(double anValue, string acKey)
        {
            double anForPay = Math.Round(anValue * 1.1525, 2);
            string query = "UPDATE tHE_Order SET anValue = @anValue, anForPay = @anForPay WHERE acKey = @acKey";

            var parameters = new DynamicParameters();
            parameters.Add("@anValue", anValue);
            parameters.Add("@anForPay", anForPay);
            parameters.Add("@acKey", acKey);

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            await connection.ExecuteAsync(query, parameters, transaction: transaction);
        }
    }
}