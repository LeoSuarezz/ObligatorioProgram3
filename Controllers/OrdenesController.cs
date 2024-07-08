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

       public IActionResult Reportes()
        {
            var ordenes = _context.Ordenes
                .Include(o => o.OrdenDetalles) // Incluir detalles de la orden
                    .ThenInclude(d => d.IdmenuNavigation) // Incluir la relación a IdmenuNavigation si es necesario
                .ToList();

            return View(ordenes);
        }
        public async Task<IActionResult> Index(int? restauranteId, string categoria = "")
        {
            IQueryable<Mesa> mesasQuery = _context.Mesas;
            IQueryable<Menu> menusQuery = _context.Menus;

            if (restauranteId.HasValue)
            {
                mesasQuery = mesasQuery.Where(m => m.Idrestaurante == restauranteId.Value);
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                menusQuery = menusQuery.Where(m => m.Categoria == categoria);
            }

            var mesas = await mesasQuery.ToListAsync();
            var menuItems = await menusQuery.ToListAsync();
            var categorias = new List<string> { "Principal", "Entradas", "Bebidas", "Postres" };

            ViewBag.Categorias = categorias;
            ViewBag.Restaurantes = await _context.Restaurantes.ToListAsync();
            ViewBag.MenuItems = menuItems;

            var viewModel = new MesaViewModel
            {
                Mesas = mesas,
                MenuItems = menuItems
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

            // Buscar si ya existe un OrdenDetalle con el mismo ordenId y menuItemId
            var existingOrdenDetalle = await _context.OrdenDetalles
                .FirstOrDefaultAsync(od => od.Idorden == ordenId && od.Idmenu == menuItemId);

            if (existingOrdenDetalle != null)
            {
                // Si existe, incrementar la cantidad
                existingOrdenDetalle.Cantidad++;
            }
            else
            {
                // Si no existe, crear un nuevo registro
                var ordenDetalle = new OrdenDetalle
                {
                    Idorden = ordenId,
                    Idmenu = menuItemId,
                    Cantidad = 1,
                };
                _context.OrdenDetalles.Add(ordenDetalle);
            }

            // Actualizar el total de la orden
            orden.Total += menuItem.Precio;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveMenuItemFromOrder(int ordenId, int menuItemId)
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

            var existingOrdenDetalle = await _context.OrdenDetalles
                .FirstOrDefaultAsync(od => od.Idorden == ordenId && od.Idmenu == menuItemId);

            if (existingOrdenDetalle != null)
            {
                if (existingOrdenDetalle.Cantidad > 1)
                {
                    existingOrdenDetalle.Cantidad--;
                }
                else
                {
                    _context.OrdenDetalles.Remove(existingOrdenDetalle);
                }

                orden.Total -= menuItem.Precio;
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }



    }
}
