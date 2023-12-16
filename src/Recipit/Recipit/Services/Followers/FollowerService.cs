namespace Recipit.Services.Followers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Constants.Exceptions;
    using Recipit.Constants.Followers;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Infrastructure.Extensions.Contracts;
    using Recipit.ViewModels.Followers;
    using System.Collections.Generic;

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

        public async Task<IEnumerable<FollowerViewModel>> GetAll()
        {
            var all = await _context.Users.Where(user => user.Email != _settings.Email).ToListAsync();

            return all.Select(_mapper.Map<FollowerViewModel>);
        }

        private async Task Deactivate(RecipitUser user)
        {
            user.Email = "deleteduser@mail";
            user.FirstName = "deleted";
            user.LastName = "user";
            user.UserName = "deleteduser";
            user.Photo = _settings.Photo;

            await _userManager.RemovePasswordAsync(user);

            _logger.LogInformation(Follower.Deleted, user.Id);
        }
    }
}
