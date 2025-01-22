using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Transactions;
using Dapper;

namespace CssService.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly DapperContext _context;
        private readonly IMapper _mapper;
        private readonly TransactionManagerService _unitOfWorkService;

        public CityRepository(DapperContext context, IMapper mapper, TransactionManagerService unitOfWorkService)
        {
            _mapper = mapper;
            _unitOfWorkService = unitOfWorkService;
            _context = context;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(bool isBulkCall)
        {
            var query = "SELECT acPost,acName,acRegion FROM tHE_SetPostCode";

            if (!isBulkCall)
            {
                using var connection = _context.CreateConnection();

                var citiesDbo = await connection.QueryAsync<CityDbo>(query);
                var cities = _mapper.Map<IEnumerable<City>>(citiesDbo);
                return cities;
            }
            else
            {
                var transaction = _unitOfWorkService.GetCurrentTransaction();
                var connection = transaction.Connection;

                var citiesDbo = await connection.QueryAsync<CityDbo>(query, transaction: transaction);
                var cities = _mapper.Map<IEnumerable<City>>(citiesDbo);
                return cities;
            }
        }
    }
}