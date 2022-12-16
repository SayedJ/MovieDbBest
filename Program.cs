using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using webapp_cloudrun;
using webapp_cloudrun.Auth;
using webapp_cloudrun.Context;
using webapp_cloudrun.Models;
using webapp_cloudrun.Repositories;
using webapp_cloudrun.Repositories.Impl;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MovieDbContext>();

////options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.themoviedb.org") });
builder.Services.AddTransient<IMoviesRepo, MovieRepoImpl>();
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.Configure<List<User>>(builder.Configuration.GetSection("Users"));

//builder.Services.AddAuthentication("Basic")
//        .AddScheme<BasicAuthenticationOptions, CustomAuthenticationHandler>("Basic", null);
AuthorizationPolicies.AddPolicies(builder.Services);
builder.Services.AddAuthorizationCore(config =>
{
config.AddPolicy("MustBeAdmin", policy => policy.RequireClaim("Roles", "Admin"));
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
{
options.LoginPath = new PathString("/Home/Login");
options.AccessDeniedPath = new PathString("/Registeration");
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Home/Login";
    options.AccessDeniedPath = "/Home/Registeration";
    options.SlidingExpiration = true;
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
