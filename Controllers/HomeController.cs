//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using ObligatorioProgram3.Models;
//using System.Diagnostics;
//using System.Security.Claims;

//namespace ObligatorioProgram3.Controllers
//{
//    [Authorize]//solo accede si estas autorizado --  no deja entrar ni cambiando el url
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            ViewBag.OcultarSidebar = true;
//            ClaimsPrincipal claimUser = HttpContext.User;
//            string nombreUsuario = "";
//            if(claimUser.Identity.IsAuthenticated)
//            {
//                nombreUsuario = claimUser.Claims.Where(c=>c.Type==ClaimTypes.Name)
//                    .Select(c=>c.Value).SingleOrDefault();
//            }
//            ViewData["nombreUsuario"] = nombreUsuario;
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }


//        public async Task<IActionResult> CerrarSesion()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            return RedirectToAction("IniciarSesion","Inicio");
//        }
//    }
//}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioProgram3.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ObligatorioProgram3.Controllers
{
<<<<<<< HEAD
    [Authorize] // Requiere autenticaci�n por defecto
=======
    [Authorize] // Solo accede si est�s autorizado
>>>>>>> main
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous] // Permite el acceso an�nimo solo a la acci�n Index
        public IActionResult Index()
        {
<<<<<<< HEAD
            ViewBag.OcultarSidebar = true;
            ClaimsPrincipal claimUser = HttpContext.User;
            string nombreUsuario = "";
            if (claimUser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }
            ViewData["nombreUsuario"] = nombreUsuario;
=======
            // No se necesita m�s c�digo para manejar el nombre de usuario
           var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            // Verifica los claims aqu� para asegurarte de que el usuario tiene el claim "Permisos" con los valores adecuados
>>>>>>> main
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("IniciarSesion", "Inicio");
        }
    }
}
