namespace Recipit.Services.Followers
{
    using Recipit.Pagination.Contracts;
    using Recipit.ViewModels.Followers;

    public interface IFollowerService
    {
        Task<IPage<FollowerViewModel>> GetAll(int pageIndex, int pageSize);
    }
}
