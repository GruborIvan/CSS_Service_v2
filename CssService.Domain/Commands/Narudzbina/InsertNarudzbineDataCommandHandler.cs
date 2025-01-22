using CssService.Domain.Interfaces;
using CssService.Domain.Interfaces.ExternalServices;
using MediatR;

namespace CssService.Domain.Commands.Narudzbina
{
    public class InsertNarudzbineDataCommandHandler : IRequestHandler<InsertNarudzbineDataCommand, Guid>
    {
        private readonly INarudzbinaRepository _narudzbinaRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public InsertNarudzbineDataCommandHandler(
            INarudzbinaRepository narudzbinaRepository,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _narudzbinaRepository = narudzbinaRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<Guid> Handle(InsertNarudzbineDataCommand request, CancellationToken cancellationToken)
        {
            // Start transaction.
            _unitOfWorkRepository.BeginTransaction();

            // Izdvajanje stavki cije narudzbine vec postoje.
            var existingStavke = request.StavkeNarudzbine.Where(x => x.AcKey != null).ToList();

            foreach (var stavka in existingStavke)
            {
                // Add stavka to database.
                await _narudzbinaRepository.PostNarudzbinaItemAsync(stavka);

                // Update narudzbina price full.
                var anValueIncrease = stavka.AnRTPrice * stavka.AnQty;
                var anForPayIncrease = stavka.AnSalePrice * stavka.AnQty;
                await _narudzbinaRepository.UpdateNarudzbinaPriceAsync(stavka.AcKey, anValueIncrease, anForPayIncrease);
            }

            // Izdvajanje svih ostalih stavki.
            var nonAssignedStavke = request.StavkeNarudzbine.Where(x => x.AcKey == null);
            
            foreach (var narudzbina in request.Narudzbine)
            {
                // Post Narudzbina & get acKey.
                string acKey = await _narudzbinaRepository.PostNarudzbinaAsync(narudzbina.AcReciever, narudzbina.AdDate.ToString(), narudzbina.UserID, narudzbina.DocType);

                double anValueIncrease = 0;
                double anForPayIncrease = 0;

                // Get all stavke for given narudzbina.
                var stavkeNarudzbine = nonAssignedStavke.Where(x => x.AcKeyLocal == narudzbina.AcKeyLocal);

                // Dodavanje svih stavki novokreirane narudzbine.
                foreach (var stavkaNarudzbine in stavkeNarudzbine)
                {
                    stavkaNarudzbine.AcKey = acKey;

                    anValueIncrease += stavkaNarudzbine.AnRTPrice * stavkaNarudzbine.AnQty;
                    anForPayIncrease += stavkaNarudzbine.AnSalePrice * stavkaNarudzbine.AnQty;

                    await _narudzbinaRepository.PostNarudzbinaItemAsync(stavkaNarudzbine);
                }

                // Azuriranje cene narudzbine.
                await _narudzbinaRepository.UpdateNarudzbinaPriceAsync(acKey, anValueIncrease, anForPayIncrease);
            }

            return Guid.NewGuid();
        }
    }
}