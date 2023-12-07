using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models
{
    public class AccesoViewModels
    {
        public class OlvidoContraseñaViewModel
        {
            [Required(ErrorMessage = "El campo correo es obligatorio.")]
            [EmailAddress(ErrorMessage = "Por favor, ingresa un correo electrónico válido.")]
            public string Correo { get; set; }
        }
    }
}
