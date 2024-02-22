namespace Recipit.ViewModels.Recipe
{
    using AutoMapper;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Infrastructure.Mapping;

    public class RecipeViewModel : IMapFrom<Recipe>
    {
        public int Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public int NutritionalValue { get; set; }
        public DateTime PublishDate { get; set; }
        public IFormFile Photo { get; set; } = default!;
        public decimal AverageRating { get; set; }
        public decimal Calories { get; set; }
        public string Category { get; set; } = default!;
        public string? Products { get; set; }

        public void Mapping(Profile map)
        {
            map.CreateMap<RecipeViewModel, Recipe>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.NutritionalValue, opt => opt.MapFrom(src => src.NutritionalValue))
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.AverageRating))
                .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category)).ReverseMap();
        }
    }
}