using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntreEspeciesNuevo.Models;

public partial class DetaCompra
{
    public int IdDetaCompra { get; set; }

    public int IdCompra { get; set; }

    public int? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public int PrecioCompra { get; set; }

    public int Subtotal { get; set; }
    [NotMapped]
    public string? ProductoNombre { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
    [NotMapped]
    public decimal TotalDetalle => PrecioCompra ;
}
