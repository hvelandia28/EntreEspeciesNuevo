using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntreEspeciesNuevo.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int? IdRol { get; set; }
    [Required(ErrorMessage = "El Nombre es Obligario*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre debe tener entre 3 y 50 caracteres")]
    public string? Nombre { get; set; }

    public string? TipoDoc { get; set; }
    [Required(ErrorMessage = "El Documento es Obligatorio*")]
    [Range(1000000, 999999999999999, ErrorMessage = "El N° Documento debe tener entre 7 y 10 dígitos")]
    public int? Documento { get; set; }

    public int? Telefono { get; set; }
    [Required(ErrorMessage = "El Correo es Obligatorio*")]
    [EmailAddress(ErrorMessage = "El Correo no contiene un formato válido")]
    public string? Correo { get; set; }
    [Required(ErrorMessage = "La Contraseña es Obligatoria*")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "La Contraseña debe tener entre 4 y 50 caracteres")]
    public string? Contraseña { get; set; }
    
    public string? Estado { get; set; }
    [Required(ErrorMessage = "La Dirección es Obligatoria*")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "La Dirección debe tener entre 4 y 100 caracteres")]
    public string? Direccion { get; set; }
    public virtual Role? IdRolNavigation { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiration { get; set; }
}
