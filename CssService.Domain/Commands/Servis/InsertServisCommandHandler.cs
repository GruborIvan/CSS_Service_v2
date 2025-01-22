using CssService.Domain.Interfaces;
using CssService.Domain.Interfaces.ExternalServices;
using CssService.Domain.Models.NarudzbinaCollections;
using CssService.Domain.Models.ServisCollections;
using MediatR;

namespace CssService.Domain.Commands.Servis
{
    public class InsertServisCommandHandler : IRequestHandler<InsertServisCommand, Guid>
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly INarudzbinaRepository _narudzbinaRepository;
        private readonly ISignatureRepository _signatureRepository;
        private readonly ISkladisteRepository _skladisteRepository;
        private readonly IMasinaRepository _masinaRepository;
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly IPdfRepository _pdfRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public InsertServisCommandHandler(
            ISubjectRepository subjectRepository,
            IServiceRepository serviceRepository,
            INarudzbinaRepository narudzbinaRepository,
            ISignatureRepository signatureRepository,
            ISkladisteRepository dokumentPrenosaRepository,
            IMasinaRepository masinaRepository,
            IContactPersonRepository contactPersonRepository,
            IPdfRepository pdfRepository,
            IEmailRepository emailRepository,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _subjectRepository = subjectRepository;
            _serviceRepository = serviceRepository;
            _narudzbinaRepository = narudzbinaRepository;
            _signatureRepository = signatureRepository;
            _skladisteRepository = dokumentPrenosaRepository;
            _masinaRepository = masinaRepository;
            _contactPersonRepository = contactPersonRepository;
            _pdfRepository = pdfRepository;
            _emailRepository = emailRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<Guid> Handle(InsertServisCommand request, CancellationToken cancellationToken)
        {
            // Ukoliko nema nijedan servis niti stavka servisa da se doda, preskoci sve korake.
            if (request.Servisi.Count == 0 || request.StavkeServisa.Count == 0)
                return Guid.Empty;

            // START transaction.
            _unitOfWorkRepository.BeginTransaction();

            var existingServisStavke = request.StavkeServisa.Where(x => x.AcKey != null);

            foreach (var stavka in existingServisStavke)
            {
                // Add stavka servisa.
                await _narudzbinaRepository.PostNarudzbinaItemAsync(stavka);
            }

            var remainingStavke = request.StavkeServisa.Where(x => x.AcKey == null);

            foreach (var servis in request.Servisi)
            {
                // 0. Provera da li je ovaj servis vec dodat.
                if (await _serviceRepository.CheckIfServiceAlreadyAdded(servis.AcDoc1, servis.AcReciever))
                    continue;

                // 1. Kreiraj zaglavlje servisa.
                var acKey = await _narudzbinaRepository.PostNarudzbinaAsync(servis.AcReciever, servis.AdDate.ToString(), servis.UserID, request.DocType);

                // 2. Dodeli preostale vrednosti servisu.
                await _serviceRepository.UpdateAdditionalServiceDataAsync(acKey, servis);

                _ = Task.Run(async () =>
                {
                    // 3. Kontakt osobe (Porucioc, Primaoc).
                    await _contactPersonRepository.AddContactPersonAsync(servis.AcFieldSI, servis.AcReciever, servis.AcFieldSD, servis.AcFieldSJ);

                    await _contactPersonRepository.AddContactPersonAsync(servis.AcFieldSG, servis.AcReciever, servis.AcFieldSE, servis.AcFieldSH);

                    // 4. Cuvanje potpisa.
                    await _signatureRepository.SaveSignatureAsync(acKey, servis.Signature);
                });

                // 5. Dokument prenosa.
                var acKeySkladiste = await _skladisteRepository.SaveSkladisteAsync(request.DocTypeStockTranfer, servis.AdDate.ToString("yyyy-MM-dd HH:mm:ss"));
                await _skladisteRepository.SaveDokumentPrenosaAsync(acKeySkladiste, servis.AdDate.ToString("yyyy-MM-dd"), request.DocTypeStockTranfer, request.DocTypeWarehouseIssuer, request.DocTypeWarehouseReceiver);

                // 6. Dodavanje stavke narudzbine i stavke dokumenta prenosa.
                var stavke = remainingStavke.Where(x => x.AcKeyLocal == servis.AcKeyLocal).ToList();

                foreach (var stavka in stavke)
                {
                    stavka.AcKey = acKey;
                    await _narudzbinaRepository.PostNarudzbinaItemAsync(stavka);

                    if (stavka.AcIdent is not "U5" && stavka.AcIdent is not "U7")
                    {
                        await _skladisteRepository.SaveStavkaDokumentaPrenosaAsync(acKeySkladiste, stavka);
                    }
                }

                // 7. Cuvanje masine i masina korisnika.
                await _masinaRepository.SaveMasinaAndUserAsync(servis.AcReciever, servis.AcFieldSA, servis.AcFieldSB, servis.AdFieldDC.ToString("yyyy-MM-dd"), servis.AdFieldDD.ToString("yyyy-MM-dd"));

                var subjectAddress = await _subjectRepository.GetSubjectAddressByAcSubject(servis.AcReciever);

                ExecuteBackgroundEmailAndPdfTask(servis, subjectAddress, stavke, request.Mail, request.Password);
            }

            return Guid.NewGuid();
        }

        private void ExecuteBackgroundEmailAndPdfTask(ServisAdd servis, string subjectAddress, List<NarudzbinaItemPost> stavke, string email, string password)
        {
            _ = Task.Run(async () =>
            {
                // Generate PDF document
                var pdfDocument = _pdfRepository.GeneratePdfDocument(servis, subjectAddress, stavke);

                // Send email
                await _emailRepository.SendEmailConfirmationAsync(servis, pdfDocument, servis.AcDoc1, email, password);
            });
        }
    }
}