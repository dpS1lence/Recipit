using AutoMapper;
using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Mapping;
using Recipit.ViewModels.Account;
using Recipit.ViewModels.Comments;
using Recipit.ViewModels.Product;
using RecipeDb = Recipit.Infrastructure.Data.Models.Recipe;

namespace Recipit.ViewModels.Recipe
{
    public class RecipeDisplayModel : IMapFrom<RecipeDb>
    {
        public RecipeDisplayModel()
        {
            Products = new HashSet<ProductViewModel>();
            Comments = new HashSet<CommentViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string UserId { get; set; } = default!;

        public UserModel User { get; set; } = default!;

        public int NutritionalValue { get; set; }

        public DateTime PublishDate { get; set; }

        public string Photo { get; set; } = default!;

        public decimal AverageRating { get; set; }

        public decimal Calories { get; set; }

        public string Category { get; set; } = default!;

        public IEnumerable<ProductViewModel> Products { get; set; }
        public IEnumerable<CommentViewModel>? Comments { get; set; }

        public void Mapping(Profile map)
        {
            map.CreateMap<RecipeDb, RecipeDisplayModel>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductRecipes.Select(pr => new ProductViewModel
                {
                    Photo = pr.Product.Photo,
                    Calories = pr.Product.Calories,
                    QuantityDetails = pr.QuantityDetails,
                    Id = pr.Product.Id,
                    Name = pr.Product.Name
                })))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Select(pr => new CommentViewModel
                {
                    DatePosted = pr.DatePosted,
                    Id = pr.Id,
                    Rating = pr.Rating,
                    RecipeId = pr.Id,
                    Text = pr.Text,
                    User = new UserViewModel
                    {
                        Email = pr.User.Email,
                        Username = pr.User.UserName,
                        Photo = pr.User.Photo,
                        Id = pr.User.Id
                    }
                })))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserModel
                {
                    Id = src.User.Id,
                    Photo = src.User.Photo,
                    UserName = src.User.UserName!
                }));

            map.CreateMap<ProductRecipe, ProductViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.QuantityDetails, opt => opt.MapFrom(src => src.QuantityDetails))
                .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Product.Calories))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Product.Photo));

            map.CreateMap<RecipeDisplayModel, RecipeDb>()
                .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.Products.Select(p => new ProductRecipe
                {
                    Product = new Infrastructure.Data.Models.Product
                    {
                        Calories = p.Calories,
                        Id = p.Id,
                        Name = p.Name,
                        Photo = p.Photo ?? "https://media.istockphoto.com/id/1354776457/vector/default-image-icon-vector-missing-picture-page-for-website-design-or-mobile-app-no-photo.jpg"
                    }
                })))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new RecipitUser
                {
                    Id = src.User.Id,
                    Photo = src.User.Photo,
                    UserName = src.User.UserName
                }));
        }
    }
}
