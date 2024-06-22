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
    public class RolesController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public RolesController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

        // GET: Rols
        /*
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rols.ToListAsync());
        }
        */

        public async Task<IActionResult> Index()
        {
            var roles = await _context.Rols
                                      .Include(r => r.Permisos)
                                      .ToListAsync();
            return View(roles);
        }


        // GET: Rols/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Rols
                .Include(r => r.Permisos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // GET: Rols/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreRol")] Rol rol, int[] selectedPermisos)
        {
            if (ModelState.IsValid)
            {
                if (selectedPermisos != null)
                {
                    rol.Permisos = new List<Permiso>();
                    foreach (var permisoId in selectedPermisos)
                    {
                        var permiso = await _context.Permisos.FindAsync(permisoId);
                        if (permiso != null)
                        {
                            rol.Permisos.Add(permiso);
                        }
                    }
                }

                _context.Add(rol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var permisos = _context.Permisos.ToList();
            ViewBag.Permisos = new SelectList(_context.Rols, "Id", "Nombre",rol.Permisos);
            return View(rol);
        }
    
        public IActionResult CreatePartial()
        {
          
            var permisos = _context.Permisos.Select(p => new { p.Id, p.Nombre }).ToList();
            ViewBag.Permisos = new SelectList(permisos, "Id", "Nombre");

            return PartialView("CreatePartialView");
        }



        // GET: Rols/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Rols.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        // POST: Rols/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreRol")] Rol rol)
        {
            if (id != rol.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolExists(rol.Id))
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
            return View(rol);
        }

        // GET: Rols/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Rols
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // POST: Rols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rol = await _context.Rols.FindAsync(id);
            if (rol != null)
            {
                _context.Rols.Remove(rol);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
            return _context.Rols.Any(e => e.Id == id);
        }
    }
}
