using CssService.Domain.Models;

namespace CssService.Domain.Interfaces
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetAllSubjectsAsync(bool isBulkCall);
        Task<string> GetSubjectAddressByAcSubject(string acSubject);
    }
}