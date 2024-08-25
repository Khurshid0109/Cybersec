using Cybersec.Api.Extentions;
using Cybersec.Api.Middlewares;
using Cybersec.Data.DbContexts;
using Cybersec.Service.Mappers;
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ErrorHandler/GlobalError");
   
    app.UseHsts();
}

app.Use(async (ctx, next) =>
{
    await next();

    if(ctx.Response.StatusCode == 404 )
    {
        ctx.Request.Path = "/ErrorHandler/GlobalError?statusCode=" + ctx.Response.StatusCode;
        await next();
    }

});

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=Index}/{id?}");

app.Run();
