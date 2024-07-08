using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ObligatorioProgram3.Models;
using System.Net.Http;

namespace ObligatorioProgram3.Controllers
{
    [Authorize]
    public class ClimasController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ObligatorioProgram3Context _context;

        public ClimasController(IHttpClientFactory httpClientFactory, ObligatorioProgram3Context context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerClima(string ciudad = "Grytviken")
        {
            var apiKey = "44dfeaa3f72f2d3f648a622ca963c41d";
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={apiKey}&units=metric";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync(url);

            var clima = JObject.Parse(response);
            var temperatura = clima["main"]["temp"].ToObject<float>();
            var descripcion = clima["weather"][0]["description"].ToString();

            var climaRegistro = new Clima
            {
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Temperatura = temperatura,
                DescripcionClima = descripcion
            };

            _context.Climas.Add(climaRegistro);
            await _context.SaveChangesAsync();

            return Json(new { Id = climaRegistro.Id, temperatura, descripcion });
        }
    

        // GET: Climas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clima = await _context.Climas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clima == null)
            {
                return NotFound();
            }

            return View(clima);
        }

        // GET: Climas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Climas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Temperatura,DescripcionClima")] Clima clima)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clima);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clima);
        }

        // GET: Climas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clima = await _context.Climas.FindAsync(id);
            if (clima == null)
            {
                return NotFound();
            }
            return View(clima);
        }

        // POST: Climas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Temperatura,DescripcionClima")] Clima clima)
        {
            if (id != clima.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clima);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClimaExists(clima.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clima);
        }

        // GET: Climas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clima = await _context.Climas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clima == null)
            {
                return NotFound();
            }

            return View(clima);
        }

        // POST: Climas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clima = await _context.Climas.FindAsync(id);
            if (clima != null)
            {
                _context.Climas.Remove(clima);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClimaExists(int id)
        {
            return _context.Climas.Any(e => e.Id == id);
        }
    }
}
