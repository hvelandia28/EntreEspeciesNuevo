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
    public class DetaVentumsController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public DetaVentumsController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: DetaVentums
        public async Task<IActionResult> Index()
        {
            var entreespeciessqlContext = _context.DetaVenta.Include(d => d.IdProductoNavigation).Include(d => d.IdServicioNavigation).Include(d => d.IdVentaNavigation);
            return View(await entreespeciessqlContext.ToListAsync());
        }

        // GET: DetaVentums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetaVenta == null)
            {
                return NotFound();
            }

            var detaVentum = await _context.DetaVenta
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdServicioNavigation)
                .Include(d => d.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleV == id);
            if (detaVentum == null)
            {
                return NotFound();
            }

            return View(detaVentum);
        }

        // GET: DetaVentums/Create
        public IActionResult Create(int ventaId)
        {
            // Obtener detalles anteriores
            var detallesAnteriores = _context.DetaVenta
                .Where(dv => dv.IdVenta == ventaId)
                .ToList();

            // Obtener la disponibilidad de los productos (considerando que Disponibilidad es de tipo int)
            var productosConDisponibilidad = _context.Productos
                .Where(p => p.Disponibilidad > 0)  // Filtrar productos con disponibilidad mayor que 0
                .ToList();

            // Pasar detalles anteriores y productos con disponibilidad a la vista
            ViewBag.DetallesAnteriores = detallesAnteriores;
            ViewBag.ProductosConDisponibilidad = new SelectList(productosConDisponibilidad, "IdProducto", "NomProducto");

            ViewData["VentaId"] = ventaId;
            ViewBag.IdServicio = new SelectList(_context.Servicios, "IdServicio", "NomServico");
            ViewData["IdVenta"] = new SelectList(_context.Ventas, "IdVenta", "IdVenta");

            // Limpiar TempData después de usarlo
            TempData.Remove("ventaId");

            return View();
        }

        // POST: DetaVentums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetalleV,IdVenta,IdServicio,IdProducto,Cantidad,SubTotalPro,SubTotalSer")] DetaVentum detaVentum, string action)
        {
            if (ModelState.IsValid)
            {
                // Verificar si se ha seleccionado al menos un servicio o un producto
                if (detaVentum.IdServicio == null && detaVentum.IdProducto == null)
                {
                    ModelState.AddModelError("", "Debes seleccionar al menos un servicio o un producto.");
                    // Puedes cargar las listas nuevamente si es necesario

                    // Almacena el ID de la venta en TempData
                    TempData["ventaId"] = detaVentum.IdVenta;

                    ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "NomProducto");
                    ViewBag.IdServicio = new SelectList(_context.Servicios, "IdServicio", "NomServico");
                    ViewData["IdVenta"] = new SelectList(_context.Ventas, "IdVenta", "IdVenta");
                    return  View(detaVentum);
                }
                // Actualizar la cantidad en el producto si se ha seleccionado un producto
                if (detaVentum.IdProducto != null)
                {
                    var producto = await _context.Productos.FindAsync(detaVentum.IdProducto);
                    if (producto != null)
                    {
                        // Restaurar la cantidad original antes de la edición
                        //producto.Cantidad += _context.DetaVenta.AsNoTracking().FirstOrDefault(dv => dv.IdProducto == detaVentum.IdProducto)?.Cantidad ?? 0;

                        // Actualizar con la nueva cantidad
                        producto.Cantidad -= detaVentum.Cantidad;

                        // Actualizar la disponibilidad a 0 si la cantidad es 0
                        if (producto.Cantidad == 0)
                        {
                            producto.Disponibilidad = 0;
                        }
                        _context.Update(producto);
                    }
                }
                _context.Add(detaVentum);
                await _context.SaveChangesAsync();
                // Actualizar el total de la venta incluso si solo se selecciona un servicio o un producto
                await UpdateTotalVenta(detaVentum.IdVenta);
                if (action == "Finalizar")
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (action == "RegistrarOtra")
                {
                    // Redirige a la acción de crear con el mismo IdVenta
                    return RedirectToAction("Create", new { ventaId = detaVentum.IdVenta });
                }
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detaVentum.IdProducto);
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", detaVentum.IdServicio);
            ViewData["IdVenta"] = new SelectList(_context.Ventas, "IdVenta", "IdVenta", detaVentum.IdVenta);
            return View(detaVentum);
        }
        private async Task UpdateTotalVenta(int ventaId)
        {
            var venta = await _context.Ventas.FindAsync(ventaId);
            if (venta != null)
            {
                // Calcular el total de la venta sumando los SubTotalPro y SubTotalSer de los detalles
                var totalDetalle = _context.DetaVenta
                    .Where(dv => dv.IdVenta == ventaId)
                    .AsEnumerable()
                    .Sum(dv => (dv.SubTotalPro ?? 0) + (dv.SubTotalSer ?? 0));

                // Actualizar el total de la venta
                venta.Total = (int)totalDetalle;

                await _context.SaveChangesAsync();
            }
        }

        // GET: DetaVentums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetaVenta == null)
            {
                return NotFound();
            }

            var detaVentum = await _context.DetaVenta.FindAsync(id);
            if (detaVentum == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detaVentum.IdProducto);
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", detaVentum.IdServicio);
            ViewData["IdVenta"] = new SelectList(_context.Ventas, "IdVenta", "IdVenta", detaVentum.IdVenta);
            return View(detaVentum);
        }

        // POST: DetaVentums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalleV,IdVenta,IdServicio,IdProducto,Cantidad,SubTotalPro,SubTotalSer")] DetaVentum detaVentum)
        {
            if (id != detaVentum.IdDetalleV)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar la cantidad en el producto si se ha seleccionado un producto
                    if (detaVentum.IdProducto != null)
                    {
                        var producto = await _context.Productos.FindAsync(detaVentum.IdProducto);
                        if (producto != null)
                        {
                            // Restaurar la cantidad original antes de la edición
                            producto.Cantidad += _context.DetaVenta.AsNoTracking().FirstOrDefault(dv => dv.IdDetalleV == id)?.Cantidad ?? 0;

                            // Actualizar con la nueva cantidad
                            producto.Cantidad -= detaVentum.Cantidad;
                            _context.Update(producto);
                        }
                    }

                    _context.Update(detaVentum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetaVentumExists(detaVentum.IdDetalleV))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Create", new { ventaId = detaVentum.IdVenta });
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detaVentum.IdProducto);
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", detaVentum.IdServicio);
            ViewData["IdVenta"] = new SelectList(_context.Ventas, "IdVenta", "IdVenta", detaVentum.IdVenta);
            return View(detaVentum);
        }
        // GET: DetaVentums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetaVenta == null)
            {
                return NotFound();
            }

            var detaVentum = await _context.DetaVenta
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdServicioNavigation)
                .Include(d => d.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleV == id);
            if (detaVentum == null)
            {
                return NotFound();
            }

            return View(detaVentum);
        }

        // POST: DetaVentums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetaVenta == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.DetaVenta'  is null.");
            }
            var detaVentum = await _context.DetaVenta.FindAsync(id);
            if (detaVentum != null)
            {
                // Restaurar la cantidad original antes de eliminar el detalle
                var producto = await _context.Productos.FindAsync(detaVentum.IdProducto);
                if (producto != null)
                {
                    producto.Cantidad += detaVentum.Cantidad;
                    _context.Update(producto);
                }
                _context.DetaVenta.Remove(detaVentum);

                // Actualizar el total de la venta después de eliminar el detalle
                await UpdateTotalVenta(detaVentum.IdVenta);

                await _context.SaveChangesAsync();
            }

            // Redirigir a la acción Create con el mismo IdVenta
            return RedirectToAction("Create", new { ventaId = detaVentum.IdVenta });
        }

        private bool DetaVentumExists(int id)
        {
          return (_context.DetaVenta?.Any(e => e.IdDetalleV == id)).GetValueOrDefault();
        }
    }
}
