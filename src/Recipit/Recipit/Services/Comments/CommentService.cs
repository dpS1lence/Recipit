namespace Recipit.Services.Comments
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.ViewModels.Comments;

    public class CommentService(RecipitDbContext context, UserManager<RecipitUser> userManager, ILogger<CommentService> logger, IMapper mapper) : ICommentService
    {
        private readonly RecipitDbContext _context = context;
        private readonly UserManager<RecipitUser> _userManager = userManager;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;

        public Task<string> Create(CommentViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
