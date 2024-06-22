using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioProgram3.Controllers
{
    public class RolesController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public RolesController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            var permisos = _context.Permisos.ToList(); // Suponiendo que tienes un contexto de base de datos (_context) configurado correctamente
            ViewBag.Permisos = permisos;

            return View(await _context.Rols.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Roles/Create
        public IActionResult Create()
        {
            // Obtener todos los permisos disponibles desde alguna fuente (base de datos, servicio, etc.)
            var permisos = _context.Permisos.ToList();
            ViewBag.Permisos = new SelectList(permisos, "Id", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreRol")] Rol rol, int[] permisosSeleccionados)
        {
            if (ModelState.IsValid)
            {
                // Añadir el rol a la base de datos
                _context.Add(rol);
                await _context.SaveChangesAsync();

                // Asignar permisos al rol
                if (permisosSeleccionados != null)
                {
                    foreach (var permisoId in permisosSeleccionados)
                    {
                        var rolPermiso = new RolPermiso
                        {
                            IdRol = rol.Id,
                            IdPermisos = permisoId
                        };
                        _context.Add(rolPermiso);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Obtener la lista de permisos
            var permisos = _context.Permisos.ToList();
            ViewBag.Permisos = permisos;

            return PartialView("CreatePartialView", new Rol()); // Devolver la vista parcial con un nuevo objeto Rol
        }

            // GET: Roles/Edit/5
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

        // POST: Roles/Edit/5
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

        // GET: Roles/Delete/5
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

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rol = await _context.Rols.FindAsync(id);
            _context.Rols.Remove(rol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
            return _context.Rols.Any(e => e.Id == id);
        }

        public IActionResult CreatePartial()
        {
            var permisos = _context.Permisos.ToList();  // Obtener los permisos desde la base de datos
            ViewBag.Permisos = permisos;

            return PartialView("CreatePartialView");
        }
    }
}
