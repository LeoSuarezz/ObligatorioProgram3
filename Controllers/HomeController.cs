using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ObligatorioProgram3.Controllers
{
    [Authorize]//solo accede si estas autorizado --  no deja entrar ni cambiando el url
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ObligatorioProgram3Context _context;

        public HomeController(ILogger<HomeController> logger, ObligatorioProgram3Context context)
        {
            _logger = logger;
            _context = context;
        }

        //[AllowAnonymous]
        //public IActionResult Index()
        //{
        //    ClaimsPrincipal claimUser = HttpContext.User;
        //    string nombreUsuario = "";
        //    if(claimUser.Identity.IsAuthenticated)
        //    {
        //        nombreUsuario = claimUser.Claims.Where(c=>c.Type==ClaimTypes.Name)
        //            .Select(c=>c.Value).SingleOrDefault();
        //    }
        //    ViewData["nombreUsuario"] = nombreUsuario;
        //    return View();
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}


        //public async Task<IActionResult> CerrarSesion()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("IniciarSesion","Inicio");
        //}

        [AllowAnonymous]
        public IActionResult Index()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string nombreUsuario = "";
            if (claimUser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }
            ViewData["nombreUsuario"] = nombreUsuario;

            ViewData["HideSidebar"] = !User.Identity.IsAuthenticated;

            // Obtener los elementos del menú (si es necesario para alguna lógica futura)
            var menuItems = _context.Menus.ToList();

            // Devolver la vista con los elementos del menú
            return View(menuItems);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult LeaveReview(Reseña review)
        {
            if (ModelState.IsValid)
            {
                review.FechaReseña = DateOnly.FromDateTime(DateTime.Now);
                _context.Reseñas.Add(review);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("IniciarSesion", "Inicio");
        }




    }
}
