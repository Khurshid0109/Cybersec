using Cybersec.Data.IRepositories;
using Cybersec.Data.Repositories;
using Cybersec.Service.Interfaces.Articles;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.Interfaces.Users;
using Cybersec.Service.Services.Articles;
using Cybersec.Service.Services.Auth;
using Cybersec.Service.Services.Users;

namespace Cybersec.Admin.Extentions;
public static class ServiceExtentions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IAdminService, AdminService>();

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IArticleService, ArticleService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUserCodeRepository, UserCodeRepository>();
        services.AddScoped<IUserCodeService, UserCodeService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IExistEmail, EmailExist>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IResetPasswordService, ResetPasswordService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
