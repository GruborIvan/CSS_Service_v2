using CssService.Domain.Constants;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CssService.Domain.Queries
{
    public class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<User>>
    {
        private readonly IMemoryCache _cache;
        private readonly IUserRepository _usersRepository;

        public GetUsersHandler(IUserRepository userRepository, IMemoryCache cache)
        {
            _usersRepository = userRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<User>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(CacheKeys.USERS_KEY, out IEnumerable<User> cachedData))
            {
                cachedData = await _usersRepository.GetUsersAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                // Store data in cache
                _cache.Set(CacheKeys.USERS_KEY, cachedData, cacheOptions);
            }

            return cachedData; 
        }
    }
}