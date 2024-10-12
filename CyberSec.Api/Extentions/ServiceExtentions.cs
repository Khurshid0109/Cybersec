using Cybersec.Data.IRepositories;
using Cybersec.Data.Repositories;
using Cybersec.Service.Interfaces.Articles;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.Interfaces.Users;
using Cybersec.Service.Services.Articles;
using Cybersec.Service.Services.Auth;
using Cybersec.Service.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cybersec.Api.Extentions;
public static class ServiceExtentions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IUserService,UserService>();

        services.AddScoped<IAdminRepository,AdminRepository>();
        services.AddScoped<IAdminService,AdminService>();

        services.AddScoped<IArticleRepository,ArticleRepository>();
        services.AddScoped<IArticleService, ArticleService>();

        services.AddScoped<ILikeRepository,LikeRepository>();
        services.AddScoped<ILikeService,LikeService>();

        services.AddScoped<ICommentRepository,CommentRepository>();
        services.AddScoped<ICommentService,CommentService>();

        services.AddScoped<IUserCodeRepository,UserCodeRepository>();
        services.AddScoped<IUserCodeService, UserCodeService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IExistEmail, EmailExist>();

        services.AddScoped<IJwtTokenService,JwtTokenService>();
        services.AddScoped<IResetPasswordService, ResetPasswordService>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
    public static void AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}
