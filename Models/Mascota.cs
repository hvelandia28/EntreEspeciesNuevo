using EntreEspeciesNuevo.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models;

public partial class Mascota
{
    public int IdMascota { get; set; }

    public int? DocumentoCliente { get; set; }
    [Required(ErrorMessage = "El Nombre es Obligatorio*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre debe tener entre 3 y 50 caracteres")]
    public string? NombreMascota { get; set; }
    [Required(ErrorMessage = "La Fecha es Obligatoria*")]
    public DateTime? FechaNacimiento { get; set; }
    [Required(ErrorMessage = "El Color es Obligatorio*")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "El Color debe tener entre 3 y 20 caracteres")]
    public string? ColorMascota { get; set; }
    [Required(ErrorMessage = "La Especie es Obligatoria*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "La Especie debe tener entre 3 y 50 caracteres")]
    public string? Especie { get; set; }
    [Required(ErrorMessage = "La Raza es Obligatoria*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "La Raza debe tener entre 3 y 50 caracteres")]
    public string? Raza { get; set; }

    public string? Genero { get; set; }

    public string? InfMascota { get; set; }
    public virtual ICollection<CitaExterna> CitaExternas { get; } = new List<CitaExterna>();

    public virtual ICollection<CitaInterna> CitaInternas { get; } = new List<CitaInterna>();
    public virtual Cliente? DocumentoClienteNavigation { get; set; }
}
