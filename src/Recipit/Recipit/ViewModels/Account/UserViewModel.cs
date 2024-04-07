using AutoMapper;
using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Mapping;
using Recipit.Pagination.Contracts;
using Recipit.ViewModels.Recipe;

namespace Recipit.ViewModels.Account
{
    public class UserViewModel : IMapFrom<Comment>
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Photo { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Username { get; set; } = default!;

        public IEnumerable<RecipeDisplayModel>? UserRecipes { get; set; }

        public void Mapping(Profile map)
        {
            map.CreateMap<UserViewModel, Comment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo)).ReverseMap();
        }
    }
}
