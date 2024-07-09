using Microsoft.AspNetCore.Mvc;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.Recursos;
using ObligatorioProgram3.Servicios.Contrato;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace ObligatorioProgram3.Controllers
{

    public class InicioController : Controller
    {

        private readonly IUsuarioServicio _usarioServicio;

        public InicioController(IUsuarioServicio usarioServicio)
        {
            _usarioServicio = usarioServicio;
        }

        public IActionResult IniciarSesion()
        {
            return View();


        }


        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string email, string contrasena)
        {
            Usuario usuario_encontrado = await _usarioServicio.GetUsuario(email, contrasena);

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            int idRol = usuario_encontrado.Idrol ?? 0; // Obtener el ID del rol del usuario

            var permisosUsuario = await _usarioServicio.ObtenerPermisosPorRol(idRol); // obtiene los permisos del usuario logeado mediante un metodo

            // Crear las claims necesarias para la autenticación
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuario_encontrado.Nombre), // claim con el nombre
                    new Claim("IdRol", idRol.ToString()), // claim con el rol
                    new Claim("Permisos", string.Join(",", permisosUsuario))//claim q contiene permisos
                };

            // Crear identidad de claims
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Configurar propiedades de autenticación
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            // Realizar la autenticación
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                );

            var rutaPermisos = "";
            var accion = "";
            foreach (var p in permisosUsuario)
            {
                switch (p)
                {
                    case "ver usuarios":
                        rutaPermisos = "Usuarios";
                        accion = "Index";
                        break;
                    case "ver pagos":
                        accion = "Reportes";
                        rutaPermisos = "Pagos";
                        break;
                    case "ver menus":
                        accion = "Index";
                        rutaPermisos = "Menus";
                        break;
                    case "ver reservas":
                        accion = "Index";
                        rutaPermisos = "Reservas";
                        break;
                    case "ver restaurantes":
                        accion = "Index";
                        rutaPermisos = "Restaurantes";
                        break;
                    case "ver mesas":
                        accion = "Index";
                        rutaPermisos = "Mesas";
                        break;
                    case "ver clientes":
                        accion = "Index";
                        rutaPermisos = "Clientes";
                        break;
                    case "ver roles":
                        accion = "Index";
                        rutaPermisos = "Roles";
                        break;
                    case "ver ordenes":
                        accion = "Reportes";
                        rutaPermisos = "Ordenes";
                        break;
                    case "ver permisos":
                        accion = "Index";
                        rutaPermisos = "Permisos";
                        break;
                }

            }
            if(permisosUsuario.Contains("ver ordenes"))
            {
                accion= "Index";
                rutaPermisos = "Ordenes";
            }
            return RedirectToAction(accion, rutaPermisos);

        }



    }
}
