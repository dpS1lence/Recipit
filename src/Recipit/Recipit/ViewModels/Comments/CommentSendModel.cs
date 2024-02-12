using AutoMapper;
using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Mapping;
using Recipit.ViewModels.Recipe;

namespace Recipit.ViewModels.Comments
{
    public class CommentSendModel : IMapFrom<Comment>
    {
        public int RecipeId { get; set; }
        public string Text { get; set; } = default!;
        public int Rating { get; set; }

        public void Mapping(Profile map)
        {
            map.CreateMap<CommentSendModel, Comment>()
                .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.RecipeId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => (decimal)src.Rating)).ReverseMap();
        }
    }
}
