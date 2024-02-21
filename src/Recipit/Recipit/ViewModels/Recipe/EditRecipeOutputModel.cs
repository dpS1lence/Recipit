namespace Recipit.ViewModels.Recipe
{
    using AutoMapper;
    using Recipit.Infrastructure.Mapping;
    using Infrastructure.Data.Models;

    public class EditRecipeOutputModel : IMapFrom<Recipe>
    {
        public int Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string UserId { get; private set; } = default!;
        public string? Photo { get; set; }
        public decimal Calories { get; set; }
        public string Category { get; set; } = default!;
        public IEnumerable<Tuple<string, string>>? Products { get; set; }

        public void Mapping(Profile map)
        {
            map.CreateMap<EditRecipeOutputModel, Recipe>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.NutritionalValue, opt => opt.Ignore())
                .ForMember(dest => dest.PublishDate, opt => opt.Ignore())
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.ProductRecipes, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(src => src.Products, opt => opt.Ignore());
        }
    }
}
