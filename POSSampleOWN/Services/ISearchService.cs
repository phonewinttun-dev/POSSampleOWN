using POSSampleOWN.DTOs;

namespace POSSampleOWN.Services
{
    public interface ISearchService
    {
        Task<List<SearchDTO>> GeneralSearchAsync(string term);

    }
}
