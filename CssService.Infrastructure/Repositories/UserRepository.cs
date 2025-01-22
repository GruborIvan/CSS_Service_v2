using AutoMapper;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using CssService.Infrastructure.Models;
using Dapper;

namespace CssService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DapperContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var query = "SELECT acUserId,anUserChg FROM tHE_SetSubjContact";

            using var connection = _context.CreateConnection();
            
            var usersDbo = await connection.QueryAsync<UserDbo>(query);
            var users = _mapper.Map<IEnumerable<User>>(usersDbo);
            return users;
        }
    }
}