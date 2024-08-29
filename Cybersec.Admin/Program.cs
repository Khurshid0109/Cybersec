using Cybersec.Admin.Extentions;
using Cybersec.Data.DbContexts;
using Cybersec.Service.Mappers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomServices(); 
// Register DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/Access/Login"; // Redirect to login page if not authenticated
           options.AccessDeniedPath = "/Access/AccessDenied"; // Redirect if user doesn't have permission
           options.Cookie.Name = "YourAppAuthCookie";
           options.ExpireTimeSpan = TimeSpan.FromHours(0.5);
           options.SlidingExpiration = true;
       });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "media",
    pattern: "media/{action}/{fileName?}",
    defaults: new { controller = "Media", action = "GetImage" });

app.Run();