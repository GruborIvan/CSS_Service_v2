using CssService.Domain.Constants;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CssService.Domain.Queries.Statuses
{
    public class GetStatusesHandler : IQueryHandler<GetStatuses, IEnumerable<Status>>
    {
        private readonly IMemoryCache _cache;
        private readonly IStatusRepository _statusRepository;

        public GetStatusesHandler(IStatusRepository statusRepository, IMemoryCache cache)
        {
            _statusRepository = statusRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Status>> Handle(GetStatuses request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(CacheKeys.STATUSES_KEY, out IEnumerable<Status> cachedData))
            {
                cachedData = await _statusRepository.GetStatusesAsync(isBulkCall: false);

                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                // Store data in cache
                _cache.Set(CacheKeys.STATUSES_KEY, cachedData, cacheOptions);
            }

            return cachedData;
        }
    }
}