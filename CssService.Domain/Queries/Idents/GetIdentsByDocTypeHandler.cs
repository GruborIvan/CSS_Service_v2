using CssService.Domain.Constants;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CssService.Domain.Queries.Idents
{
    public class GetIdentsByDocTypeHandler : IQueryHandler<GetIdentsByDocType, IEnumerable<Ident>>
    {
        private readonly IIdentRepository _identRepository;
        private readonly IMemoryCache _cache;

        public GetIdentsByDocTypeHandler(IIdentRepository identRepository, IMemoryCache cache)
        {
            _identRepository = identRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Ident>> Handle(GetIdentsByDocType request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(CacheKeys.IDENTS_KEY, out IEnumerable<Ident> cachedData))
            {
                cachedData = await _identRepository.GetIdentsByDocTypeAsync(request.AcDocType, isBulkCall: false);

                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                // Store data in cache
                _cache.Set(CacheKeys.IDENTS_KEY, cachedData, cacheOptions);
            }

            return cachedData;
        }
    }
}