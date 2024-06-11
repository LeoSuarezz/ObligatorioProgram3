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
    public class PagoesController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public PagoesController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

        // GET: Pagoes
        public async Task<IActionResult> Index()
        {
            var obligatorioProgram3Context = _context.Pagos.Include(p => p.IdclimaNavigation).Include(p => p.IdcotizacionNavigation).Include(p => p.IdreservaNavigation);
            return View(await obligatorioProgram3Context.ToListAsync());
        }

        // GET: Pagoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.IdclimaNavigation)
                .Include(p => p.IdcotizacionNavigation)
                .Include(p => p.IdreservaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagoes/Create
        public IActionResult Create()
        {
            ViewData["Idclima"] = new SelectList(_context.Climas, "Id", "Id");
            ViewData["Idcotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id");
            ViewData["Idreserva"] = new SelectList(_context.Reservas, "Id", "Id");
            return View();
        }

        // POST: Pagoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idreserva,Monto,FechaPago,Idcotizacion,MetodoPago,Idclima")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idclima"] = new SelectList(_context.Climas, "Id", "Id", pago.Idclima);
            ViewData["Idcotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id", pago.Idcotizacion);
            ViewData["Idreserva"] = new SelectList(_context.Reservas, "Id", "Id", pago.Idreserva);
            return View(pago);
        }

        // GET: Pagoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["Idclima"] = new SelectList(_context.Climas, "Id", "Id", pago.Idclima);
            ViewData["Idcotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id", pago.Idcotizacion);
            ViewData["Idreserva"] = new SelectList(_context.Reservas, "Id", "Id", pago.Idreserva);
            return View(pago);
        }

        // POST: Pagoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idreserva,Monto,FechaPago,Idcotizacion,MetodoPago,Idclima")] Pago pago)
        {
            if (id != pago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id))
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
            ViewData["Idclima"] = new SelectList(_context.Climas, "Id", "Id", pago.Idclima);
            ViewData["Idcotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id", pago.Idcotizacion);
            ViewData["Idreserva"] = new SelectList(_context.Reservas, "Id", "Id", pago.Idreserva);
            return View(pago);
        }

        // GET: Pagoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.IdclimaNavigation)
                .Include(p => p.IdcotizacionNavigation)
                .Include(p => p.IdreservaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }
    }
}
