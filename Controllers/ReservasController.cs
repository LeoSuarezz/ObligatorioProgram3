using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;

namespace ObligatorioProgram3.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public ReservasController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var obligatorioProgram3Context = _context.Reservas.Include(r => r.IdclienteNavigation).Include(r => r.IdmesaNavigation);
            return View(await obligatorioProgram3Context.ToListAsync());
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.IdclienteNavigation)
                .Include(r => r.IdmesaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["Idcliente"] = new SelectList(_context.Clientes, "Id", "Id");
            ViewData["Idmesa"] = new SelectList(_context.Mesas, "Id", "Id");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idcliente,Idmesa,FechaReserva,Estado")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var mesas = _context.Mesas.Select(m => m.Id).ToList();
            ViewBag.Idmesa = new SelectList(mesas);

            var clientes = _context.Clientes.Select(c => new { c.Id, c.Nombre }).ToList();
            ViewBag.IdCliente = new SelectList(clientes, "Id", "Nombre");

            return PartialView("CreatePartialView", reserva);
        }

        //crea la vista del modal y le carga los datos necesarios
        public IActionResult CreatePartial()
        {
            var clientes = _context.Clientes.Select(c => new { c.Id, c.Nombre }).ToList();
            ViewBag.IdCliente = new SelectList(clientes, "Id", "Nombre");

            var restaurantes = _context.Restaurantes.Select(r=> new { r.Id, r.Nombre }).ToList();
            ViewBag.Idrestaurante = new SelectList(restaurantes, "Id", "Nombre");

            var mesas = _context.Mesas.Select(m => m.NumeroMesa).ToList();
            ViewBag.Idmesa = new SelectList(mesas);

            return PartialView("CreatePartialView");
        }



      


        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["Idcliente"] = new SelectList(_context.Clientes, "Id", "Id", reserva.Idcliente);
            ViewData["Idmesa"] = new SelectList(_context.Mesas, "Id", "Id", reserva.Idmesa);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idcliente,Idmesa,FechaReserva,Estado")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            ViewData["Idcliente"] = new SelectList(_context.Clientes, "Id", "Id", reserva.Idcliente);
            ViewData["Idmesa"] = new SelectList(_context.Mesas, "Id", "Id", reserva.Idmesa);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.IdclienteNavigation)
                .Include(r => r.IdmesaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }

       


    }
}
