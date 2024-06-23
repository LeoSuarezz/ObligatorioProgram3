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
            var rolesConPermisos = await _context.Rol
                .Include(r => r.RolPermisos)
                    .ThenInclude(rp => rp.Permiso)
                .ToListAsync();

            return View(rolesConPermisos);
        }



        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Rol
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
            var permisos = await _context.Permisos.ToListAsync();
            ViewBag.Permisos = permisos;

            return PartialView("CreatePartialView", rol); // Devolver la vista parcial con un nuevo objeto Rol
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Rol
                .Include(r => r.IdPermisos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rol == null)
            {
                return NotFound();
            }

            // Obtener todos los permisos disponibles
            var permisos = _context.Permisos.ToList();
            ViewBag.Permisos = permisos;

            return View(rol);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreRol")] Rol rol, int[] permisosSeleccionados)
        {
            if (id != rol.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var rolActualizado = await _context.Rol
                        .Include(r => r.IdPermisos)
                        .FirstOrDefaultAsync(r => r.Id == id);

                    if (rolActualizado == null)
                    {
                        return NotFound();
                    }

                    // Actualizar los datos básicos del rol
                    rolActualizado.NombreRol = rol.NombreRol;

                    // Limpiar los permisos existentes
                    rolActualizado.IdPermisos.Clear();

                    // Agregar los nuevos permisos seleccionados
                    if (permisosSeleccionados != null)
                    {
                        foreach (var permisoId in permisosSeleccionados)
                        {
                            var permiso = _context.Permisos.Find(permisoId);
                            if (permiso != null)
                            {
                                rolActualizado.IdPermisos.Add(permiso);
                            }
                        }
                    }

                    _context.Update(rolActualizado);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
            }

            // Si el modelo no es válido, recargar la vista con los datos actuales
            var permisos = _context.Permisos.ToList();
            ViewBag.Permisos = permisos;

            return View(rol);
        }






        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Rol
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
            var rol = await _context.Rol.FindAsync(id);
            _context.Rol.Remove(rol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
            return _context.Rol.Any(e => e.Id == id);
        }

        public IActionResult CreatePartial()
        {
            var permisos = _context.Permisos.ToList();  // Obtener los permisos desde la base de datos
            ViewBag.Permisos = permisos;

            return PartialView("CreatePartialView");
        }
    }
}
