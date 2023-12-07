using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models;

public partial class Proveedore
{
    public int IdProveedor { get; set; }

    public int? NitProveedor { get; set; }
    [Required]
    public string? NomProveedor { get; set; }

    public string? Correo { get; set; }
    [Required(ErrorMessage = "El Telefono es Obligatorio*")]
    [Range(0, 3239999999)]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "El campo Telefono debe tener exactamente 10 dígitos.")]
    public int? Telefono { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
