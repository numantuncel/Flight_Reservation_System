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


//Global Authorization Policy (T�m kullan�c�lar�n kimlik do�rulamas� gerekecek)
var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
builder.Services.AddMvc(config => config.Filters.Add(new AuthorizeFilter(policy))); // Politika ekleniyor

// Kimlik do�rulama servisi ekleme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Web_SignIn"; // Giri� sayfas�
    });

// DbContext'inizi yap�land�rma
builder.Services.AddDbContext<FlightReservation_Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Ba��ml�l�klar�n eklenmesi
builder.Services.AddScoped<IUserDal, EfUserDal>(); // IUserDal ve EfUserDal'� DI konteynerine ekliyoruz
builder.Services.AddScoped<UserMenager>(); // UserMenager'� da ekliyoruz

builder.Services.AddScoped<IEventDal, EfEventDal>();
builder.Services.AddScoped<EventMenager>();
// isim ta��ma i�lemi
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman a��m� s�resini 30 dakika yap�yoruz
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS etkinle�tiriliyor
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Kimlik do�rulama middleware'i ekleniyor
app.UseAuthorization();

app.UseSession();// sessionun kullan�lmas�

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

