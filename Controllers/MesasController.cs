using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;

namespace ObligatorioProgram3.Controllers
{
    [Authorize(Policy = "VerMesasPermiso")]
    public class MesasController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public MesasController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

        // GET: Mesas
        public async Task<IActionResult> Index(int? idRestaurante)
        {
            var mesas = _context.Mesas.Include(m => m.IdrestauranteNavigation).AsQueryable();

            if (idRestaurante.HasValue)
            {
                mesas = mesas.Where(m => m.Idrestaurante == idRestaurante.Value);
            }

            mesas = mesas.OrderBy(m => m.IdrestauranteNavigation.Id)
                         .ThenBy(m => m.NumeroMesa);


            var restaurantes = await _context.Restaurantes.ToListAsync();
            ViewBag.Restaurantes = restaurantes;

            return View(mesas);
        }

        // GET: Mesas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas
                .Include(m => m.IdrestauranteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesa == null)
            {
                return NotFound();
            }

            return View(mesa);
        }

        // GET: Mesas/Create
        public IActionResult Create()
        {
            ViewBag.Idrestaurante = new SelectList(_context.Restaurantes, "Id", "Nombre");
            return View();
        }

        // POST: Mesas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroMesa,Capacidad,Estado,Idrestaurante")] Mesa mesa)
        {
            if (ModelState.IsValid)
            {
                //Controla si ya existe mesa con el mismo numero de mesa e ID resto
                var existeMesa = await _context.Mesas
                    .AnyAsync(m => m.NumeroMesa == mesa.NumeroMesa && m.Idrestaurante == mesa.Idrestaurante);
                if (existeMesa)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe una mesa con este número en el restaurante seleccionado.");
                }
                else
                {
                    _context.Add(mesa);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.IdRestaurante = new SelectList(_context.Restaurantes, "Id", "Nombre", mesa.Idrestaurante);

            return PartialView("CreatePartialView", mesa);

        }

        // GET: Mesas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }
            ViewBag.Idrestaurante = new SelectList(_context.Restaurantes, "Id", "Nombre", mesa.Idrestaurante);
            return View(mesa);
        }

        // POST: Mesas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroMesa,Capacidad,Estado,Idrestaurante")] Mesa mesa)
        {
            if (id != mesa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //verifica si ya existe una mesa con el mismo numero y restaurant a excepción de la actual
                var existeMesa = await _context.Mesas.
                    AnyAsync(m => m.NumeroMesa == mesa.NumeroMesa && m.Idrestaurante == mesa.Idrestaurante && m.Id != mesa.Id);
                if (existeMesa)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe una mesa con este número en el restaurante seleccionado.");
                }
                else
                {
                    try
                    {
                        _context.Update(mesa);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!MesaExists(mesa.Id))
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

            }
            ViewBag.Idrestaurante = new SelectList(_context.Restaurantes, "Id", "Nombre", mesa.Idrestaurante);
            return View(mesa);

        }

        // GET: Mesas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas
                .Include(m => m.IdrestauranteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesa == null)
            {
                return NotFound();
            }

            return View(mesa);
        }

        // POST: Mesas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa != null)
            {
                _context.Mesas.Remove(mesa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MesaExists(int id)
        {
            return _context.Mesas.Any(e => e.Id == id);
        }
        public IActionResult CreatePartial()
        {
            var restaurantes = _context.Restaurantes.Select(r => new { r.Id, r.Nombre }).ToList();
            ViewBag.IdRestaurante = new SelectList(restaurantes, "Id", "Nombre");

            return PartialView("CreatePartialView", new Mesa());
        }


    }
}
