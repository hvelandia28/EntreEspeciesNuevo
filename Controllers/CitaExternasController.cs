using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.models;
using EntreEspeciesNuevo.Models;
using X.PagedList;

namespace EntreEspeciesNuevo.Controllers
{
    public class CitaExternasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public CitaExternasController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            bool RegistrarCitasExternas = await RegistrarCitaExterna();
            ViewBag.RegistrarCitasExternas = RegistrarCitasExternas;
            bool ActualizarCitasExternas = await ActualizarCitaExterna();
            ViewBag.ActualizarCitasExternas = ActualizarCitasExternas;
            bool EliminarCitasExternas = await EliminarCitaExterna();
            ViewBag.EliminarCitasExternas = EliminarCitasExternas;

            int pageSize = 5; // Define el número de elementos por página
            int pageNumber = page ?? 1; // Si page es nulo, establece el número de página en 1

            var model = await _context.CitaExternas
                .Include(c => c.DocumentoClienteNavigation)
                .Include(c => c.IdMascotaNavigation)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(model);
        }
        public async Task<bool> RegistrarCitaExterna()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 25)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> ActualizarCitaExterna()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 26)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> EliminarCitaExterna()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 28)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        // GET: CitaExternas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citaExterna = await _context.CitaExternas
                .Include(c => c.IdMascotaNavigation)
                .FirstOrDefaultAsync(m => m.IdCitaExt == id);

            if (citaExterna == null)
            {
                return NotFound();
            }

            // Consulta la base de datos para obtener los datos actualizados del Cliente y la Mascota
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.DocumentoCliente == citaExterna.IdMascotaNavigation.DocumentoCliente);
            var mascota = await _context.Mascotas.FirstOrDefaultAsync(m => m.IdMascota == citaExterna.IdMascotaNavigation.IdMascota);

            var viewModel = new DetalleCitaViewModel
            {
                CitaExterna = citaExterna,
                Cliente = cliente,
                Mascota = mascota
            };

            return View(viewModel);
        }





        // GET: CitaExternas/Create
        public IActionResult Create()
        {
            // Cargar la lista de mascotas al ViewBag
            ViewBag.IdMascota = new SelectList(_context.Mascotas, "IdMascota", "IdMascota");

            // Inicializar el ViewBag.DocumentoCliente con una lista vacía (se actualizará dinámicamente)
            ViewBag.DocumentoCliente = new SelectList(new List<SelectListItem>(), "Value", "Text");

            return View();
        }

        // Acción Ajax para obtener documentos por IdMascota
        [HttpGet]
        public IActionResult ObtenerMascotasPorDocumento(int documentoCliente)
        {
            try
            {
                var mascotas = _context.Mascotas
                    .Where(m => m.DocumentoCliente == documentoCliente)
                    .Select(m => new SelectListItem { Value = m.IdMascota.ToString(), Text = m.NombreMascota })
                    .ToList();

                return Json(mascotas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
        private List<SelectListItem> ObtenerMascotasPorDocumentoCliente(int documentoCliente)
        {
            // Lógica para obtener mascotas por DocumentoCliente
            var mascotas = _context.Clientes
                .Where(c => c.DocumentoCliente == documentoCliente)
                .SelectMany(c => c.Mascota)
                .Select(m => new SelectListItem
                {
                    Value = m.IdMascota.ToString(),
                    Text = m.NombreMascota
                })
                .ToList();

            return mascotas;


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaExterna citaExterna)
        {
            if (ModelState.IsValid)
            {
                _context.Add(citaExterna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(citaExterna);
        }

        // GET: CitaExternas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CitaExternas == null)
            {
                return NotFound();
            }

            var citaExterna = await _context.CitaExternas.FindAsync(id);
            if (citaExterna == null)
            {
                return NotFound();
            }
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", citaExterna.DocumentoCliente);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "IdMascota", citaExterna.IdMascota);
            return View(citaExterna);
        }

        // POST: CitaExternas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCitaExt,IdMascota,DocumentoCliente,NomCita,FechaHora,Estado,Precio")] CitaExterna citaExterna)
        {
            if (id != citaExterna.IdCitaExt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citaExterna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExternaExists(citaExterna.IdCitaExt))
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
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", citaExterna.DocumentoCliente);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "IdMascota", citaExterna.IdMascota);
            return View(citaExterna);
        }

        // GET: CitaExternas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CitaExternas == null)
            {
                return NotFound();
            }

            var citaExterna = await _context.CitaExternas
                .Include(c => c.DocumentoClienteNavigation)
                .Include(c => c.IdMascotaNavigation)
                .FirstOrDefaultAsync(m => m.IdCitaExt == id);
            if (citaExterna == null)
            {
                return NotFound();
            }

            return View(citaExterna);
        }

        // POST: CitaExternas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CitaExternas == null)
            {
                return Problem("Entity set 'EntreespeciessqlpContext.CitaExternas'  is null.");
            }
            var citaExterna = await _context.CitaExternas.FindAsync(id);
            if (citaExterna != null)
            {
                _context.CitaExternas.Remove(citaExterna);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExternaExists(int id)
        {
            return (_context.CitaExternas?.Any(e => e.IdCitaExt == id)).GetValueOrDefault();
        }
    }
}
