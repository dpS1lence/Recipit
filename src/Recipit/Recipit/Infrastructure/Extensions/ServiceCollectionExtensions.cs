namespace Recipit.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Services.Account;
    using Recipit.Services.Comments;
    using Recipit.Services.Followers;
    using Recipit.Services.ImageWebSearch;
    using Recipit.Services.Products;
    using Recipit.Services.Ratings;
    using Recipit.Services.Recipes;
    using Serilog;

    public static class ServiceCollectionExtensions
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Configuration.AddConfiguration(configBuilder.Build()).Build();
        }
        public static void AddDatabase(this WebApplicationBuilder builder)
        {
            var envConnection = builder.Configuration.GetValue<string>("CONNECTION_STRING");

            builder.Services.AddDbContext<RecipitDbContext>(
                options => options.UseSqlServer(!string.IsNullOrEmpty(envConnection) ? envConnection : builder.Configuration.GetConnectionString("DefaultConnection")));
        }
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<IFollowerService, FollowerService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<IExternalRecipeCreationService, ExternalRecipeCreationService>();
            builder.Services.AddScoped<ISearchService, GoogleImageSearchService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();
            builder.Services.AddSession();
        }
        public static void AddMvc(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            builder.Services.AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });
        }
        public static void AddCustomIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            });

            builder.Services.AddIdentity<Comment, IdentityRole>(cfg =>
            {
                cfg.Password.RequireUppercase = false;
                cfg.User.RequireUniqueEmail = true;
                cfg.SignIn.RequireConfirmedEmail = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<RecipitDbContext>()
                .AddDefaultTokenProviders();
        }
        public static Serilog.ILogger CreateSerilogLogger(IConfiguration config, string appName)
        {
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            return loggerConfig;
        }

        public static void UseMappedEndpointsWithLoginRedirect(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var endpoint = context.GetEndpoint();
                if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() != null)
                {
                    var user = context.User?.Identity;
                    if (user == null || !user.IsAuthenticated)
                    {
                        context.Response.Redirect("/login");
                        return;
                    }
                }

                await next(context);
            });
        }

        private static readonly string[] tags = ["IdentityDB"];

        public static void AddCustomHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy())
                    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!,
                        name: "RecipitDB-check",
                        tags: tags);
        }
    }
}
