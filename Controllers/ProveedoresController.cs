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
    public class ProveedoresController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public ProveedoresController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            bool RegistrarProveedores = RegistrarProveedor().Result;
            ViewBag.RegistrarProveedores = RegistrarProveedores;
            bool ActualizarProveedores = ActualizarProveedor().Result;
            ViewBag.ActualizarProveedores = ActualizarProveedores;
            bool EliminarProveedores = EliminarProveedor().Result;
            ViewBag.EliminarProveedores = EliminarProveedores;
            return _context.Proveedores != null ? 
                          View(await _context.Proveedores.ToListAsync()) :
                          Problem("Entity set 'EntreespeciessqlContext.Proveedores'  is null.");
        }
        public async Task<bool> RegistrarProveedor()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 29)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> ActualizarProveedor()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 30)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> EliminarProveedor()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 31)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedore == null)
            {
                return NotFound();
            }

            return Json(proveedore);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProveedor,NitProveedor,NomProveedor,Correo,Telefono,Direccion")] Proveedore proveedore)
        {
            // Verificar si ya existe un cliente con el mismo número de documento
            if (_context.Proveedores.Any(p => p.NitProveedor == proveedore.NitProveedor))
            {
                ModelState.AddModelError("NitProveedor", "Ya existe un proveedor con este número de nit.");
                return View(proveedore);
            }
            if (ModelState.IsValid)
            {
                _context.Add(proveedore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedore);
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore == null)
            {
                return NotFound();
            }
            return View(proveedore);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProveedor,NitProveedor,NomProveedor,Correo,Telefono,Direccion")] Proveedore proveedore)
        {
            if (id != proveedore.IdProveedor)
            {
                return NotFound();
            }

            // Verificar si ya existe un proveedor con el mismo número de documento y diferente ID
            if (_context.Proveedores.Any(p => p.NitProveedor == proveedore.NitProveedor && p.IdProveedor != id))
            {
                ModelState.AddModelError("NitProveedor", "Ya existe un proveedor con este número de nit.");
                return View(proveedore);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedoreExists(proveedore.IdProveedor))
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

            return View(proveedore);
        }



        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedore == null)
            {
                return NotFound();
            }

            return View(proveedore);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proveedores == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Proveedores'  is null.");
            }
            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore != null)
            {
                _context.Proveedores.Remove(proveedore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedoreExists(int id)
        {
          return (_context.Proveedores?.Any(e => e.IdProveedor == id)).GetValueOrDefault();
        }
    }
}
