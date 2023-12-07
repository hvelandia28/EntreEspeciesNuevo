using System;
using System.Collections.Generic;

namespace EntreEspeciesNuevo.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int? IdProveedor { get; set; }

    public int? IdCategoria { get; set; }

    public string? NomProducto { get; set; }

    public int Disponibilidad { get; set; }
    public int? Cantidad { get; set; }

    public DateTime? FechaVen { get; set; }

    public int Precio { get; set; }

    public virtual ICollection<DetaCompra> DetaCompras { get; set; } = new List<DetaCompra>();

    public virtual ICollection<DetaVentum> DetaVenta { get; set; } = new List<DetaVentum>();

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual Proveedore? IdProveedorNavigation { get; set; }
}
