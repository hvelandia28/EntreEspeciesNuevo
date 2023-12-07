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
    public class ProductoesController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public ProductoesController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Productoes
        public async Task<IActionResult> Index()
        {
            bool RegistrarProductos = RegistrarProducto().Result;
            ViewBag.RegistrarProductos = RegistrarProductos;
            bool ActualizarProductos = ActualizarProducto().Result;
            ViewBag.ActualizarProductos = ActualizarProductos;
            bool CambioEstadoProductos = CambioEstadoProducto().Result;
            ViewBag.CambioEstadoProductos = CambioEstadoProductos;
            var entreespeciessqlContext = _context.Productos.Include(p => p.IdCategoriaNavigation).Include(p => p.IdProveedorNavigation);
            return View(await entreespeciessqlContext.ToListAsync());
        }
        public async Task<bool> RegistrarProducto()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 14)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> ActualizarProducto()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 15)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> CambioEstadoProducto()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 16)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productoes/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NomCategoria");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "NomProveedor");
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,IdProveedor,IdCategoria,NomProducto,Disponibilidad,Cantidad,FechaVen,Precio")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "IdCategoria", producto.IdCategoria);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "IdProveedor", producto.IdProveedor);
            return View(producto);
        }
        [HttpGet]
        public IActionResult GetPrecioProducto(int productId)
        {
            try
            {
                var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == productId);

                if (producto != null)
                {
                    return Json(producto.Precio);
                }

                return NotFound(); // O devuelve un valor predeterminado, según tus requisitos.
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades (por ejemplo, registrarla)
                return StatusCode(500, "Error interno del servidor");
            }
        }



        [HttpGet]
        public IActionResult GetCantidad(int productId)
        {
            try
            {
                var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == productId);

                if (producto != null)
                {
                    return Json(producto.Cantidad);
                }

                return NotFound(); // O devuelve un valor predeterminado, según tus requisitos.
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades (por ejemplo, registrarla)
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAvailability(int idProducto, int disponibilidad)
        {
            var producto = await _context.Productos.FindAsync(idProducto);

            if (producto != null)
            {
                // Verifica si la cantidad es igual o menor a 0 y actualiza la disponibilidad en consecuencia
                if (producto.Cantidad < 0)
                {
                    producto.Disponibilidad = 0;
                }
                else
                {
                    // Invertir el valor de disponibilidad (0 a 1 o 1 a 0)
                    producto.Disponibilidad = (disponibilidad == 1) ? 0 : 1;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "IdCategoria", producto.IdCategoria);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "IdProveedor", producto.IdProveedor);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Productoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,IdProveedor,IdCategoria,NomProducto,Disponibilidad,Cantidad,FechaVen,Precio")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar la disponibilidad a 0 si la cantidad es 0
                    if (producto.Cantidad <= 0)
                    {
                        producto.Disponibilidad = 0;
                    }

                    // Resto del código para actualizar el producto
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
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
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "IdCategoria", producto.IdCategoria);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "IdProveedor", producto.IdProveedor);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
          return (_context.Productos?.Any(e => e.IdProducto == id)).GetValueOrDefault();
        }
    }
}
