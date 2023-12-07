using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntreEspeciesNuevo.Models
{
    public partial class DetaVentum
    {
        public int IdDetalleV { get; set; }

        public int IdVenta { get; set; }

        public int? IdServicio { get; set; }

        public int? IdProducto { get; set; }

        public int? Cantidad { get; set; }

        public int? SubTotalPro { get; set; }

        public int? SubTotalSer { get; set; }

        [NotMapped]
        public string? ProductoNombre { get; set; }

        // Propiedad para almacenar los detalles existentes
        [NotMapped]
        


        public virtual Producto? IdProductoNavigation { get; set; }

        public virtual Servicio? IdServicioNavigation { get; set; }

        public virtual Venta? IdVentaNavigation { get; set; }
    }

    
}