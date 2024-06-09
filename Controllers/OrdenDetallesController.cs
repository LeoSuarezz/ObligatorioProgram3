using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;

namespace ObligatorioProgram3.Controllers
{
    public class OrdenDetallesController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public OrdenDetallesController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

        // GET: OrdenDetalles
        public async Task<IActionResult> Index()
        {
            var obligatorioProgram3Context = _context.OrdenDetalles.Include(o => o.IdmenuNavigation).Include(o => o.IdordenNavigation);
            return View(await obligatorioProgram3Context.ToListAsync());
        }

        // GET: OrdenDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenDetalle = await _context.OrdenDetalles
                .Include(o => o.IdmenuNavigation)
                .Include(o => o.IdordenNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordenDetalle == null)
            {
                return NotFound();
            }

            return View(ordenDetalle);
        }

        // GET: OrdenDetalles/Create
        public IActionResult Create()
        {
            ViewData["Idmenu"] = new SelectList(_context.Menus, "Id", "Id");
            ViewData["Idorden"] = new SelectList(_context.Ordenes, "Id", "Id");
            return View();
        }

        // POST: OrdenDetalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cantidad,Idorden,Idmenu")] OrdenDetalle ordenDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordenDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idmenu"] = new SelectList(_context.Menus, "Id", "Id", ordenDetalle.Idmenu);
            ViewData["Idorden"] = new SelectList(_context.Ordenes, "Id", "Id", ordenDetalle.Idorden);
            return View(ordenDetalle);
        }

        // GET: OrdenDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenDetalle = await _context.OrdenDetalles.FindAsync(id);
            if (ordenDetalle == null)
            {
                return NotFound();
            }
            ViewData["Idmenu"] = new SelectList(_context.Menus, "Id", "Id", ordenDetalle.Idmenu);
            ViewData["Idorden"] = new SelectList(_context.Ordenes, "Id", "Id", ordenDetalle.Idorden);
            return View(ordenDetalle);
        }

        // POST: OrdenDetalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cantidad,Idorden,Idmenu")] OrdenDetalle ordenDetalle)
        {
            if (id != ordenDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordenDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenDetalleExists(ordenDetalle.Id))
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
            ViewData["Idmenu"] = new SelectList(_context.Menus, "Id", "Id", ordenDetalle.Idmenu);
            ViewData["Idorden"] = new SelectList(_context.Ordenes, "Id", "Id", ordenDetalle.Idorden);
            return View(ordenDetalle);
        }

        // GET: OrdenDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenDetalle = await _context.OrdenDetalles
                .Include(o => o.IdmenuNavigation)
                .Include(o => o.IdordenNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordenDetalle == null)
            {
                return NotFound();
            }

            return View(ordenDetalle);
        }

        // POST: OrdenDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordenDetalle = await _context.OrdenDetalles.FindAsync(id);
            if (ordenDetalle != null)
            {
                _context.OrdenDetalles.Remove(ordenDetalle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenDetalleExists(int id)
        {
            return _context.OrdenDetalles.Any(e => e.Id == id);
        }
    }
}
