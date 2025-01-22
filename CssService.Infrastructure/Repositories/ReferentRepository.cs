using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Repositories.ExternalServicesRepository;
using CssService.Infrastructure.Transactions;
using Dapper;

namespace CssService.Infrastructure.Repositories
{
    public class ReferentRepository : IReferentRepository
    {
        private readonly TransactionManagerService _unitOfWorkService;
        private readonly IMapper _mapper;

        public ReferentRepository(TransactionManagerService unitOfWorkService, IMapper mapper)
        {
            _unitOfWorkService = unitOfWorkService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Referent>> GetReferentiAsync()
        {
            var query = $"SELECT acName, acMiddle, acSurname, anUserId FROM tHE_SetSubjContact";

            var transaction = _unitOfWorkService.GetCurrentTransaction();
            var connection = transaction.Connection;

            var referentiDbo = await connection.QueryAsync<ReferentDbo>(query, transaction: transaction);
            var referenti = _mapper.Map<IEnumerable<Referent>>(referentiDbo);

            return referenti;
        }
    }
}
