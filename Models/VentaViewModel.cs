using Microsoft.AspNetCore.Mvc.Rendering;

namespace EntreEspeciesNuevo.Models
{
    public class VentaViewModel
    {
        public int VentaId { get; set; }
        public SelectList Productos { get; set; }
        public SelectList Servicios { get; set; }
        public SelectList Ventas { get; set; }
        public List<DetaVentum> DetallesVentaExistente { get; set; }
    }
}
