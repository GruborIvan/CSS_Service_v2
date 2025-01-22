using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Transactions;
using Dapper;

namespace CssService.Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly DapperContext _context;
        private readonly TransactionManagerService _unitOfWorkService;
        private readonly IMapper _mapper;

        public StatusRepository(DapperContext context, TransactionManagerService unitOfWorkService, IMapper mapper)
        {
            _context = context;
            _unitOfWorkService = unitOfWorkService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Status>> GetStatusesAsync(bool isBulkCall)
        {
            var query = "SELECT acStatus,acName FROM tPA_SetDocTypeStat";

            if (!isBulkCall) 
            {
                using var connection = _context.CreateConnection();

                var statusesDbo = await connection.QueryAsync<StatusDbo>(query);
                var statuses = _mapper.Map<IEnumerable<Status>>(statusesDbo);
                return statuses;
            }
            else
            {
                var transaction = _unitOfWorkService.GetCurrentTransaction();
                var connection = transaction.Connection;

                var statusesDbo = await connection.QueryAsync<StatusDbo>(query, transaction: transaction);
                var statuses = _mapper.Map<IEnumerable<Status>>(statusesDbo);
                return statuses;
            }
        }
    }
}