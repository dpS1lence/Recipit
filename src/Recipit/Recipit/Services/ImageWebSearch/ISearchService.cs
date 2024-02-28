namespace Recipit.Services.ImageWebSearch
{
    public interface ISearchService
    {
        Task<string> ImageUrlByName(string searchQuery);
    }
}
