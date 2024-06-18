using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.Servicios.Contrato;
using ObligatorioProgram3.Servicios.Implementacion;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//ANGEL
builder.Services.AddDbContext<ObligatorioProgram3Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});

builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.AccessDeniedPath = "/Home/Index"; // ruta para acceso denegado

    });

//options.UseSqlServer("Data Source=ANGELMACHADO;Initial Catalog=ObligatorioProgram3;Integrated Security=true; TrustServerCertificate=True"));

//FRAN
//options.UseSqlServer("Data Source=DESKTOP-LTBG5HI;Initial Catalog=ObligatorioProgram3;Integrated Security=true; TrustServerCertificate=True"));

//LEO
//options.UseSqlServer("Data Source=LAPTOP-83A9Q1R9;Initial Catalog=ObligatorioProgram3;Integrated Security=true; TrustServerCertificate=True"));

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
        new ResponseCacheAttribute
        {
            NoStore = true,
            Location=ResponseCacheLocation.None,
        }
        );
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("IdRol", "1"));
    // añadimos un servicio de autorizacion
    // creamos el filtro adminOnly y llamamos a la claim diciendole q queremos el idrol 1 
    // porque ya sabemos que 1 es admin
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
