namespace Recipit.Services.Followers
{
    using Recipit.Pagination.Contracts;
    using Recipit.ViewModels.Followers;

    public interface IFollowerService
    {
        Task Delete(string followerId);
        Task<IPage<FollowerViewModel>> GetAll(int pageIndex, int pageSize);
    }
}
