using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;
using Microsoft.Extensions.Logging;

namespace EntreEspeciesNuevo.Controllers
{
    public class DetaComprasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public DetaComprasController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: DetaCompras
        public async Task<IActionResult> Index()
        {
            var entreespeciessqlContext = _context.DetaCompras.Include(d => d.IdCompraNavigation).Include(d => d.IdProductoNavigation);
            return View(await entreespeciessqlContext.ToListAsync());
        }

        // GET: DetaCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetaCompras == null)
            {
                return NotFound();
            }

            var detaCompra = await _context.DetaCompras
                .Include(d => d.IdCompraNavigation)
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetaCompra == id);
            if (detaCompra == null)
            {
                return NotFound();
            }

            return View(detaCompra);
        }

        // GET: DetaCompras/Create
        public IActionResult Create(int compraId)
        {
            // Obtener detalles anteriores
            var detallesAnteriores = _context.DetaCompras
                .Where(dc => dc.IdCompra == compraId)
                .ToList();

            // Obtener la disponibilidad de los productos (considerando que Disponibilidad es de tipo int)
            var productosConDisponibilidad = _context.Productos
                .Where(p => p.Disponibilidad > 0)  // Filtrar productos con disponibilidad mayor que 0
                .ToList();
            // Pasar detalles anteriores a la vista
            ViewBag.DetallesAnteriores = detallesAnteriores;
            ViewData["CompraId"] = compraId; // Establece el ID de la compra en la vista
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra");
            ViewBag.ProductosConDisponibilidad = new SelectList(productosConDisponibilidad, "IdProducto", "NomProducto");
            return View();
        }

        // POST: DetaCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetaCompra,IdCompra,IdProducto,Cantidad,PrecioCompra,Subtotal")] DetaCompra detaCompra, string submitButton)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detaCompra);

                // Actualizar la cantidad en el producto
                var producto = await _context.Productos.FindAsync(detaCompra.IdProducto);
                if (producto != null)
                {
                    producto.Cantidad += detaCompra.Cantidad;
                    _context.Update(producto);
                }

                await _context.SaveChangesAsync();

                if (submitButton == "Finalizar")
                {
                    // Lógica para finalizar la compra y redirigir al índice
                    await UpdateTotalCompra(detaCompra.IdCompra);
                    return RedirectToAction("Index","Compras");
                }
                else if (submitButton == "Registrar Otra")
                {
                    // Lógica para registrar otra compra y redirigir a la misma vista con el mismo ID de compra
                    return RedirectToAction("Create", new { compraId = detaCompra.IdCompra });
                }
            }

            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", detaCompra.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detaCompra.IdProducto);
            return View(detaCompra);
        }
        private async Task UpdateTotalCompra(int compraId)
        {
            var compra = await _context.Compras.FindAsync(compraId);
            if (compra != null)
            {
                var totalDetalle = _context.DetaCompras
                    .Where(dc => dc.IdCompra == compraId)
                    .AsEnumerable() // Trae los datos a la memoria
                    .Sum(dc => dc.TotalDetalle);

                compra.Total = (int)totalDetalle;

                await _context.SaveChangesAsync();
            }
        }


        // GET: DetaCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetaCompras == null)
            {
                return NotFound();
            }

            var detaCompra = await _context.DetaCompras.FindAsync(id);
            if (detaCompra == null)
            {
                return NotFound();
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", detaCompra.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detaCompra.IdProducto);
            return View(detaCompra);
        }

        // POST: DetaCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetaCompra,IdCompra,IdProducto,Cantidad,PrecioCompra,Subtotal")] DetaCompra detaCompra)
        {
            if (id != detaCompra.IdDetaCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var producto = await _context.Productos.FindAsync(detaCompra.IdProducto);
                    if (producto != null)
                    {
                        // Restaurar la cantidad original antes de la edición
                        producto.Cantidad -= _context.DetaCompras.AsNoTracking().FirstOrDefault(dv => dv.IdDetaCompra == id)?.Cantidad ?? 0;

                        // Actualizar con la nueva cantidad
                        producto.Cantidad += detaCompra.Cantidad;
                        _context.Update(producto);
                    }
                    _context.Update(detaCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetaCompraExists(detaCompra.IdDetaCompra))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Create", new { compraId = detaCompra.IdCompra});
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", detaCompra.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detaCompra.IdProducto);
            return View(detaCompra);
        }

        // GET: DetaCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetaCompras == null)
            {
                return NotFound();
            }

            var detaCompra = await _context.DetaCompras
                .Include(d => d.IdCompraNavigation)
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetaCompra == id);
            if (detaCompra == null)
            {
                return NotFound();
            }

            return View(detaCompra);
        }

        // POST: DetaCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetaCompras == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.DetaCompras'  is null.");
            }
            var detaCompra = await _context.DetaCompras.FindAsync(id);
            if (detaCompra != null)
            {
                // Restaurar la cantidad original antes de eliminar el detalle
                var producto = await _context.Productos.FindAsync(detaCompra.IdProducto);
                if (producto != null)
                {
                    producto.Cantidad -= detaCompra.Cantidad;
                    _context.Update(producto);
                }
                _context.DetaCompras.Remove(detaCompra);

                // Actualizar el total de la venta después de eliminar el detalle
                await UpdateTotalCompra(detaCompra.IdCompra);

                await _context.SaveChangesAsync();
            }

            // Redirigir a la acción Create con el mismo IdVenta
            return RedirectToAction("Create", new { compraId = detaCompra.IdCompra  });
        }

        private bool DetaCompraExists(int id)
        {
          return (_context.DetaCompras?.Any(e => e.IdDetaCompra == id)).GetValueOrDefault();
        }
    }
}
