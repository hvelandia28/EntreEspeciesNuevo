using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;
using System.Security.Cryptography;
using System.Data;

namespace EntreEspeciesNuevo.Controllers
{
    public class ConfiguracionsController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public ConfiguracionsController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Configuracions
        public async Task<IActionResult> Index()
        {
            var entreespeciessqlContext = _context.Configuracions.Include(c => c.IdPermisoNavigation).Include(c => c.IdRolNavigation);
            return View(await entreespeciessqlContext.ToListAsync());
        }

        // GET: Configuracions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Configuracions == null)
            {
                return NotFound();
            }

            var configuracion = await _context.Configuracions
                .Include(c => c.IdPermisoNavigation)
                .Include(c => c.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdConfiguracion == id);
            if (configuracion == null)
            {
                return NotFound();
            }

            return View(configuracion);
        }

        // GET: Configuracions/Create
        public IActionResult Create(int rolId)
        {
            // Dentro del método Create del controlador
            ViewData["Permisos"] = _context.Permisos.ToList(); // Reemplaza esto con la obtención real de tus permisos

            ViewData["RolId"] = rolId; // Establece el ID de la compra en la vista
            ViewData["IdPermiso"] = new SelectList(_context.Permisos, "IdPermiso", "NomPermiso");
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol");
            return View();
        }

        // POST: Configuracions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(int idRol, int[] selectedPermisos)
        {
            // Lógica para agregar configuración con permisos seleccionados
            if (selectedPermisos != null)
            {
                foreach (var permisoId in selectedPermisos)
                {
                    var configuracion = new Configuracion
                    {
                        IdRol = idRol,
                        IdPermiso = permisoId
                    };

                    _context.Add(configuracion);
                }

                await _context.SaveChangesAsync();
            }

            return Ok(); // Puedes devolver cualquier cosa aquí según tus necesidades
        }

        // GET: Configuracions/Edit/5
        // GET: Configuracions/Edit/5
        public async Task<IActionResult> Edit(int idRol)
        {
            // Obtener la configuración existente para el rol dado
            var configuracionExistente = await _context.Configuracions
                .Where(c => c.IdRol == idRol)
                .ToListAsync();

            // Obtener todos los permisos
            var todosLosPermisos = await _context.Permisos.ToListAsync();

            // Enviar a la vista la lista de permisos y la lista de configuraciones existentes
            ViewData["Permisos"] = todosLosPermisos;
            ViewData["ConfiguracionExistente"] = configuracionExistente;
            ViewData["RolId"] = idRol;

            return View();
        }

        // POST: Configuracions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int rolId, int[] selectedPermisos)
        {
            try
            {
                // Eliminar configuraciones existentes para el rol
                var configuracionesExistentes = await _context.Configuracions
                    .Where(c => c.IdRol == rolId)
                    .ToListAsync();

                _context.Configuracions.RemoveRange(configuracionesExistentes);

                // Agregar nuevas configuraciones según los permisos seleccionados
                if (selectedPermisos != null)
                {
                    foreach (var permisoId in selectedPermisos)
                    {
                        var nuevaConfiguracion = new Configuracion
                        {
                            IdRol = rolId,
                            IdPermiso = permisoId
                        };

                        _context.Configuracions.Add(nuevaConfiguracion);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Roles");
            }
            catch (Exception ex)
            {
                // Manejar el error según tus necesidades
                return RedirectToAction("Index", "Roles");
            }
        }

        // GET: Configuracions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Configuracions == null)
            {
                return NotFound();
            }

            var configuracion = await _context.Configuracions
                .Include(c => c.IdPermisoNavigation)
                .Include(c => c.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdConfiguracion == id);
            if (configuracion == null)
            {
                return NotFound();
            }

            return View(configuracion);
        }

        // POST: Configuracions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Configuracions == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Configuracions'  is null.");
            }
            var configuracion = await _context.Configuracions.FindAsync(id);
            if (configuracion != null)
            {
                _context.Configuracions.Remove(configuracion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConfiguracionExists(int id)
        {
          return (_context.Configuracions?.Any(e => e.IdConfiguracion == id)).GetValueOrDefault();
        }
    }
}
