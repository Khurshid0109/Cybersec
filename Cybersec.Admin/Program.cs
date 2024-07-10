using Cybersec.Data.DbContexts;
using Cybersec.Service.Mappers;
using Cybersec.Data.Repositories;
using Cybersec.Data.IRepositories;
using Microsoft.EntityFrameworkCore;
using Cybersec.Service.Services.Articles;
using Microsoft.Extensions.FileProviders;
using Cybersec.Service.Interfaces.Articles;

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

// Configure to serve SharedMedia directory
string sharedMediaPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\SharedMedia");
if (Directory.Exists(sharedMediaPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(sharedMediaPath),
        RequestPath = "/media"
    });
}

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
