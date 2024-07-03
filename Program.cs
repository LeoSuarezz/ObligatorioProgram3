using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.Servicios.Contrato;
using ObligatorioProgram3.Servicios.Implementacion;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ObligatorioProgram3Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});


builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
        new ResponseCacheAttribute
        {
            NoStore = true,
            Location = ResponseCacheLocation.None,
        }
        );
});

// AUTENTICACION DE USUARIO LOGEADO
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.AccessDeniedPath = "/Home/Index"; // ruta para acceso denegado

    });
// autenticacion de permisos
// se crea una politica para cada permiso
// evalua si la claim de Permisos q tiene el permiso ver xxxxx y si la tiene esta autorizado
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("VerUsuariosPermiso", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var permisosClaim = context.User.FindFirst(c => c.Type == "Permisos")?.Value; // obtiene el valor del claim Permisos del usuario.
                if (permisosClaim != null)
                {
                    var permisosUsuario = permisosClaim.Split(',');
                    return permisosUsuario.Any(p => p.Trim() == "ver usuarios"); //verifica si al menos uno de los permisos en la lista coincide con ver usuarios
                }
                return false;
            });
        });
        options.AddPolicy("VerReservasPermiso", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var permisosClaim = context.User.FindFirst(c => c.Type == "Permisos")?.Value;
                if (permisosClaim != null)
                {
                    var permisosUsuario = permisosClaim.Split(',');
                    return permisosUsuario.Any(p => p.Trim() == "ver reservas");
                }
                return false;
            });
        });
        options.AddPolicy("VerRestaurantesPermiso", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var permisosClaim = context.User.FindFirst(c => c.Type == "Permisos")?.Value; 
                if (permisosClaim != null)
                {
                    var permisosUsuario = permisosClaim.Split(',');
                    return permisosUsuario.Any(p => p.Trim() == "ver restaurantes"); 
                }
                return false;
            });
        });
        options.AddPolicy("VerMesasPermiso", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var permisosClaim = context.User.FindFirst(c => c.Type == "Permisos")?.Value; 
                if (permisosClaim != null)
                {
                    var permisosUsuario = permisosClaim.Split(',');
                    return permisosUsuario.Any(p => p.Trim() == "ver mesas"); 
                }
                return false;
            });
        });
        options.AddPolicy("VerClientesPermiso", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var permisosClaim = context.User.FindFirst(c => c.Type == "Permisos")?.Value;
                if (permisosClaim != null)
                {
                    var permisosUsuario = permisosClaim.Split(',');
                    return permisosUsuario.Any(p => p.Trim() == "ver clientes"); 
                }
                return false;
            });
        });
        options.AddPolicy("VerRolesPermiso", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var permisosClaim = context.User.FindFirst(c => c.Type == "Permisos")?.Value; 
                if (permisosClaim != null)
                {
                    var permisosUsuario = permisosClaim.Split(',');
                    return permisosUsuario.Any(p => p.Trim() == "ver roles");
                }
                return false;
            });
        });
        options.AddPolicy("VerPermisosPermiso", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var permisosClaim = context.User.FindFirst(c => c.Type == "Permisos")?.Value; 
                if (permisosClaim != null)
                {
                    var permisosUsuario = permisosClaim.Split(',');
                    return permisosUsuario.Any(p => p.Trim() == "ver permisos");
                }
                return false;
            });
        });

        options.AddPolicy("VerOrdenesPermiso", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var permisosClaim = context.User.FindFirst(c => c.Type == "Permisos")?.Value;
                if (permisosClaim != null)
                {
                    var permisosUsuario = permisosClaim.Split(',');
                    return permisosUsuario.Any(p => p.Trim() == "ver ordenes");
                }
                return false;
            });
        });

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
