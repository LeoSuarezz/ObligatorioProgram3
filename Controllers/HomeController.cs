using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace ObligatorioProgram3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ObligatorioProgram3Context context)
        {
            _logger = logger;
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            var menuItems = await _context.Menus.ToListAsync();
            // No se necesita más código para manejar el nombre de usuario
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            // Verifica los claims aquí para asegurarte de que el usuario tiene el claim "Permisos" con los valores adecuados
            var categorias = new List<string> { "Entradas","Principal", "Postres" };

            ViewBag.Categorias = categorias;
            ViewBag.MenuItems = menuItems;
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
            return RedirectToAction("Index", "Home");
        }
    }
}
