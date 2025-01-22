using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Transactions;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace CssService.Infrastructure.Repositories
{
    public class MasinaRepository : IMasinaRepository
    {
        private readonly TransactionManagerService _transactionManager;
        private readonly ILogger<MasinaRepository> _logger;
        private readonly IMapper _mapper;

        public MasinaRepository( 
            TransactionManagerService unitOfWorkRepository,
            ILogger<MasinaRepository> logger,
            IMapper mapper)
        {
            _transactionManager = unitOfWorkRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<Masina>> GetMasineAsync()
        {
            var query = "SELECT ID,Naziv_masine,Serijski_broj,Garancija_od,Garancija_do FROM _css_MasinaSerBr";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var masineDbo = await connection.QueryAsync<MasinaDbo>(query, transaction: transaction);
            var masine = _mapper.Map<IEnumerable<Masina>>(masineDbo);
            return masine;
        }

        public async Task<IEnumerable<MasinaKorisnik>> GetMasinaKorisniciAsync()
        {
            var query = "SELECT Masina_id,Subjekt_id FROM _css_MasinaKorisnik";

            var transaction = _transactionManager.GetCurrentTransaction();
            var connection = transaction.Connection;

            var masinaKorisnikDbo = await connection.QueryAsync<MasinaKorisnikDbo>(query, transaction: transaction);
            var masineKorisnici = _mapper.Map<IEnumerable<MasinaKorisnik>>(masinaKorisnikDbo);
            return masineKorisnici;
        }

        public async Task SaveMasinaAndUserAsync(string acReciever, string acFieldSA, string acFieldSB, string adFieldDC, string adFieldDD)
        {
            var query = $@"
                DECLARE @count INT;
                DECLARE @MasinaId INT;
                
                SELECT @count = COUNT(*) FROM _css_MasinaSerBr WHERE Serijski_broj = '{acFieldSB}';
                
                IF @count = 0
                BEGIN
                    INSERT INTO _css_MasinaSerBr (Naziv_masine, Serijski_broj, Garancija_od, Garancija_do)
                    VALUES ('{acFieldSA}', '{acFieldSB}', '{adFieldDC}', '{adFieldDD}');
                    
                    SELECT @MasinaId = Id 
                    FROM _css_MasinaSerBr 
                    WHERE Naziv_masine = '{acFieldSA}' AND Serijski_broj = '{acFieldSB}';
                END
                ELSE
                BEGIN
                    SELECT @MasinaId = Id 
                    FROM _css_MasinaSerBr 
                    WHERE Serijski_broj = '{acFieldSB}';
                END
                
                IF NOT EXISTS (
                    SELECT 1
                    FROM _css_MasinaKorisnik
                    WHERE Masina_id = @MasinaId AND Subjekt_id = '{acReciever}'
                )
                BEGIN
                    INSERT INTO _css_MasinaKorisnik (Masina_id, Subjekt_id)
                    VALUES (@MasinaId, '{acReciever}');
                END;
            ";

            try
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                await connection.ExecuteAsync(query, transaction: transaction);
                _logger.LogInformation($"Adding new machine has succeeded.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Saving machine and contact info failed. \n {ex}");
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }
    }
}