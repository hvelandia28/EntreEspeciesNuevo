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
    public class CitaInternasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public CitaInternasController(EntreespeciessqlContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page)
        {
            bool RegistrarCitasInternas = await RegistrarCitaInterna();
            ViewBag.RegistrarCitasInternas = RegistrarCitasInternas;
            bool ActualizarCitasInternas = await ActualizarCitaInterna();
            ViewBag.ActualizarCitasInternas = ActualizarCitasInternas;
            bool EliminarCitasInternas = await EliminarCitaInterna();
            ViewBag.EliminarCitasInternas = EliminarCitasInternas;

            int pageSize = 5; // Define el número de elementos por página
            int pageNumber = page ?? 1; // Si page es nulo, establece el número de página en 1

            var model = await _context.CitaInternas
                .Include(c => c.DocumentoClienteNavigation)
                .Include(c => c.IdMascotaNavigation)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(model);
        }
        public async Task<bool> RegistrarCitaInterna()
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
        public async Task<bool> ActualizarCitaInterna()
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
        public async Task<bool> EliminarCitaInterna()
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
        // GET: CitaInternas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citaInterna = await _context.CitaInternas
                .Include(c => c.IdMascotaNavigation)
                .FirstOrDefaultAsync(m => m.IdCitaInt == id);

            if (citaInterna == null)
            {
                return NotFound();
            }

            // Consulta la base de datos para obtener los datos actualizados del Cliente y la Mascota
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.DocumentoCliente == citaInterna.IdMascotaNavigation.DocumentoCliente);
            var mascota = await _context.Mascotas.FirstOrDefaultAsync(m => m.IdMascota == citaInterna.IdMascotaNavigation.IdMascota);

            var viewModel = new DetalleCitaViewModel
            {
                CitaInterna = citaInterna,
                Cliente = cliente,
                Mascota = mascota
            };

            return View(viewModel);
        }

        // GET: CitaInternas/Create
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
        public async Task<IActionResult> Create(CitaInterna citaInterna)
        {
            if (ModelState.IsValid)
            {
                _context.Add(citaInterna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(citaInterna);
        }


        // GET: CitaInternas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CitaInternas == null)
            {
                return NotFound();
            }

            var citaInterna = await _context.CitaInternas.FindAsync(id);
            if (citaInterna == null)
            {
                return NotFound();
            }
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", citaInterna.DocumentoCliente);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "IdMascota", citaInterna.IdMascota);
            return View(citaInterna);
        }

        // POST: CitaInternas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCitaInt,IdMascota,DocumentoCliente,NomCita,FechaHora,Estado,Precio")] CitaInterna citaInterna)
        {
            if (id != citaInterna.IdCitaInt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citaInterna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaInternaExists(citaInterna.IdCitaInt))
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
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", citaInterna.DocumentoCliente);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "IdMascota", citaInterna.IdMascota);
            return View(citaInterna);
        }

        // GET: CitaInternas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CitaInternas == null)
            {
                return NotFound();
            }

            var citaInterna = await _context.CitaInternas
                .Include(c => c.DocumentoClienteNavigation)
                .Include(c => c.IdMascotaNavigation)
                .FirstOrDefaultAsync(m => m.IdCitaInt == id);
            if (citaInterna == null)
            {
                return NotFound();
            }

            return View(citaInterna);
        }

        // POST: CitaInternas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CitaInternas == null)
            {
                return Problem("Entity set 'EntreespeciessqlpContext.CitaInternas'  is null.");
            }
            var citaInterna = await _context.CitaInternas.FindAsync(id);
            if (citaInterna != null)
            {
                _context.CitaInternas.Remove(citaInterna);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaInternaExists(int id)
        {
            return (_context.CitaInternas?.Any(e => e.IdCitaInt == id)).GetValueOrDefault();
        }
    }
}
