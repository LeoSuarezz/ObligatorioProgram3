//using Microsoft.AspNetCore.Mvc;
//using ObligatorioProgram3.Models;
//using ObligatorioProgram3.Recursos;
//using ObligatorioProgram3.Servicios.Contrato;

//using System.Security.Claims;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;


//namespace ObligatorioProgram3.Controllers
//{

//    public class InicioController : Controller
//    {

//        private readonly IUsuarioServicio _usarioServicio;

//        public InicioController(IUsuarioServicio usarioServicio)
//        {
//            _usarioServicio = usarioServicio;
//        }

//        public IActionResult IniciarSesion()
//        {
//            return View();
//        }

//        [HttpPost]
//        [HttpPost]
//        public async Task<IActionResult> IniciarSesion(string email, string contrasena)
//        {
//            Usuario usuario_encontrado = await _usarioServicio.GetUsuario(email, contrasena);

//            if (usuario_encontrado == null)
//            {
//                ViewData["Mensaje"] = "No se encontraron coincidencias";
//                return View();
//            }

//            List<Claim> claims = new List<Claim>()
//            {
//                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
//                new Claim("IdRol", usuario_encontrado.Idrol.ToString() ?? "0") 
//                // manejar el caso de rol nulo
//                // creamos claim para obtener el idrol del objeto user que esta logeado
//            };

//            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//            AuthenticationProperties properties = new AuthenticationProperties()
//            {
//                AllowRefresh = true,
//            };

//            await HttpContext.SignInAsync(
//                CookieAuthenticationDefaults.AuthenticationScheme,
//                new ClaimsPrincipal(claimsIdentity),
//                properties
//            );

//            return RedirectToAction("Index", "Home");
//        }

//    }
//}

using Microsoft.AspNetCore.Mvc;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.Recursos;
using ObligatorioProgram3.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace ObligatorioProgram3.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioServicio _usarioServicio;

        public InicioController(IUsuarioServicio usarioServicio)
        {
            _usarioServicio = usarioServicio;
        }

        [HttpGet]
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

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
                new Claim("IdRol", usuario_encontrado.Idrol.ToString() ?? "0")
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

            return RedirectToAction("Index", "Reservas");
        }
    }
}


