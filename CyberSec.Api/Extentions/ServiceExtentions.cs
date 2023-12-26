using Cybersec.Data.IRepositories;
using Cybersec.Data.Repositories;
using Cybersec.Service.Interfaces.News;
using Cybersec.Service.Interfaces.Users;
using Cybersec.Service.Services.NewsService;
using Cybersec.Service.Services.Users;

namespace Cybersec.Api.Extentions;
public static class ServiceExtentions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IUserService,UserService>();

        services.AddScoped<INewsRepository,NewsRepository>();
        services.AddScoped<INewsService,NewsService>();

    }
}
