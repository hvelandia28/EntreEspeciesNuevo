using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;

namespace EntreEspeciesNuevo.Controllers
{
    public class ComprasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public ComprasController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            bool RegistrarCompras = RegistrarCompra().Result;
            ViewBag.RegistrarCompras = RegistrarCompras;
            return _context.Compras != null ? 
                          View(await _context.Compras.ToListAsync()) :
                          Problem("Entity set 'EntreespeciessqlContext.Compras'  is null.");
        }
        public async Task<bool> RegistrarCompra()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 21)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.DetaCompras)
                .FirstOrDefaultAsync(m => m.IdCompra == id);

            if (compra == null)
            {
                return NotFound();
            }

            // Cargar el nombre del producto para cada detalle
            foreach (var detalle in compra.DetaCompras)
            {
                detalle.ProductoNombre = await ObtenerNombreProducto(detalle.IdProducto);
            }

            return View(compra);
        }

        private async Task<string> ObtenerNombreProducto(int? idProducto)
        {
            if (idProducto == null)
            {
                return "Producto no disponible";
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == idProducto);

            return producto?.NomProducto ?? "Producto no disponible";
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Compras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,FechaCompra,FormaPago,Total")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                // Verificar si la fecha no se ha seleccionado
                if (compra.FechaCompra == null)
                {
                    // Establecer la fecha y hora actual
                    compra.FechaCompra = DateTime.Now;
                }

                _context.Add(compra);
                await _context.SaveChangesAsync();

                // Redirige a la acción "Create" del controlador de "DetaCompras" y pasa el ID de la compra
                return RedirectToAction("Create", "DetaCompras", new { compraId = compra.IdCompra });
            }
            return View(compra);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompra,FechaCompra,FormaPago")] Compra compra)
        {
            if (id != compra.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.IdCompra))
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
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Compras == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Compras'  is null.");
            }
            var compra = await _context.Compras.FindAsync(id);
            if (compra != null)
            {
                _context.Compras.Remove(compra);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
          return (_context.Compras?.Any(e => e.IdCompra == id)).GetValueOrDefault();
        }
    }
}
