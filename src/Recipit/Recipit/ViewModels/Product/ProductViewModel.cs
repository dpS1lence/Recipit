namespace Recipit.ViewModels.Product
{
    using AutoMapper;
    using Recipit.Infrastructure.Mapping;
    using ProductDbo = Infrastructure.Data.Models.Product;
    
    public class ProductViewModel : IMapFrom<ProductDbo>
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int Calories { get; set; }
        public string? Photo { get; set; }
        public void Mapping(Profile map)
        {
            map.CreateMap<ProductViewModel, ProductDbo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo)).ReverseMap();
        }
    }
}
