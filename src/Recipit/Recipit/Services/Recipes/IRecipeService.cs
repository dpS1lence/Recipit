﻿using Recipit.Pagination.Contracts;
using Recipit.ViewModels.Recipe;

namespace Recipit.Services.Recipes
{
    public interface IRecipeService
    {
        Task Delete(int recipeId);
        Task Edit(RecipeViewModel recipe);
        Task<IPage<RecipeDisplayModel>> All(int currentPage, int pageSize);
        Task<IPage<RecipeDisplayModel>> Filter(RecipeFilterModel model, int currentPage, int pageSize);
        Task<string> Create(RecipeViewModel recipe);
    }
}
