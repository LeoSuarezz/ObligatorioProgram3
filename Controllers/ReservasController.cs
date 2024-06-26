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
    [Authorize(Policy = "VerReservasPermiso")]
    public class ReservasController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public ReservasController(ObligatorioProgram3Context context)
        {
            _context = context;
        }
        public IActionResult Index(string filter, int? idRestaurante, DateOnly? fechaReserva)
        {
            var reservas = _context.Reservas
                                    .Include(r => r.IdmesaNavigation)
                                    .Include(r => r.IdclienteNavigation)
                                    .Include(r => r.IdmesaNavigation.IdrestauranteNavigation)
                                    .AsQueryable();

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly nextWeek = today.AddDays(7);
            DateOnly tomorrow = today.AddDays(1);

            if (fechaReserva.HasValue)
            {
                reservas = reservas.Where(r => r.FechaReserva == fechaReserva.Value);
            }
            else
            {

                switch (filter)
                {
                    case "hoy":
                        reservas = reservas.Where(r => r.FechaReserva == today);
                        break;
                    case "pasadas":
                        reservas = reservas.Where(r => r.FechaReserva < today);
                        break;
                    case "proximos7dias":
                        reservas = reservas.Where(r => r.FechaReserva >= tomorrow && r.FechaReserva <= nextWeek);
                        break;
                    default:
                        break;
                }
            }

            if (idRestaurante.HasValue)
            {
                reservas = reservas.Where(r => r.IdmesaNavigation.Idrestaurante == idRestaurante.Value);
            }

            var restaurantes = _context.Restaurantes.ToList();
            ViewBag.Restaurantes = restaurantes;

            ViewBag.FechaReserva = fechaReserva?.ToString("yyyy-MM-dd") ?? string.Empty;

            return View(reservas.ToList());
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

        //crea la vista del modal y le carga los datos necesarios
        public IActionResult CreatePartial(int idRestaurante, DateOnly fechaReserva)
        {
            var clientes = _context.Clientes.Select(c => new { c.Id, c.Nombre }).ToList();
            ViewBag.IdCliente = new SelectList(clientes, "Id", "Nombre");

            ViewBag.IdRestaurante = idRestaurante;

            var mesasOcupadas = _context.Reservas
                                .Where(r => r.FechaReserva == fechaReserva && r.IdmesaNavigation.Idrestaurante == idRestaurante)
                                .Select(r => r.Idmesa)
                                .ToList();

            var mesasDisponibles = _context.Mesas
                                           .Where(m => m.Idrestaurante == idRestaurante && !mesasOcupadas.Contains(m.Id))
                                           .Select(m => new { m.Id, m.NumeroMesa })
                                           .ToList();
            ViewBag.IdMesa = new SelectList(mesasDisponibles, "Id", "NumeroMesa");
            ViewBag.FechaReserva = fechaReserva;

            return PartialView("CreatePartialView", new Reserva() { FechaReserva = fechaReserva });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idcliente,Idmesa,FechaReserva,Estado")] Reserva reserva)
        {
            if (_context.Reservas.Any(r => r.Idmesa == reserva.Idmesa && r.FechaReserva == reserva.FechaReserva))
            {
                ModelState.AddModelError("FechaReserva", "La mesa no está disponible para la fecha seleccionada.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var mesa = await _context.Mesas.FindAsync(reserva.Idmesa);
            if (mesa != null)
            {
                var mesas = _context.Mesas
                                    .Where(m => m.Estado == "Disponible" && m.Idrestaurante == reserva.IdmesaNavigation.Idrestaurante)
                                    .Select(m => new { m.Id, m.NumeroMesa })
                                    .ToList();
                ViewBag.IdMesa = new SelectList(mesas, "Id", "NumeroMesa");
            }
            else
            {
                ViewBag.IdMesa = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            //mesa.Idrestaurante

            var clientes = _context.Clientes.Select(c => new { c.Id, c.Nombre }).ToList();
            ViewBag.IdCliente = new SelectList(clientes, "Id", "Nombre");

            return PartialView("CreatePartialView", reserva);
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
        public IActionResult GetMesasDisponibles(int idRestaurante)
        {
            var mesasDisponibles = _context.Mesas
                                           .Where(m => m.Estado == "Disponible" && m.Idrestaurante == idRestaurante)
                                           .Select(m => new { m.Id, m.NumeroMesa })
                                           .ToList();

            return Json(mesasDisponibles);
        }

        [HttpGet]
        public JsonResult CheckDisponibilidad(int idMesa, DateTime fechaReserva)
        {
            DateOnly fecha = DateOnly.FromDateTime(fechaReserva);
            bool disponible = !_context.Reservas.Any(r => r.Idmesa == idMesa && r.FechaReserva == fecha);

            return Json(new { disponible });
        }

    }
}
