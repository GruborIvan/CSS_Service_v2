using CssService.Domain.Constants;
using CssService.Domain.Interfaces;
using CssService.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CssService.Domain.Queries.Subjects
{
    public class GetAllSubjectsHandler : IQueryHandler<GetAllSubjects, IEnumerable<Subject>>
    {
        private readonly IMemoryCache _cache;
        private readonly ISubjectRepository _subjectRepository;

        public GetAllSubjectsHandler(ISubjectRepository subjectRepository, IMemoryCache cache)
        {
            _subjectRepository = subjectRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Subject>> Handle(GetAllSubjects request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(CacheKeys.SUBJECTS_KEY, out IEnumerable<Subject> cachedData))
            {
                cachedData = await _subjectRepository.GetAllSubjectsAsync(isBulkCall: false);

                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                // Store data in cache
                _cache.Set(CacheKeys.SUBJECTS_KEY, cachedData, cacheOptions);
            }
            
            return cachedData;
        }
    }
}