using Cybersec.Data.Repositories;
using Cybersec.Data.IRepositories;
using Cybersec.Service.Services.Auth;
using Cybersec.Service.Services.Users;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.Interfaces.Users;
using Cybersec.Service.Services.Articles;
using Cybersec.Service.Interfaces.Articles;

namespace Cybersec.Api.Extentions;
public static class ServiceExtentions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IUserService,UserService>();

        services.AddScoped<IArticleRepository,ArticleRepository>();
        services.AddScoped<IArticleService, ArticleService>();

        services.AddScoped<IUserAuthentication, UserAuthentication>();

        services.AddScoped<IUserCodeRepository,UserCodeRepository>();
        services.AddScoped<IUserCodeService, UserCodeService>();

    }
}
