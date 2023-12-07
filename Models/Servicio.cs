using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models
{
    public partial class Servicio
    {
        public int IdServicio { get; set; }

        [Required(ErrorMessage = "El campo Nombre del Servicio es obligatorio.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "El Nombre del Servicio debe tener al menos 4 caracteres.")]
        [RegularExpression("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ ]+$", ErrorMessage = "El Nombre del Servicio solo debe contener letras.")]
        public string NomServico { get; set; }

        [Required(ErrorMessage = "El campo Categoría es obligatorio.")]
        [RegularExpression("^(Belleza|Medicinal)$", ErrorMessage = "La Categoría debe ser 'Belleza' o 'Medicinal'.")]
        public string? Categoria { get; set; }

        [Required(ErrorMessage = "El campo Precio es obligatorio.")]
        [Range(1000, 999999, ErrorMessage = "El Precio debe estar entre 1000 y 999999.")]
        public int? Precio { get; set; }

        public virtual ICollection<DetaVentum> DetaVenta { get; set; } = new List<DetaVentum>();
    }
}
