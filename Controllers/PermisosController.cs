﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioProgram3.Controllers
{
    [Authorize(Policy = "VerPermisosPermiso")]
    public class PermisosController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public PermisosController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

        // GET: Permisos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Permisos.ToListAsync());
        }

        // GET: Permisos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // GET: Permisos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Permisos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(permiso);
        }

        public IActionResult CreatePartial()
        {
            var roles = _context.Rol
            .Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),    // Asignar el ID del rol como Value
                Text = r.NombreRol           // Asignar el nombre del rol como Text
            })
            .ToList();

            ViewBag.Idrol = new SelectList(roles, "Value", "Text");

            return PartialView("CreatePartialView");
        }

        // GET: Permisos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return View(permiso);
        }

        // POST: Permisos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Permiso permiso)
        {
            if (id != permiso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermisoExists(permiso.Id))
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
            return View(permiso);
        }

        // GET: Permisos/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = _context.Permisos
                                  .Include(p => p.RolPermisos)
                                  .FirstOrDefault(p => p.Id == id);

            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // POST: Permisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var permiso = _context.Permisos
                                  .Include(p => p.RolPermisos)
                                  .FirstOrDefault(p => p.Id == id);

            if (permiso == null)
            {
                return NotFound();
            }

            try
            {
                var rolPermisos = _context.Set<RolPermiso>()
                                                      .Where(rp => rp.IdPermisos == id)
                                                      .ToList();

                // Eliminar las asociaciones RolPermiso
                _context.Set<RolPermiso>().RemoveRange(rolPermisos);

                // Eliminar el permiso
                _context.Permisos.Remove(permiso);

                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Manejar excepciones según tus necesidades
                ModelState.AddModelError("", "Error al eliminar el permiso. Detalles: " + ex.Message);
                return View(permiso);
            }
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.Id == id);
        }
    }
}
