using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.ViewModels;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;

namespace ObligatorioProgram3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CurrencyService _currencyService;

        public HomeController(ILogger<HomeController> logger, ObligatorioProgram3Context context, IHttpClientFactory httpClientFactory,CurrencyService currencyService)
        {
            _logger = logger;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _currencyService = currencyService;
        }



        public async Task<IActionResult> Index()
        {
            var menuItems = await _context.Menus.Where(m => m.Categoria != "Bebidas").ToListAsync();
            var reseñas = await _context.Reseñas.ToListAsync();

            //clima
            // Obtener el clima actual
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://api.openweathermap.org/data/2.5/weather?q=Maldonado&appid=44dfeaa3f72f2d3f648a622ca963c41d&units=metric");
            var climaData = JObject.Parse(response);

            var temperatura = climaData["main"]["temp"].ToList();
            var descripcion = climaData["weather"][0]["description"].ToList();


            ViewBag.Temperatura = temperatura;
            ViewBag.Description = descripcion;

            //

            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            // Verifica los claims aqu? para asegurarte de que el usuario tiene el claim "Permisos" con los valores adecuados

            var categorias = new List<string> { "Entradas", "Principal", "Postres" };

            ViewBag.Reseñas = reseñas;

            ViewBag.Categorias = categorias;
            ViewBag.MenuItems = menuItems;

            return View(menuItems);

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
