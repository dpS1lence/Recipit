namespace Recipit.Services.Recipes
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Contracts.Helpers;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Infrastructure.Data.Models.Contracts;
    using Recipit.Infrastructure.Mapping;
    using Recipit.Services.ImageWebSearch;
    using System.Text;
    using System.Text.RegularExpressions;

    public partial class ExternalRecipeCreationService
        (RecipitDbContext context
        , UserManager<RecipitUser> userManager
        , HttpClient httpClient
        , ILogger<RecipeService> logger
        , IMapper mapper
        , IHttpContextAccessor httpContextAccessor
        , ISearchService searchService)
        : IExternalRecipeCreationService
    {
        private readonly RecipitDbContext _context = context;
        private readonly UserManager<RecipitUser> _userManager = userManager;
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger _logger = logger;
        private readonly ISearchService _searchService = searchService;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private static readonly string[] separator = ["\r\n", "\r", "\n"];

        public async Task<int> Create(string url)
        {
            string htmlContent = await FetchHtmlContent(url);

            var title = Title().Matches(htmlContent).FirstOrDefault()?.Groups[1].Value;
            var img = "https://recepti.gotvach.bg" + Image().Matches(htmlContent).FirstOrDefault()?.Groups[1].Value;
            var description = FetchDescription(htmlContent);
            var products = await FetchProducts(htmlContent);

            var user = await GetUser.Data(_userManager, _httpContextAccessor);

            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(img);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(products);
            ArgumentNullException.ThrowIfNull(user);

            var recipe = new Recipe
            {
                Name = title,
                Description = description,
                Photo = img,
                Category = Category.MainCourse,
                UserId = user.Id,
                User = user,
                PublishDate = DateTime.UtcNow,
                Calories = new Random().Next(220, 650),
                NutritionalValue = new Random().Next(220, 650)
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            foreach (var item in products)
            {
                var product = new Product();
                if(await _context.Products.AnyAsync(a => string.Equals(a.Name.ToLower(), item.Name.ToLower())))
                {
                    product = await _context.Products.FirstOrDefaultAsync(a => a.Name == item.Name);
                }
                else
                {
                    product = new Product
                    {
                        Name = item.Name,
                        Photo = item.Img,
                        Calories = RecipeProduct.Calories
                    };

                    _context.Products.Add(product);
                }

                ArgumentNullException.ThrowIfNull(product);
                
                _context.ProductsRecipies.Add(new ProductRecipe
                {
                    Product = product,
                    Recipe = recipe,
                    QuantityDetails = item.Quantity,
                    ProductId = product.Id,
                    RecipeId = recipe.Id
                });
            }

            await _context.SaveChangesAsync();

            return recipe.Id;
        }

        private string FetchDescription(string htmlContent)
        {
            var sb = new StringBuilder();

            var matches = RawDecription().Matches(htmlContent);
            foreach (var match in matches.Cast<Match>())
            {
                if (match.Success)
                {
                    sb.AppendLine(match.Groups[1].Value.Trim());
                }
            }

            string withoutATags = WithoutTags().Replace(sb.ToString(), "$1");
            string withoutSourceLine = WithoutSourceLine().Replace(withoutATags, string.Empty);

            return CleanText().Replace(withoutSourceLine, string.Empty);
        }

        private async Task<List<RecipeProduct>> FetchProducts(string htmlContent)
        {
            var products = new List<RecipeProduct>();

            var matches = Products().Matches(htmlContent);
            foreach (var match in matches.Cast<Match>())
            {
                if (match.Success)
                {
                    string productName = match.Groups[1].Value.Trim();
                    string quantity = match.Groups[2].Value.Trim();
                    var imageUrl = await _searchService.ImageUrlByName(productName);

                    products.Add(new(productName, quantity, imageUrl));
                }
            }

            return products;
        }

        private async Task<string> FetchHtmlContent(string url)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                var htmlContent = await response.Content.ReadAsStringAsync();
                return htmlContent;
            }
            else
            {
                _logger.LogError("Failed to fetch content from {Url}. Status code: {StatusCode}", url, response.StatusCode);
                throw new HttpRequestException($"Failed to fetch content from {url}. Status code: {response.StatusCode}");
            }
        }


        [GeneratedRegex("<[^>]+>")]
        private static partial Regex MyRegex();
        [GeneratedRegex(@"<p class=""desc"">(.*?)<\/p>")]
        private static partial Regex RawDecription();
        [GeneratedRegex("<a href=[^>]+>(.*?)</a>")]
        private static partial Regex WithoutTags();
        [GeneratedRegex(@"<b>Източник:</b>.*", RegexOptions.Singleline)]
        private static partial Regex WithoutSourceLine();
        [GeneratedRegex("<[^>]+>")]
        private static partial Regex CleanText();
        [GeneratedRegex("<div class=\"combocolumn mr\"><h1>(.*)<\\/h1>")]
        private static partial Regex Title();
        [GeneratedRegex("<div id=\"gall\"><img width=\"600\" height=\"350\" src=\"(.*?)\"")]
        private static partial Regex Image();
        [GeneratedRegex(@"<li><b>([^<]+)</b> - ([^<]+)</li>")]
        private static partial Regex Products();
    }

    class RecipeProduct(string name, string quantity, string url) : IMapFrom<Product>
    {
        public string Name { get; set; } = name;
        public string Quantity { get; set; } = quantity;
        public string Img { get; set; } = url;
        public static int Calories { get => new Random().Next(50, 300); }

        public void Mapping(Profile map)
        {
            map.CreateMap<RecipeProduct, Product>()
                .ForMember(dest => dest.Id, opt => opt.AllowNull())
                .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => new Random().Next(50, 300)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Img)).ReverseMap();
        }
    }
}
