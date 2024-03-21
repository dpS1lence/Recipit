namespace Recipit.ViewModels.Recipe
{
    using AutoMapper;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Infrastructure.Mapping;
    using Recipit.ViewModels.Product;

    public class RecipeOutputModel : IMapFrom<Recipe>
    {
        public int Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int NutritionalValue { get; set; }
        public DateTime PublishDate { get; set; }
        public string Photo { get; set; } = default!;
        public decimal AverageRating { get; set; }
        public decimal Calories { get; set; }
        public string Category { get; set; } = default!;
        public IEnumerable<ProductViewModel> Products { get; set; } = default!;

        public void Mapping(Profile map)
        {
            map.CreateMap<Recipe, RecipeOutputModel>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductRecipes.Select(pr => new ProductViewModel
                {
                    Photo = pr.Product.Photo,
                    Calories = pr.Product.Calories,
                    QuantityDetails = pr.QuantityDetails,
                    Id = pr.Product.Id,
                    Name = pr.Product.Name
                })))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.NutritionalValue, opt => opt.MapFrom(src => src.NutritionalValue))
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.AverageRating))
                .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category)).ReverseMap();
        }
    }
}
