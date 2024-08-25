using Cybersec.Data.DbContexts;
using Cybersec.Data.IRepositories;
using Cybersec.Data.Repositories;
using Cybersec.Service.Interfaces.Articles;
using Cybersec.Service.Mappers;
using Cybersec.Service.Services.Articles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IArticleService,ArticleService>();
builder.Services.AddScoped<IArticleRepository,ArticleRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "media",
    pattern: "media/{action}/{fileName?}",
    defaults: new { controller = "Media", action = "GetImage" });

app.Run();