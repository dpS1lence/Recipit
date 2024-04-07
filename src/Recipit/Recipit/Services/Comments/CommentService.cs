namespace Recipit.Services.Comments
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Contracts;
    using Recipit.Contracts.Constants;
    using Recipit.Contracts.Helpers;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.ViewModels.Comments;
    using Comment = Infrastructure.Data.Models.Comment;

    public class CommentService(RecipitDbContext context, UserManager<RecipitUser> userManager, ILogger<CommentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
        : ICommentService
    {
        private readonly RecipitDbContext _context = context;
        private readonly UserManager<RecipitUser> _userManager = userManager;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<int> Create(CommentSendModel model)
        {
            Validate.Model(model, _logger);

            ArgumentException.ThrowIfNullOrEmpty(model.Text);
            if (model.RecipeId <= 0)
                throw new ArgumentException(nameof(model.RecipeId));

            var comment = _mapper.Map<Comment>(model);
            comment.DatePosted = DateTime.UtcNow;
            comment.UserId = GetUser.Id(_httpContextAccessor);
            comment.User = await GetUser.Data(_userManager, _httpContextAccessor);

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment.Id;
        }

        public async Task Delete(int id)
        {
            var user = await GetUser.Data(_userManager, _httpContextAccessor);

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(RecipitRole.Administrator))
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(a => a.Id == id);

                if (comment != null)
                    _context.Comments.Remove(comment);
            }
            else
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(a => a.Id == id && a.UserId == user.Id);

                if(comment != null) 
                    _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
        }
    }
}
