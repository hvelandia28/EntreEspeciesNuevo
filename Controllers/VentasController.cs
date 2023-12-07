using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;
using X.PagedList;

namespace EntreEspeciesNuevo.Controllers
{
    public class VentasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public VentasController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;

            var entreespeciessqlContext = await _context.Ventas.Include(v => v.DocumentoClienteNavigation)
                                                              .ToPagedListAsync(pageNumber, pageSize);

            return View(entreespeciessqlContext);
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.DocumentoClienteNavigation)
                .Include(c => c.DetaVenta)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }
            // Cargar el nombre del producto para cada detalle
            foreach (var detalle in venta.DetaVenta)
            {
                detalle.ProductoNombre = await ObtenerNombreProducto(detalle.IdProducto);
            }

            return View(venta);
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

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,DocumentoCliente,FechaVenta,FormaPago,Total")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create","DetaVentums", new { ventaId = venta.IdVenta});
            }
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", venta.DocumentoCliente);
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", venta.DocumentoCliente);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,DocumentoCliente,FechaVenta,FormaPago,Total")] Venta venta)
        {
            if (id != venta.IdVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.IdVenta))
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
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", venta.DocumentoCliente);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.DocumentoClienteNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ventas == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Ventas'  is null.");
            }
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
          return (_context.Ventas?.Any(e => e.IdVenta == id)).GetValueOrDefault();
        }
    }
}
