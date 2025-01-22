using CssService.Domain.Interfaces;
using MediatR;

namespace CssService.Domain.Commands.UpdateNarudzbina
{
    public class UpdateNarudzbinaCommandHandler : IRequestHandler<UpdateNarudzbinaCommand, Guid>
    {
        private readonly INarudzbinaRepository _narudzbinaRepository;

        public UpdateNarudzbinaCommandHandler(INarudzbinaRepository narudzbinaRepository)
        {
            _narudzbinaRepository = narudzbinaRepository;
        }   

        public async Task<Guid> Handle(UpdateNarudzbinaCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.AcKey))
            {
                await _narudzbinaRepository.UpdateNarudzbina(request.AnValue, request.AcKey);
            }

            return Guid.NewGuid();
        }
    }
}
