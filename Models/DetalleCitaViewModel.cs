using EntreEspeciesNuevo.models;

namespace EntreEspeciesNuevo.Models
{
    public class DetalleCitaViewModel
    {
        public CitaExterna CitaExterna { get; set; }
        public Mascota Mascota { get; set; }
        public Cliente Cliente { get; set; }
        public CitaInterna CitaInterna { get; set; }
    }
}
