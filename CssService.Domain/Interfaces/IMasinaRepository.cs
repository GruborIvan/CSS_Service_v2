using CssService.Domain.Models;

namespace CssService.Domain.Interfaces
{
    public interface IMasinaRepository
    {
        Task<IEnumerable<Masina>> GetMasineAsync();
        Task<IEnumerable<MasinaKorisnik>> GetMasinaKorisniciAsync();
        Task SaveMasinaAndUserAsync(string acReciever, string acFieldSA, string acFieldSB, string adFieldDC, string adFieldDD);
    }
}