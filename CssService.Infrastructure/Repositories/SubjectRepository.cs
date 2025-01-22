using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Transactions;
using Dapper;

namespace CssService.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DapperContext _context;
        private readonly TransactionManagerService _transactionManager;
        private readonly IMapper _mapper;

        public SubjectRepository(DapperContext context, TransactionManagerService unitOfWorkRepository, IMapper mapper)
        {
            _context = context;
            _transactionManager = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync(bool isBulkCall)
        {
            var query = "SELECT acSubject,acName2,acPost,acAddress FROM tHE_SetSubj WHERE acBuyer='T'";

            if (!isBulkCall)
            {
                using var connection = _context.CreateConnection();

                var subjectsDbo = await connection.QueryAsync<SubjectDbo>(query);
                var subjects = _mapper.Map<IEnumerable<Subject>>(subjectsDbo);
                return subjects;
            }
            else
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                var subjectsDbo = await connection.QueryAsync<SubjectDbo>(query, transaction: transaction);
                var subjects = _mapper.Map<IEnumerable<Subject>>(subjectsDbo);
                return subjects;
            }
        }

        public async Task<string> GetSubjectAddressByAcSubject(string acSubject)
        {
            var query = $"SELECT acAddress FROM tHE_SetSubj WHERE acSubject='{acSubject}'";

            try
            {
                var transaction = _transactionManager.GetCurrentTransaction();
                var connection = transaction.Connection;

                var address = await connection.QuerySingleOrDefaultAsync<string>(query, transaction: transaction);
                return address;
            }
            catch
            {
                _transactionManager.Rollback();
                _transactionManager.DisposeTransaction();
                throw;
            }
        }
    }
}