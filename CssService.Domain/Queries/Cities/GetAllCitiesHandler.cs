using CssService.Domain.Constants;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CssService.Domain.Queries.Cities
{
    public class GetAllCitiesHandler : IQueryHandler<GetAllCities, IEnumerable<City>>
    {
        private readonly IMemoryCache _cache;
        private readonly ICityRepository _cityRepository;

        public GetAllCitiesHandler(ICityRepository cityRepository, IMemoryCache cache)
        {
            _cityRepository = cityRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<City>> Handle(GetAllCities request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(CacheKeys.CITIES_KEY, out IEnumerable<City> cachedData))
            {
                cachedData = await _cityRepository.GetCitiesAsync(isBulkCall: false);

                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                // Store data in cache
                _cache.Set(CacheKeys.CITIES_KEY, cachedData, cacheOptions);

            }

            return cachedData;
        }
    }
}