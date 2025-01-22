using CssService.Domain.Interfaces;
using CssService.Domain.Interfaces.ExternalServices;
using CssService.Domain.Models.NarudzbinaReturn;
using Microsoft.Extensions.Logging;

namespace CssService.Domain.Queries.Narudzbine
{
    public class GetAllNarudzbinaDataHandler : IQueryHandler<GetAllNarudzbinaData, NarudzbinaReturn>
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IIdentRepository _identRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IReferentRepository _referentRepository;
        private readonly IMasinaRepository _masinaRepository;
        private readonly INarudzbinaRepository _narudzbinaRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkService;
        private readonly ILogger _logger;

        public GetAllNarudzbinaDataHandler(
            ISubjectRepository subjectRepository, 
            IIdentRepository identRepository, 
            ICityRepository cityRepository, 
            IStatusRepository statusRepository, 
            IReferentRepository referentRepository, 
            IMasinaRepository masinaRepository, 
            INarudzbinaRepository narudzbinaRepository, 
            IUnitOfWorkRepository unitOfWorkService,
            ILogger<GetAllNarudzbinaDataHandler> logger)
        {
            _subjectRepository = subjectRepository;
            _identRepository = identRepository;
            _cityRepository = cityRepository;
            _statusRepository = statusRepository;
            _referentRepository = referentRepository;
            _masinaRepository = masinaRepository;
            _narudzbinaRepository = narudzbinaRepository;
            _unitOfWorkService = unitOfWorkService;
            _logger = logger;
        }

        public async Task<NarudzbinaReturn> Handle(GetAllNarudzbinaData request, CancellationToken cancellationToken)
        {
            _unitOfWorkService.BeginTransaction();

            try
            {
                _logger.LogInformation("Retrieving all Narudzbina data.");

                var subjekti = await _subjectRepository.GetAllSubjectsAsync(isBulkCall: true);
                var identi = await _identRepository.GetIdentsByDocTypeAsync(request.AcDocTypeService, isBulkCall: true);
                var gradovi = await _cityRepository.GetCitiesAsync(isBulkCall: true);
                var statusi = await _statusRepository.GetStatusesAsync(isBulkCall: true);
                var referenti = await _referentRepository.GetReferentiAsync();
                var masine = await _masinaRepository.GetMasineAsync();
                var masinaKorisnici = await _masinaRepository.GetMasinaKorisniciAsync();
                var narudzbine = await _narudzbinaRepository.GetNarudzbineByDocTypeAsync(request.AcDocType);
                var narudzbinaItems = await _narudzbinaRepository.GetNarudzbinaItemsByDocTypeAsync(request.AcDocType);

                _unitOfWorkService.EndTransaction();
                _unitOfWorkService.DisposeTransaction();

                _logger.LogInformation("Retrieving all Narudzbina data completed.");

                return new NarudzbinaReturn()
                {
                    Subjekti = subjekti,
                    Identi = identi,
                    Gradovi = gradovi,
                    Statusi = statusi,
                    Referenti = referenti,
                    Masine = masine,
                    MasinaKorisnik = masinaKorisnici,
                    Narudzbine = narudzbine,
                    NarudzbinaItemi = narudzbinaItems,
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Retrieving all Narudzbina data FAILED \n \n {e}.");
                _unitOfWorkService.Rollback();
                throw;
            }
        }
    }
}