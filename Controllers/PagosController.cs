using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ObligatorioProgram3.Models;
using System.Net.Http;
using ObligatorioProgram3.ViewModels;


namespace ObligatorioProgram3.Controllers
{
    

    public class PagosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ObligatorioProgram3Context _context;
        private readonly CurrencyService _currencyService;

        public PagosController(IHttpClientFactory httpClientFactory, ObligatorioProgram3Context context, CurrencyService currencyService)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _currencyService = currencyService;
        }
        [Authorize(Policy = "VerReportesPermiso")]
        public IActionResult Reportes()
        {
            var pagos = _context.Pagos
                .Include(p => p.IdcotizacionNavigation)
                .Include(p => p.IdreservaNavigation)
                .ToList();

            return View(pagos);
        }

        [Authorize(Policy = "VerPagosPermiso")]
        public async Task<IActionResult> Index(int id)
        {
            var orden = await _context.Ordenes
            .Include(o => o.OrdenDetalles)
            .ThenInclude(d => d.IdmenuNavigation)
            .Include(o => o.IdreservaNavigation)
            .ThenInclude(r => r.IdmesaNavigation)
            .Include(o => o.IdreservaNavigation)
            .ThenInclude(r => r.IdclienteNavigation)
            .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            // Obtener el clima actual
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://api.openweathermap.org/data/2.5/weather?q=Maldonado&appid=44dfeaa3f72f2d3f648a622ca963c41d&units=metric");
            var climaData = JObject.Parse(response);

            var temperatura = climaData["main"]["temp"].ToObject<float>();
            var descripcion = climaData["weather"][0]["description"].ToString();

            // Calcular el descuento basado en el clima
            var descuentoClima = 0m;
            if (temperatura < 10)
            {
                descuentoClima += 0.05m;
            }
            if (temperatura < 0)
            {
                descuentoClima += 0.10m;
            }
            if (descripcion.Contains("rain"))
            {
                descuentoClima += 0.05m;
            }

            // Calcular el descuento basado en el tipo de cliente
            var descuentoCliente = 0m;
            var tipoCliente = orden.IdreservaNavigation.IdclienteNavigation.TipoCliente;
            if (tipoCliente == "Frecuente")
            {
                descuentoCliente = 0.10m;
            }
            else if (tipoCliente == "VIP")
            {
                descuentoCliente = 0.20m;
            }

            // Calcular el monto con descuento
            var montoConDescuento = orden.Total.HasValue ? orden.Total.Value * (1 - descuentoClima - descuentoCliente) : 0;

            var viewModel = new PagoViewModel
            {
                Orden = orden,
                Descuento = descuentoClima,
                DescuentoCliente = descuentoCliente,
                MontoConDescuento = montoConDescuento,
                Temperatura = temperatura,
                DescripcionClima = descripcion
            };

            return View(viewModel);
        }

        [Authorize(Policy = "VerPagosPermiso")]
        public async Task<IActionResult> PagosPartial(int ordenId, decimal montoConDescuento)
        {
            var viewModel = new PagoViewModel
            {
                Orden = await _context.Ordenes
                    .Include(o => o.OrdenDetalles)
                    .ThenInclude(d => d.IdmenuNavigation)
                    .Include(o => o.IdreservaNavigation)
                    .ThenInclude(r => r.IdmesaNavigation)
                    .FirstOrDefaultAsync(o => o.Id == ordenId),
                MontoConDescuento = montoConDescuento
            };

            // Obtener la cotización del dólar
            var client = _httpClientFactory.CreateClient();
            var currencyResponse = await client.GetStringAsync("http://api.currencylayer.com/live?access_key=68ffa533c801b33066ecdb3209c78b17&currencies=UYU&source=USD");
            var currencyData = JObject.Parse(currencyResponse);

            if (currencyData["success"].ToObject<bool>() && currencyData["quotes"]?["USDUYU"] != null)
            {
                var usdToUyuRate = currencyData["quotes"]["USDUYU"].ToObject<decimal>();
                viewModel.MontoConDescuentoUSD = Math.Round(montoConDescuento / usdToUyuRate, 2);
            }
            else
            {
                viewModel.MontoConDescuentoUSD = 0; // O manejar el error de otra manera
            }

            return PartialView("PagosPartial", viewModel);
        }

        [Authorize(Policy = "VerPagosPermiso")]
        [HttpPost]
        public async Task<IActionResult> Pagar(int id, string metodoPago, string moneda)
        {
            try
            {
                var orden = await _context.Ordenes
                    .Include(o => o.IdreservaNavigation)
                    .ThenInclude(r => r.IdmesaNavigation)
                    .Include(o => o.IdreservaNavigation)
                    .ThenInclude(r => r.IdclienteNavigation)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (orden == null)
                {
                    return Json(new { success = false, message = "Orden no encontrada" });
                }

                // Obtener el clima actual y guardar en la base de datos
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetStringAsync("https://api.openweathermap.org/data/2.5/weather?q=Maldonado&appid=44dfeaa3f72f2d3f648a622ca963c41d&units=metric");
                var climaData = JObject.Parse(response);

                var temperatura = climaData["main"]["temp"].ToObject<float>();
                var descripcion = climaData["weather"][0]["description"].ToString();

                var climaRegistro = new Clima
                {
                    Fecha = DateOnly.FromDateTime(DateTime.Now),
                    Temperatura = temperatura,
                    DescripcionClima = descripcion
                };

                _context.Climas.Add(climaRegistro);
                await _context.SaveChangesAsync();

                // Calcular el descuento basado en el clima
                var descuentoClima = 0m;
                if (temperatura < 10)
                {
                    descuentoClima += 0.05m;
                }
                if (temperatura < 0)
                {
                    descuentoClima += 0.10m;
                }
                if (descripcion.Contains("rain"))
                {
                    descuentoClima += 0.05m;
                }

                // Calcular el descuento basado en el tipo de cliente
                var descuentoCliente = 0m;
                var tipoCliente = orden.IdreservaNavigation.IdclienteNavigation.TipoCliente;
                if (tipoCliente == "Frecuente")
                {
                    descuentoCliente = 0.10m;
                }
                else if (tipoCliente == "VIP")
                {
                    descuentoCliente = 0.20m;
                }

                // Calcular el monto con descuento
                var descuentoTotal = descuentoClima + descuentoCliente;
                var montoConDescuento = orden.Total.HasValue ? orden.Total.Value * (1 - descuentoTotal) : 0;

                decimal montoFinal = montoConDescuento;
                Cotizacion cotizacion = null;

                // Si la moneda es USD, obtener la cotización del dólar y ajustar el monto
                if (moneda == "USD")
                {
                    var currencyResponse = await client.GetStringAsync($"http://api.currencylayer.com/live?access_key=68ffa533c801b33066ecdb3209c78b17&currencies=UYU&source=USD");
                    var currencyData = JObject.Parse(currencyResponse);

                    if (currencyData["success"].ToObject<bool>() && currencyData["quotes"]?["USDUYU"] != null)
                    {
                        var usdToUyuRate = currencyData["quotes"]["USDUYU"].ToObject<decimal>();
                        montoFinal = montoConDescuento / usdToUyuRate;

                        // Registrar la cotización en la base de datos
                        cotizacion = new Cotizacion
                        {
                            Fecha = DateOnly.FromDateTime(DateTime.Now),
                            Moneda = "USD",
                            Monto = usdToUyuRate
                        };

                        _context.Cotizacions.Add(cotizacion);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error al obtener la cotización del dólar." });
                    }
                }


                var pago = new Pago
                {
                    Idreserva = orden.Idreserva,
                    Monto = montoFinal,
                    FechaPago = orden.IdreservaNavigation.FechaReserva,
                    MetodoPago = metodoPago,
                    Idclima = climaRegistro.Id,
                    Idcotizacion = cotizacion?.Id
                };

                orden.Estado = "Pagada";
                var mesa = orden.IdreservaNavigation.IdmesaNavigation;
                if (mesa != null)
                {
                    mesa.Estado = "Disponible";
                    _context.Mesas.Update(mesa); // Actualizar el estado de la mesa
                }

                _context.Pagos.Add(pago);
                _context.Ordenes.Update(orden); // Actualizar la orden
                await _context.SaveChangesAsync();


                return Json(new { success = true, message = "Pago realizado con éxito", redirectUrl = Url.Action("Index", "Ordenes") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al realizar el pago: {ex.Message}" });
            }
        }


        [Authorize(Policy = "VerPagosPermiso")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["Idclima"] = new SelectList(_context.Climas, "Id", "Id", pago.Idclima);
            ViewData["Idcotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id", pago.Idcotizacion);
            ViewData["Idreserva"] = new SelectList(_context.Reservas, "Id", "Id", pago.Idreserva);
            return View(pago);
        }

        // POST: Pagoes/Edit/5
        [Authorize(Policy = "VerPagosPermiso")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idreserva,Monto,FechaPago,Idcotizacion,MetodoPago,Idclima")] Pago pago)
        {
            if (id != pago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id))
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
            ViewData["Idclima"] = new SelectList(_context.Climas, "Id", "Id", pago.Idclima);
            ViewData["Idcotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id", pago.Idcotizacion);
            ViewData["Idreserva"] = new SelectList(_context.Reservas, "Id", "Id", pago.Idreserva);
            return View(pago);
        }

        // GET: Pagoes/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.IdclimaNavigation)
                .Include(p => p.IdcotizacionNavigation)
                .Include(p => p.IdreservaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Reportes));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }
    }
}
