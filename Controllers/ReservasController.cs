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

            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();

                if (reserva.Estado == "Confirmada")
                {
                    var orden = new Ordene
                    {
                        Idreserva = reserva.Id,
                        Total = 0,
                        IdreservaNavigation = reserva

                        // Otros campos necesarios
                    };
                    _context.Ordenes.Add(orden);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Cargar la mesa y el restaurante asociados a la mesa
            var mesa = await _context.Mesas
                                    .Include(m => m.IdrestauranteNavigation)
                                    .FirstOrDefaultAsync(m => m.Id == reserva.Idmesa);
            if (mesa != null)
            {
                // Obtener las mesas ocupadas para la fecha y el restaurante seleccionados
                var mesasOcupadas = _context.Reservas
                                    .Where(r => r.FechaReserva == reserva.FechaReserva && r.IdmesaNavigation.Idrestaurante == mesa.Idrestaurante)
                                    .Select(r => r.Idmesa)
                                    .ToList();

                // Filtrar las mesas disponibles
                var mesasDisponibles = _context.Mesas
                                               .Where(m => m.Idrestaurante == mesa.Idrestaurante && !mesasOcupadas.Contains(m.Id))
                                               .Select(m => new { m.Id, m.NumeroMesa })
                                               .ToList();

                ViewBag.IdMesa = new SelectList(mesasDisponibles, "Id", "NumeroMesa");
            }
            else
            {
                ViewBag.IdMesa = new SelectList(Enumerable.Empty<SelectListItem>());
            }

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

            var reserva = await _context.Reservas
                .Include(r => r.IdclienteNavigation)
                .Include(r => r.IdmesaNavigation)
                .ThenInclude(m => m.IdrestauranteNavigation) // Incluye el restaurante
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            var clientes = _context.Clientes.Select(c => new { c.Id, NombreCompleto = c.Nombre + " " + c.Apellido }).ToList();
            ViewData["Idcliente"] = new SelectList(clientes, "Id", "NombreCompleto", reserva.Idcliente);

            var mesasOcupadas = _context.Reservas
                                .Where(r => r.FechaReserva == reserva.FechaReserva && r.IdmesaNavigation.Idrestaurante == reserva.IdmesaNavigation.Idrestaurante && r.Id != reserva.Id)
                                .Select(r => r.Idmesa)
                                .ToList();

            var mesasDisponibles = _context.Mesas
                                           .Where(m => m.Idrestaurante == reserva.IdmesaNavigation.Idrestaurante && !mesasOcupadas.Contains(m.Id))
                                           .Select(m => new { m.Id, m.NumeroMesa })
                                           .ToList();

            // Agregar la mesa actual a las mesas disponibles
            if (!mesasDisponibles.Any(m => m.Id == reserva.Idmesa))
            {
                var mesaActual = _context.Mesas
                                         .Where(m => m.Id == reserva.Idmesa)
                                         .Select(m => new { m.Id, m.NumeroMesa })
                                         .FirstOrDefault();
                if (mesaActual != null)
                {
                    mesasDisponibles.Add(mesaActual);
                }
            }

            ViewData["Idmesa"] = new SelectList(mesasDisponibles, "Id", "NumeroMesa", reserva.Idmesa);

            return View(reserva);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idcliente,Idmesa,Estado")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            // Obtén la reserva original de la base de datos
            var reservaOriginal = await _context.Reservas.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (reservaOriginal == null)
            {
                return NotFound();
            }

            // Mantén la fecha y el restaurante originales
            reserva.FechaReserva = reservaOriginal.FechaReserva;
            reserva.IdmesaNavigation = reservaOriginal.IdmesaNavigation;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();

                    if (reserva.Estado == "Confirmada")
                    {
                        var ordenExistente = _context.Ordenes.FirstOrDefault(o => o.Idreserva == reserva.Id);
                        if (ordenExistente == null)
                        {
                            var orden = new Ordene
                            {
                                Idreserva = reserva.Id,
                                Total = 0, // Inicializar con 0 o calcular el total según tu lógica
                                IdreservaNavigation = reserva
                            };
                            _context.Ordenes.Add(orden);
                            await _context.SaveChangesAsync();
                        }
                    }
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
            ViewData["Idmesa"] = new SelectList(_context.Mesas
                                    .Where(m => m.Estado == "Disponible" && m.Idrestaurante == reservaOriginal.IdmesaNavigation.Idrestaurante)
                                    .Select(m => new { m.Id, m.NumeroMesa }), "Id", "NumeroMesa", reserva.Idmesa);
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

        [HttpPost]
        public async Task<IActionResult> ConfirmarReserva(int id)
        {
            var reserva = await _context.Reservas
                                .Include(r => r.IdmesaNavigation)
                                .FirstOrDefaultAsync(r => r.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            
            reserva.Estado = "Confirmada";
            _context.Update(reserva);

            var mesa = reserva.IdmesaNavigation;
            if (mesa != null)
            {
                mesa.Estado = "Reservada";
                _context.Update(mesa);
            }


            await _context.SaveChangesAsync();

            // Crear una orden asociada a la reserva confirmada
            var ordenExistente = await _context.Ordenes.FirstOrDefaultAsync(o => o.Idreserva == reserva.Id);
            if (ordenExistente == null)
            {
                reserva.IdmesaNavigation.Estado = "Ocupada";

                var orden = new Ordene
                {
                    Idreserva = reserva.Id,
                    Total = 0, // Inicializar con 0 o calcular el total según tu lógica
                    IdreservaNavigation = reserva,
                };
                _context.Ordenes.Add(orden);
                await _context.SaveChangesAsync();
            }

            return Ok();
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
