using CssService.Domain.Constants;
using CssService.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CssService.Domain.Queries.Skladiste
{
    public class GetSkladisteByDocTypeHandler : IQueryHandler<GetSkladisteByDocType, Models.Skladiste>
    {
        private readonly ISkladisteRepository _skladisteRepository;
        private readonly IMemoryCache _cache;

        public GetSkladisteByDocTypeHandler(ISkladisteRepository skladisteRepository, IMemoryCache cache)
        {
            _skladisteRepository = skladisteRepository;
            _cache = cache;
        }

        public async Task<Models.Skladiste> Handle(GetSkladisteByDocType request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(CacheKeys.SKLADISTE_KEY, out Models.Skladiste cachedData))
            {
                cachedData = await _skladisteRepository.GetSkladisteByDocTypeAsync(request.AcDocType);

                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                // Store data in cache
                _cache.Set(CacheKeys.SKLADISTE_KEY, cachedData, cacheOptions);
            }

            return cachedData;
        }
    }
}