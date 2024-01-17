using Cybersec.Api.Extentions;
using Cybersec.Service.Mappers;
using Cybersec.Data.DbContexts;
using Cybersec.Service.Helpers;
using Cybersec.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddCustomServices();


var app = builder.Build();

WebHostEnvironmentHelper.WebRootPath = Path.GetFullPath("wwwroot");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ErrorHandler/GlobalError");
   
    app.UseHsts();
}
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=Index}/{id?}");

app.Run();
