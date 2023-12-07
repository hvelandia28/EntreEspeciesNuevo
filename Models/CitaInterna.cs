using EntreEspeciesNuevo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.models;

public partial class CitaInterna
{
    public int IdCitaInt { get; set; }

    public int? IdMascota { get; set; }
    [Required(ErrorMessage = "El Documento es Obligatorio*")]
    [Range(1000000, 999999999999999, ErrorMessage = "El N° Documento debe tener entre 7 y 10 dígitos")]
    public int? DocumentoCliente { get; set; }
    [Required(ErrorMessage = "El Nombre es Obligatorio*")]
    public string? NomCita { get; set; }
    [Required(ErrorMessage = "La Fecha es Obligatoria*")]

    public DateTime? FechaHora { get; set; }

    public string? Estado { get; set; }
    [Required(ErrorMessage = "El Precio es Obligatorio*")]

    public int? Precio { get; set; }

    public virtual Cliente? DocumentoClienteNavigation { get; set; }

    public virtual Mascota? IdMascotaNavigation { get; set; }
}

// epit-pdf
