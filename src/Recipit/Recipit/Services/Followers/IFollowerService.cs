namespace Recipit.Services.Followers
{
    using Recipit.ViewModels.Followers;

    public interface IFollowerService
    {
        Task Delete(string followerId);
        Task<IEnumerable<FollowerViewModel>> GetAll();
    }
}
