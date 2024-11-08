using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;



var builder = WebApplication.CreateBuilder(args);

// Servisleri ekleme
builder.Services.AddRazorPages();


builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllersWithViews();


//Global Authorization Policy (Tüm kullanýcýlarýn kimlik doðrulamasý gerekecek)
var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
builder.Services.AddMvc(config => config.Filters.Add(new AuthorizeFilter(policy))); // Politika ekleniyor

// Kimlik doðrulama servisi ekleme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Web_SignIn"; // Giriþ sayfasý
    });

// DbContext'inizi yapýlandýrma
builder.Services.AddDbContext<FlightReservation_Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Baðýmlýlýklarýn eklenmesi
builder.Services.AddScoped<IUserDal, EfUserDal>(); // IUserDal ve EfUserDal'ý DI konteynerine ekliyoruz
builder.Services.AddScoped<UserMenager>(); // UserMenager'ý da ekliyoruz

builder.Services.AddScoped<IEventDal, EfEventDal>();
builder.Services.AddScoped<EventMenager>();
// isim taþýma iþlemi
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman aþýmý süresini 30 dakika yapýyoruz
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS etkinleþtiriliyor
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Kimlik doðrulama middleware'i ekleniyor
app.UseAuthorization();

app.UseSession();// sessionun kullanýlmasý

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Web_SignIn}/{Id?}");

app.MapControllerRoute(
    name: "User",
    pattern: "Home/UserUI/{RoleUrl}",
    defaults: new { controller = "Home", action = "UserUI" });

app.MapControllerRoute(
    name: "Admin",
    pattern: "Home/Index/{RoleUrl}",
    defaults: new { controller = "Home", action = "Index" });

app.Run();

