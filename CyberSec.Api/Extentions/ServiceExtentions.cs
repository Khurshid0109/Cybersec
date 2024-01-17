using Cybersec.Data.Repositories;
using Cybersec.Data.IRepositories;
using Cybersec.Service.Services.Auth;
using Cybersec.Service.Services.Users;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.Interfaces.News;
using Cybersec.Service.Interfaces.Users;
using Cybersec.Service.Services.NewsService;

namespace Cybersec.Api.Extentions;
public static class ServiceExtentions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IUserService,UserService>();

        services.AddScoped<INewsRepository,NewsRepository>();
        services.AddScoped<INewsService,NewsService>();

        services.AddScoped<IUserAuthentication, UserAuthentication>();

        services.AddScoped<IUserCodeRepository,UserCodeRepository>();
        services.AddScoped<IUserCodeService, UserCodeService>();

    }
}
