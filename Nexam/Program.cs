using Microsoft.AspNetCore.Authentication.Cookies;
using Nexam.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews()
    // The site's JavaScript reads JSON properties in PascalCase (result.Remarks, result.Score), as MVC 5 produced.
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "Nexam.Auth";
    });
builder.Services.AddAuthorization();

builder.Services.AddNexam(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapNexamRoutes();

app.Run();
