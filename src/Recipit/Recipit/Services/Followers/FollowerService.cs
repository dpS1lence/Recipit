namespace Recipit.Services.Followers
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Extensions.Contracts;
    using Recipit.Pagination;
    using Recipit.Pagination.Contracts;
    using Recipit.ViewModels.Followers;

    public class FollowerService
        (RecipitDbContext context, IMapper mapper, IConfiguration configuration)
        : IFollowerService
    {
        private readonly RecipitDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly UserSettings _settings = configuration.GetSection("UserSettings").Get<UserSettings>()!;

        public async Task<IPage<FollowerViewModel>> GetAll(int pageIndex = 1, int pageSize = 50)
        {
            var all = await _context.Users.Where(user => user.Email != _settings.Email).ToListAsync();

            return new Page<FollowerViewModel>(all.Select(_mapper.Map<FollowerViewModel>), pageIndex + 1, pageSize, all.Count);
        }
    }
}
