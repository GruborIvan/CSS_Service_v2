using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Transactions;
using Dapper;
using System.Data;

namespace CssService.Infrastructure.Repositories
{
    public class IdentRepository : IIdentRepository
    {
        private readonly DapperContext _context;
        private readonly TransactionManagerService _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public IdentRepository(DapperContext context, TransactionManagerService unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<Ident>> GetIdentsByDocTypeAsync(string acDocType, bool isBulkCall)
        {
            IDbConnection connection;

            var query = $@"
                SELECT the_setitem.acident, the_setitem.acName, tHE_SetItem.anRTPrice, tHE_SetItem.anSalePrice,
                the_setitem.acUM, tHE_SetItem.acVATCode, tHE_SetItem.anVAT, tHE_SetItem.anUMToUM2,acWarehouse, anStock
                FROM (select tHE_Stock.acIdent, tHE_Stock.acwarehouse as acWarehouse, tHE_Stock.anStock as anStock from tHE_Stock, tPA_SetDocType
                WHERE tPA_SetDocType.acissuer = tHE_Stock.acwarehouse AND tPA_SetDocType.acdoctype = '{acDocType}') AS zalihe
                RIGHT JOIN the_setitem on the_setitem.acident = zalihe.acIdent
                ORDER BY the_setitem.acident
            ";

            if (!isBulkCall)
            {
                connection = _context.CreateConnection();

                var identsDbo = await connection.QueryAsync<IdentDbo>(query);
                var idents = _mapper.Map<IEnumerable<Ident>>(identsDbo);
                return idents;
            }
            else
            {
                var transaction = _unitOfWorkRepository.GetCurrentTransaction();
                connection = transaction.Connection;

                var identsDbo = await connection.QueryAsync<IdentDbo>(query, transaction: transaction);
                var idents = _mapper.Map<IEnumerable<Ident>>(identsDbo);
                return idents;
            }
        }
    }
}