using Microsoft.AspNetCore.Mvc;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.Recursos;
using ObligatorioProgram3.Servicios.Contrato;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;


namespace ObligatorioProgram3.Controllers
{

    public class InicioController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;

        public InicioController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string email, string contrasena)
        {
            Usuario usuario_encontrado = await _usuarioServicio.GetUsuario(email, contrasena);

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
                new Claim("IdRol", usuario_encontrado.Idrol.ToString() ?? "0") 
                // manejar el caso de rol nulo
                // creamos claim para obtener el idrol del objeto user que esta logeado
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            return RedirectToAction("Index", "Home");
        }

    }
}
