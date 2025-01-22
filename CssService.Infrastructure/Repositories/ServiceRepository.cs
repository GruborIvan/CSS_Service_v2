using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Domain.Models.ServisCollections;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Transactions;
using Dapper;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CssService.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly TransactionManagerService _transactionManager;
        private readonly ILogger<ServiceRepository> _logger;
        private readonly IMapper _mapper;

        public ServiceRepository(
            TransactionManagerService unitOfWorkRepository,
            ILogger<ServiceRepository> logger,
            IMapper mapper)
        {
            _transactionManager = unitOfWorkRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> CheckIfServiceAlreadyAdded(string acDoc1, string acReceiver)
        {
            const string query = @"
                SELECT CASE WHEN EXISTS (
                    SELECT 1 FROM tHE_Order 
                    WHERE acDoc1 = @acDoc1 AND acReceiver = @acReceiver
                ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
            ";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var parameters = new DynamicParameters();
            parameters.Add("@acDoc1", acDoc1);
            parameters.Add("@acReceiver", acReceiver);

            try
            {
                _logger.LogInformation($"Checking if service {acDoc1} for buyer {acReceiver} is already added.");
                return (bool)await connection.ExecuteScalarAsync(query, parameters, transaction: transaction);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed checking if service {acDoc1} for buyer {acReceiver} is already added. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }

        public async Task<IEnumerable<Servis>> GetAllServicesAsync(string acDocTypeService)
        {
            var query = @$"
                SELECT ord.acKey,acStatus,adDate,acDocType,acDoc1,acReceiver,
                   acFieldSI,acFieldSJ,acFieldSA,acFieldSB,acNote,acInternalNote,
                   adFieldDA,adFieldDB,anFieldNA,anFieldNB,acSignature,acFieldSD,
                   acFieldSC, adFieldDC, adFieldDD, acFieldSG, acFieldSH, acFieldSE
                FROM tHE_Order ord 
                LEFT JOIN _css_Signature s ON ord.acKey = s.acKey 
                WHERE acDocType = '{acDocTypeService}'
                ORDER BY adDate ASC
            ";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var servisiDbo = await connection.QueryAsync<ServiceDbo>(query, transaction: transaction);
            var servisi = _mapper.Map<IEnumerable<Servis>>(servisiDbo);
            return servisi;
        }

        public async Task UpdateAdditionalServiceDataAsync(string acKey, ServisAdd servis)
        {
            string query = $@"
                UPDATE tHE_Order 
                SET acDoc1 = @acDoc1, acFieldSI = @acFieldSI, acFieldSA = @acFieldSA, 
                    acFieldSB = @acFieldSB, acNote = @acNote, acInternalNote = @acInternalNote, 
                    adFieldDA = @adFieldDA, adFieldDB = @adFieldDB, anFieldNA = @anFieldNA, 
                    acFieldSD = @acFieldSD, acFieldSJ = @acFieldSJ, anFieldNB = @anFieldNB,
                    acFieldSC = @acFieldSC, adFieldDC = @adFieldDC, adFieldDD = @adFieldDD,
                    acFieldSG = @acFieldSG, acFieldSH = @acFieldSH, acFieldSE = @acFieldSE
                    WHERE acKey = '{acKey}';
            ";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var parameters = new DynamicParameters();
            parameters.Add("@acDoc1", servis.AcDoc1);
            parameters.Add("@acFieldSI", servis.AcFieldSI);
            parameters.Add("@acFieldSA", servis.AcFieldSA);
            parameters.Add("@acFieldSB", servis.AcFieldSB);
            parameters.Add("@acNote", servis.AcNote);
            parameters.Add("@acInternalNote", servis.AcInternalNote);
            parameters.Add("@adFieldDA", servis.AdFieldDA);
            parameters.Add("@adFieldDB", servis.AdFieldDB);
            parameters.Add("@anFieldNA", servis.AnFieldNA);
            parameters.Add("@acFieldSD", servis.AcFieldSD);
            parameters.Add("@acFieldSJ", servis.AcFieldSJ);
            parameters.Add("@anFieldNB", servis.AnFieldNB);
            parameters.Add("@acFieldSC", servis.AcFieldSC);
            parameters.Add("@adFieldDC", servis.AdFieldDC);
            parameters.Add("@adFieldDD", servis.AdFieldDD);
            parameters.Add("@acFieldSG", servis.AcFieldSG);
            parameters.Add("@acFieldSH", servis.AcFieldSH);
            parameters.Add("@acFieldSE", servis.AcFieldSE);

            try
            {
                await connection.ExecuteAsync(query, parameters, transaction: transaction);
                _logger.LogInformation($"Update service data for acKey:{acKey} has succeeded.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Update service data for acKey:{acKey} failed. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }
    }
}