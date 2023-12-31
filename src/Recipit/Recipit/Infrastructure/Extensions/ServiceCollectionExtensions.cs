﻿namespace Recipit.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.MailSending;
    using Recipit.Services.Followers;
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
            builder.Services.AddTransient<IMailSender, MailSender>();
            builder.Services.AddHttpContextAccessor();
        }
        public static void AddMvc(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            builder.Services.AddControllersWithViews();

            builder.Services.AddControllers();
        }
        public static void AddCustomIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/login";
            });
            builder.Services.AddIdentity<RecipitUser, IdentityRole>(cfg =>
            {
                cfg.Password.RequireUppercase = false;
                cfg.User.RequireUniqueEmail = true;
                cfg.SignIn.RequireConfirmedEmail = true;
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

        private static readonly string[] tags = ["IdentityDB"];

        public static void AddCustomHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy())
                    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!,
                        name: "IdentityDB-check",
                        tags: tags);
        }
        public static void AddEmailSending(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<EmailSendingSettings>(builder.Configuration.GetSection("EmailSending"));
            builder.Services.AddTransient<IMailSender, MailSender>();
        }
    }
}
