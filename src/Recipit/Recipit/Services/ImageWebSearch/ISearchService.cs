namespace Recipit.Services.ImageWebSearch
{
    public interface ISearchService
    {
        Task<string> GetImageUrlByName(string searchQuery);
    }
}
