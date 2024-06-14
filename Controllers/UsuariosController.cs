using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.Recursos;

namespace ObligatorioProgram3.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public UsuariosController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var obligatorioProgram3Context = _context.Usuarios.Include(u => u.IdrolNavigation);
            return View(await obligatorioProgram3Context.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdrolNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["Idrol"] = new SelectList(_context.Rols, "Id", "Id");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idrol,Nombre,Apellido,Email,Contraseña")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                //usuario.Contraseña = Utilidades.encriptarClave(usuario.Contraseña);
                //para q esto funcione habria q cambiar el largo q admite la contraseña en la bd

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            /*
            var roles = _context.Rols.Select(r => r.NombreRol).ToList();
            ViewBag.Idrol = new SelectList(roles); 
            */
            var roles = _context.Rols.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.NombreRol
            }).ToList();
            ViewBag.Idrol = new SelectList(roles, "Value", "Text", usuario.Idrol);
            return PartialView("_CreatePartialView", usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["Idrol"] = new SelectList(_context.Rols, "Id", "Id", usuario.Idrol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idrol,Nombre,Apellido,Email,Contraseña")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["Idrol"] = new SelectList(_context.Rols, "Id", "Id", usuario.Idrol);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdrolNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
        public IActionResult CreatePartial()
        {
            var roles = _context.Rols
            .Select(r => new SelectListItem{
                Value = r.Id.ToString(),    // Asignar el ID del rol como Value
                Text = r.NombreRol           // Asignar el nombre del rol como Text
             })
     .ToList();

            ViewBag.Idrol = new SelectList(roles, "Value", "Text");

            return PartialView("_CreatePartialView");
        }

    }
}
