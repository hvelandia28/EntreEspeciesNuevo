using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EntreEspeciesNuevo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;
using Microsoft.CodeAnalysis.Scripting;

namespace EntreEspeciesNuevo.Controllers
{
    public class AccesoController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public AccesoController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Acceso/Index
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string correo, string contraseña)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
            {
                ModelState.AddModelError(string.Empty, "Correo y contraseña son obligatorios.");
                return View();
            }


            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo && u.Contraseña == contraseña);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");

                // Mostrar SweetAlert de error
                TempData["ErrorMessage"] = "Correo o contraseña incorrectos.";
                return View();
            }

            // Crear identidad y establecer la cookie de autenticación
            var claims = new[] { new Claim(ClaimTypes.Name, usuario.Nombre) };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Puedes ajustar esto según tus necesidades
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home"); // Redirigir solo si la autenticación es exitosa
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> VerClientes()
        {
            bool tieneAcceso = await VerCliente();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<IActionResult> VisualizarMascotas()
        {
            bool tieneAcceso = await VisualizarMascota();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<IActionResult> VisualizarUsuarios()
        {
            bool tieneAcceso = await VisualizarUsuario();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<IActionResult> VisualizarCitas()
        {
            bool tieneAcceso = await VisualizarCita();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<IActionResult> VisualizarCategorias()
        {
            bool tieneAcceso = await VisualizarCategoria();
            return Json(new { accesoClientes = tieneAcceso });
        }

        public async Task<IActionResult> VisualizarVentas()
        {
            bool tieneAcceso = await VisualizarVenta();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<IActionResult> VisualizarCompras()
        {
            bool tieneAcceso = await VisualizarCompra();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<IActionResult> VisualizarConfiguraciones()
        {
            bool tieneAcceso = await VisualizarConfiguracion();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<IActionResult> VisualizarProveedores()
        {
            bool tieneAcceso = await VisualizarProveedor();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<IActionResult> VisualizarProductos()
        {
            bool tieneAcceso = await VisualizarProducto();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<IActionResult> VisualizarServicios()
        {
            bool tieneAcceso = await VisualizarServicio();
            return Json(new { accesoClientes = tieneAcceso });
        }
        public async Task<bool> VerCliente()
        {
            // Obtén el nombre de usuario desde la sesión
            var username = User.Identity.Name;

            // Obtén el usuario a partir del nombre de usuario
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);

            if (usuario == null)
            {
                // Puedes manejar esta situación según tus necesidades, por ejemplo, lanzar una excepción.
                return false;
            }

            // Verifica si el usuario tiene acceso según la configuración
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 23)
                .FirstOrDefaultAsync();

            return configuracion != null;
        }
        public async Task<bool> VisualizarUsuario()
        {
            // Obtén el nombre de usuario desde la sesión
            var username = User.Identity.Name;

            // Obtén el usuario a partir del nombre de usuario
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);

            if (usuario == null)
            {
                // Puedes manejar esta situación según tus necesidades, por ejemplo, lanzar una excepción.
                return false;
            }

            // Verifica si el usuario tiene acceso según la configuración
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 6)
                .FirstOrDefaultAsync();

            return configuracion != null;
        }
        public async Task<bool> VisualizarProducto()
        {
            // Obtén el nombre de usuario desde la sesión
            var username = User.Identity.Name;

            // Obtén el usuario a partir del nombre de usuario
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);

            if (usuario == null)
            {
                // Puedes manejar esta situación según tus necesidades, por ejemplo, lanzar una excepción.
                return false;
            }

            // Verifica si el usuario tiene acceso según la configuración
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 56)
                .FirstOrDefaultAsync();

            return configuracion != null;
        }
        public async Task<bool> VisualizarProveedor()
        {
            // Obtén el nombre de usuario desde la sesión
            var username = User.Identity.Name;

            // Obtén el usuario a partir del nombre de usuario
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);

            if (usuario == null)
            {
                // Puedes manejar esta situación según tus necesidades, por ejemplo, lanzar una excepción.
                return false;
            }

            // Verifica si el usuario tiene acceso según la configuración
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 28)
                .FirstOrDefaultAsync();

            return configuracion != null;
        }
        public async Task<bool> VisualizarCategoria()
        {
            // Obtén el nombre de usuario desde la sesión
            var username = User.Identity.Name;

            // Obtén el usuario a partir del nombre de usuario
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);

            if (usuario == null)
            {
                // Puedes manejar esta situación según tus necesidades, por ejemplo, lanzar una excepción.
                return false;
            }

            // Verifica si el usuario tiene acceso según la configuración
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 52)
                .FirstOrDefaultAsync();

            return configuracion != null;
        }
        public async Task<bool> VisualizarCita()
        {
            // Obtén el nombre de usuario desde la sesión
            var username = User.Identity.Name;

            // Obtén el usuario a partir del nombre de usuario
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);

            if (usuario == null)
            {
                // Puedes manejar esta situación según tus necesidades, por ejemplo, lanzar una excepción.
                return false;
            }

            // Verifica si el usuario tiene acceso según la configuración
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 27)
                .FirstOrDefaultAsync();

            return configuracion != null;
        }
        public async Task<bool> VisualizarConfiguracion()
        {
            // Obtén el nombre de usuario desde la sesión
            var username = User.Identity.Name;

            // Obtén el usuario a partir del nombre de usuario
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);

            if (usuario == null)
            {
                // Puedes manejar esta situación según tus necesidades, por ejemplo, lanzar una excepción.
                return false;
            }

            // Verifica si el usuario tiene acceso según la configuración
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 32)
                .FirstOrDefaultAsync();

            return configuracion != null;
        }

        public async Task<bool> VisualizarVenta()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 18)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> VisualizarCompra()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 23)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> VisualizarMascota()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 11)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> VisualizarServicio()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 57)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        [HttpGet]
        public IActionResult OlvidoContraseña()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OlvidoContraseña(string correo)
        {
            if (string.IsNullOrEmpty(correo))
            {
                ModelState.AddModelError(string.Empty, "El correo es obligatorio.");
                return View();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "No se encontró un usuario con este correo.");
                return View();
            }

            // Generar el token y almacenarlo en la base de datos
            var token = Guid.NewGuid().ToString();
            usuario.ResetToken = token;
            usuario.ResetTokenExpiration = DateTime.UtcNow.AddHours(1); // Establecer una expiración, por ejemplo, 1 hora

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Enviar el correo electrónico con el enlace que contiene el token
            EnviarCorreoRecuperacion(usuario.Correo, token);

            TempData["Mensaje"] = "Se ha enviado un enlace de restablecimiento de contraseña a tu correo electrónico.";
            return RedirectToAction("Index", "Acceso");
        }

        private void EnviarCorreoRecuperacion(string correo, string token)
        {
            var fromAddress = new MailAddress("hgarzonvelandia@gmail.com", "Tu Nombre");
            var toAddress = new MailAddress(correo, "Nombre del destinatario");
            const string fromPassword = "neug qjyl wthz gobj";
            const string subject = "Recuperación de Contraseña";
            string body = $"Haz clic en el siguiente enlace para restablecer tu contraseña: " +
                          $"https://localhost:7234/Acceso/ResetPassword?token={token}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        private bool ValidarContraseña(string contraseña)
        {
            // Implementa tus reglas de validación de contraseña aquí
            return contraseña.Length >= 8;
        }

        private bool ValidarTokenYContraseña(string token, string nuevaContraseña, out Usuario usuario)
        {
            // Obtén el usuario directamente en lugar de obtener todos los usuarios
            usuario = _context.Usuarios.FirstOrDefault(u => u.ResetToken == token && DateTime.UtcNow < u.ResetTokenExpiration);

            return usuario != null && ValidarContraseña(nuevaContraseña);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            // Validar el token y mostrar la vista para ingresar la nueva contraseña
            var model = new ResetPassword { Token = token };

            // Obtener el usuario con el token de la base de datos
            if (ValidarTokenYObtenerUsuario(token, out var usuario))
            {
                return View(model);
            }
            else
            {
                // Token no válido, puedes redirigir a una página de error o mostrar un mensaje adecuado
                return RedirectToAction("Index", "Ventas");
            }
        }

        private bool ValidarTokenYObtenerUsuario(string token, out Usuario usuario)
        {
            // Obtén el usuario directamente en lugar de obtener todos los usuarios
            usuario = _context.Usuarios.FirstOrDefault(u => u.ResetToken == token && DateTime.UtcNow < u.ResetTokenExpiration);

            return usuario != null;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                if (ValidarTokenYContraseña(model.Token, model.NuevaContraseña, out var usuario))
                {
                    try
                    {
                        // Restablecer la contraseña y limpiar el token
                        usuario.Contraseña = model.NuevaContraseña;
                        usuario.ResetToken = null;
                        usuario.ResetTokenExpiration = null;

                        // Hash de la nueva contraseña antes de almacenarla en la base de datos

                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", "Acceso");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al restablecer la contraseña: {ex.Message}");
                        Console.WriteLine($"StackTrace: {ex.StackTrace}");
                        Console.WriteLine($"Tipo de excepción: {ex.GetType().FullName}");
                        ModelState.AddModelError(string.Empty, "Se produjo un error al restablecer la contraseña.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Token no válido o contraseña incorrecta.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Modelo invalido ");
            }

            // Si llega aquí, hay errores de validación o de procesamiento, vuelve a mostrar la vista con los errores.
            return View(model);
        }

        public IActionResult Logout()
        {
            // Realiza cualquier lógica de cierre de sesión adicional aquí

            // Sign out al usuario
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige a la página de inicio de sesión o a otra página después de cerrar sesión
            return RedirectToAction("Index", "Acceso");
        }

    }
}