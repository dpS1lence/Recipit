namespace Recipit.Services.Followers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Contracts.Exceptions;
    using Recipit.Contracts.Constants;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Infrastructure.Extensions.Contracts;
    using Recipit.ViewModels.Followers;
    using System.Collections.Generic;
    using Recipit.Pagination;
    using System.Linq.Expressions;
    using Recipit.Contracts.Enums;
    using Recipit.Pagination.Contracts;

    public class FollowerService
        (RecipitDbContext context, UserManager<RecipitUser> userManager, ILogger<FollowerService> logger, IMapper mapper, IConfiguration configuration) 
        : IFollowerService
    {
        private readonly RecipitDbContext _context = context;
        private readonly UserManager<RecipitUser> _userManager = userManager;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly UserSettings _settings = configuration.GetSection("UserSettings").Get<UserSettings>()!;

        public async Task Delete(string followerId)
        {
            var follower = await _userManager.FindByIdAsync(followerId) 
                ?? throw new UserNotFoundException(followerId);
            
            await Deactivate(follower);
        }

        public async Task<IPage<FollowerViewModel>> GetAll(int pageIndex = 1, int pageSize = 50)
        {
            var all = await _context.Users.Where(user => user.Email != _settings.Email).ToListAsync();

            return new Page<FollowerViewModel>(all.Select(_mapper.Map<FollowerViewModel>), pageIndex + 1, pageSize, all.Count);
        }

        private async Task Deactivate(RecipitUser user)
        {
            user.Email = "deleteduser@recipit";
            user.FirstName = "deleted";
            user.LastName = "user";
            user.UserName = "deleteduser";
            user.Photo = _settings.Photo;

            await _userManager.RemovePasswordAsync(user);

            _logger.LogInformation(Follower.Deleted, user.Id);
        }
    }
}
