﻿using System;
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
    public class ReseñasController : Controller
    {
        private readonly ObligatorioProgram3Context _context;

        public ReseñasController(ObligatorioProgram3Context context)
        {
            _context = context;
        }

        // GET: Reseña
        public async Task<IActionResult> Index()
        {
            var reseñas = await _context.Reseñas.ToListAsync();
            return View(reseñas);
        }

        // GET: Reseña/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reseña = await _context.Reseñas
                .Include(r => r.IdclienteNavigation)
                .Include(r => r.IdrestauranteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reseña == null)
            {
                return NotFound();
            }

            return View(reseña);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Reseña/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Puntaje,Comentario")] Reseña reseña)
        {
            if (ModelState.IsValid)
            {
                DateTime fechaActual = DateTime.Now;

                // Convertir a DateOnly
                DateOnly fechaDateOnly = new DateOnly(fechaActual.Year, fechaActual.Month, fechaActual.Day);

                // Establecer la fecha actual en la reseña
                reseña.FechaReseña = fechaDateOnly;
                _context.Add(reseña);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View(reseña);
        }

        // GET: Reseña/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reseña = await _context.Reseñas.FindAsync(id);
            if (reseña == null)
            {
                return NotFound();
            }
            ViewData["Idcliente"] = new SelectList(_context.Clientes, "Id", "Id", reseña.Idcliente);
            ViewData["Idrestaurante"] = new SelectList(_context.Restaurantes, "Id", "Id", reseña.Idrestaurante);
            return View(reseña);
        }

        // POST: Reseña/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Puntaje,Comentario,FechaReseña,Idcliente,Idrestaurante")] Reseña reseña)
        {
            if (id != reseña.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reseña);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReseñaExists(reseña.Id))
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
            ViewData["Idcliente"] = new SelectList(_context.Clientes, "Id", "Id", reseña.Idcliente);
            ViewData["Idrestaurante"] = new SelectList(_context.Restaurantes, "Id", "Id", reseña.Idrestaurante);
            return View(reseña);
        }

        // GET: Reseña/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reseña = await _context.Reseñas
                .Include(r => r.IdclienteNavigation)
                .Include(r => r.IdrestauranteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reseña == null)
            {
                return NotFound();
            }

            return View(reseña);
        }

        // POST: Reseña/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reseña = await _context.Reseñas.FindAsync(id);
            if (reseña != null)
            {
                _context.Reseñas.Remove(reseña);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReseñaExists(int id)
        {
            return _context.Reseñas.Any(e => e.Id == id);
        }
    }
}
