using LoggerService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoManager.Data;
using TodoManager.Data.Services;
using TodoManager.Membership;
using TodoManager.Membership.Entities;
using TodoManager.Membership.Services;
using TodoManager.Notification.Entities;
using LogLevel = NLog.LogLevel;

namespace TodoManager.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy", builder =>
            {
                builder
                    .WithOrigins("http://localhost:8080") // let the client access
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
    {
        const string connectionStringName = "SqlConnection";

        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString(connectionStringName), b =>
            {
                b.MigrationsAssembly(typeof(Program).Assembly.FullName);
            })
        );

        services.AddDbContext<TodosDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString(connectionStringName), b =>
            {
                b.MigrationsAssembly(typeof(Program).Assembly.FullName);
            })
        );

        services.AddDbContext<NotificationContext>(options =>
            options.UseSqlServer(config.GetConnectionString(connectionStringName), b =>
            {
                b.MigrationsAssembly(typeof(Program).Assembly.FullName);
            })
        );
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<long>>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
            options.Password.RequiredUniqueChars = 1;
            // Default User settings.
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        });
    }

    public static void ConfigureCookies(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(config =>
        {
            config.LoginPath = "/signin";
            config.Cookie.SameSite = SameSiteMode.None; // allow cross-site cookie setup
            config.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = context =>
                {
                    // don't redirect to login but return 401 as unauthorize
                    context.HttpContext.Response.StatusCode = 401;
                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void ConfigureLogger(this IServiceCollection services)
    {
        var config = new NLog.Config.LoggingConfiguration();
        var targetFile = new NLog.Targets.FileTarget("logfile") { FileName = "logfile.txt" };

        // minimum loglevel is Info & max is Fatal
        config.AddRule(LogLevel.Info, LogLevel.Fatal, targetFile);

        // Apply config
        NLog.LogManager.Configuration = config; 
        
        // Add Logger in DI as singleton
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<TodosDbContext>();
        services.AddScoped<NotificationContext>();
    }

    public static void RegisterRepositoryManagers(this IServiceCollection services)
    {
        services.AddScoped<ITodoRepositoryManager, TodoRepositoryManager>();
        services.AddScoped<ITodoService, TodoService>();
    }
}
