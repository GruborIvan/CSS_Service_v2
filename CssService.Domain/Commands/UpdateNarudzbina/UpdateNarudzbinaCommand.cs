using MediatR;

namespace CssService.Domain.Commands.UpdateNarudzbina
{
    public class UpdateNarudzbinaCommand : IRequest<Guid>
    {
        public double AnValue { get; set; }
        public string AcKey { get; set; }

        public UpdateNarudzbinaCommand(double anValue, string acKey)
        {
            AnValue = anValue;
            AcKey = acKey;
        }
    }
}