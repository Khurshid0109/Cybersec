using Cybersec.Api.Extentions;
using Cybersec.Service.Mappers;
using Cybersec.Data.DbContexts;
using Cybersec.Service.Helpers;
using Cybersec.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

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

// Serve static files from the SharedMedia folder
string sharedFolderPath = Path.Combine(AppContext.BaseDirectory, "..", "SharedMedia");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.GetFullPath(sharedFolderPath)),
    RequestPath = "/media"
});

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
