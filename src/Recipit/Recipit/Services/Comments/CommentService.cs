﻿namespace Recipit.Services.Comments
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Recipit.Contracts;
    using Recipit.Contracts.Helpers;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.ViewModels.Comments;

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

            if (string.IsNullOrEmpty(model.Text))
                throw new ArgumentException(nameof(model.Text));
            else if (model.RecipeId <= 0)
                throw new ArgumentException(nameof(model.RecipeId));

            var comment = _mapper.Map<Comment>(model);
            comment.DatePosted = DateTime.UtcNow;
            comment.UserId = GetUser.Id(_httpContextAccessor);
            comment.User = await GetUser.ById(_userManager, _httpContextAccessor);

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment.Id;
        }

        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
