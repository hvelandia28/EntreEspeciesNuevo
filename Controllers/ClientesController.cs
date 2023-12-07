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
    public class ClientesController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public ClientesController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(int? page)
        {
            bool RegistrarClientes = await RegistrarCliente();
            ViewBag.RegistrarClientes = RegistrarClientes;
            bool EditarClientes = await EditarCliente();
            ViewBag.EditarClientes = EditarClientes;
            bool CambioEstadoClientes = await CambioEstadoCliente();
            ViewBag.CambioEstadoClientes = CambioEstadoClientes;

            int pageSize = 5; // Define el número de elementos por página
            int pageNumber = page ?? 1; // Si page es nulo, establece el número de página en 1

            var model = _context.Clientes.ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        public async Task<bool> RegistrarCliente()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 7)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> EditarCliente()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 8)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> CambioEstadoCliente()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 9)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.DocumentoCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocumentoCliente,TipoDocu,NombreCliente,Direccion,Telefono,Correo")] Cliente cliente)
        {
            // Verificar si ya existe un cliente con el mismo número de documento
            if (_context.Clientes.Any(c => c.DocumentoCliente == cliente.DocumentoCliente))
            {
                ModelState.AddModelError("DocumentoCliente", "Ya existe un cliente con este número de documento.");
                return View(cliente);
            }

            // Si no hay duplicados, continuar con el proceso de creación
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }
        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocumentoCliente,TipoDocu,NombreCliente,Direccion,Telefono,Correo")] Cliente cliente)
        {
            if (id != cliente.DocumentoCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.DocumentoCliente))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.DocumentoCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return (_context.Clientes?.Any(e => e.DocumentoCliente == id)).GetValueOrDefault();
        }
    }
}
