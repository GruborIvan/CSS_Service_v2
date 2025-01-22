using CssService.Domain.Constants;
using CssService.Domain.Interfaces;
using CssService.Domain.Interfaces.ExternalServices;
using CssService.Domain.Models.ServisCollections;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CssService.Domain.Queries.Servisi
{
    public class GetAllServiceDataHandler : IQueryHandler<GetAllServiceData, ServisReturn>
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IIdentRepository _identRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IMasinaRepository _masinaRepository;
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly INarudzbinaRepository _narudzbinaRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkService;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;

        public GetAllServiceDataHandler(
            ISubjectRepository subjectRepository, 
            IIdentRepository identRepository, 
            ICityRepository cityRepository, 
            IMasinaRepository masinaRepository, 
            IContactPersonRepository contactPersonRepository, 
            IServiceRepository serviceRepository,
            INarudzbinaRepository narudzbinaRepository,
            IUnitOfWorkRepository unitOfWorkService,
            IMemoryCache memoryCache,
            ILogger<GetAllServiceDataHandler> logger)
        {
            _subjectRepository = subjectRepository;
            _identRepository = identRepository;
            _cityRepository = cityRepository;
            _masinaRepository = masinaRepository;
            _contactPersonRepository = contactPersonRepository;
            _serviceRepository = serviceRepository;
            _narudzbinaRepository = narudzbinaRepository;
            _unitOfWorkService = unitOfWorkService;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<ServisReturn> Handle(GetAllServiceData request, CancellationToken cancellationToken)
        {
            // Check if transaction is available and begin if not.
            if (!_unitOfWorkService.IsTransactionActive())
                _unitOfWorkService.BeginTransaction();

            try
            {
                var subjekti = await GetOrSetCacheAsync(CacheKeys.SUBJECTS_KEY, () => _subjectRepository.GetAllSubjectsAsync(isBulkCall: true));
                var identi = await GetOrSetCacheAsync(CacheKeys.IDENTS_KEY, () => _identRepository.GetIdentsByDocTypeAsync(request.AcDocTypeService, isBulkCall: true));
                var gradovi = await GetOrSetCacheAsync(CacheKeys.CITIES_KEY, () => _cityRepository.GetCitiesAsync(isBulkCall: true));
                var masine = await GetOrSetCacheAsync(CacheKeys.SKLADISTE_KEY, () => _masinaRepository.GetMasineAsync());
                var masinaKorisnici = await GetOrSetCacheAsync(CacheKeys.USERS_KEY, () => _masinaRepository.GetMasinaKorisniciAsync());
                var contactPersons = await GetOrSetCacheAsync(CacheKeys.USERS_KEY, () => _contactPersonRepository.GetAllContactPersonsAsync());
                var servisi = await GetOrSetCacheAsync(CacheKeys.SUBJECTS_KEY, () => _serviceRepository.GetAllServicesAsync(request.AcDocTypeService));
                var stavkeServisa = await GetOrSetCacheAsync(CacheKeys.STATUSES_KEY, () => _narudzbinaRepository.GetNarudzbinaItemsByDocTypeAsync(request.AcDocTypeService));

                return new ServisReturn()
                {
                    Subjekti = subjekti,
                    Identi = identi,
                    Gradovi = gradovi,
                    Masine = masine,
                    MasinaKorisnik = masinaKorisnici,
                    ContactPersons = contactPersons,
                    Servisi = servisi,
                    StavkeServisa = stavkeServisa,
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Retrieving all Service data FAILED \n \n {e}.");
                _unitOfWorkService.Rollback();
                throw;
            }
            finally
            {
                _unitOfWorkService.EndTransaction();
                _unitOfWorkService.DisposeTransaction();
            }
        }

        public async Task<T> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<T>> retrieveData, MemoryCacheEntryOptions cacheOptions = null)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out T cachedData))
            {
                // Retrieve data if not in cache
                cachedData = await retrieveData();

                // Set default cache options if none provided
                cacheOptions ??= new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                // Store data in cache
                _memoryCache.Set(cacheKey, cachedData, cacheOptions);
            }

            return cachedData;
        }

    }
}