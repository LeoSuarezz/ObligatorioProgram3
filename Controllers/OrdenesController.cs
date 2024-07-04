using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.ViewModels;

namespace ObligatorioProgram3.Controllers
{
    [Authorize(Policy = "VerOrdenesPermiso")]
    public class OrdenesController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public OrdenesController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

    
        public async Task<IActionResult> Index(int? restauranteId)
        {
            // Obtener todas las mesas si no se especifica un restauranteId
            IQueryable<Mesa> mesasQuery = _context.Mesas;

            if (restauranteId.HasValue)
            {
                // Filtrar las mesas por restauranteId si se especifica
                mesasQuery = mesasQuery.Where(m => m.Idrestaurante == restauranteId.Value);
            }

            var mesas = await mesasQuery.ToListAsync();
            var menuItems = await _context.Menus.ToListAsync();

            // También puedes obtener los restaurantes para el selector de la vista
            ViewBag.Restaurantes = await _context.Restaurantes.ToListAsync();

            var viewModel = new MesaViewModel
            {
                Mesas = mesas,
                MenuItems = menuItems
                // Asegúrate de incluir cualquier otra información necesaria en el ViewModel
            };

            return View(viewModel);
        }

        // Acción para obtener detalles de la orden de una mesa
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(int mesaId)
        {
            var mesa = await _context.Mesas
                .Include(m => m.Reservas)
                .ThenInclude(r => r.Ordenes)
                .FirstOrDefaultAsync(m => m.Id == mesaId);

            if (mesa == null)
            {
                return NotFound();
            }

            var orden = mesa.Reservas
                .SelectMany(r => r.Ordenes)
                .FirstOrDefault();

            if (orden == null)
            {
                return PartialView("OrdenDetallePartial", new OrdenDetailsViewModel { Mesa = mesa });
            }

            orden.OrdenDetalles = await _context.OrdenDetalles
                .Where(od => od.Idorden == orden.Id)
                .Include(od => od.IdmenuNavigation) // Asegúrate de incluir la navegación al menú
                .ToListAsync();

            var viewModel = new OrdenDetailsViewModel
            {
                Mesa = mesa,
                Orden = orden
            };

            return PartialView("OrdenDetallePartial", viewModel);
        }



        // Acción para agregar un ítem del menú a la orden de una mesa
        [HttpPost]
        public async Task<IActionResult> AddMenuItemToOrder(int ordenId, int menuItemId)
        {
            var orden = await _context.Ordenes.FindAsync(ordenId);
            if (orden == null)
            {
                return NotFound();
            }

            var menuItem = await _context.Menus.FindAsync(menuItemId);
            if (menuItem == null)
            {
                return NotFound();
            }


            var ordenDetalle = new OrdenDetalle
            {
                
                Idorden = ordenId,
                Idmenu = menuItemId,
                Cantidad = 1,
            };

            _context.OrdenDetalles.Add(ordenDetalle);
            orden.Total += ordenDetalle.IdmenuNavigation.Precio;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
