using System;
using System.Collections.Generic;

namespace EntreEspeciesNuevo.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public DateTime? FechaCompra { get; set; }

    public string? FormaPago { get; set; }
    public int Total { get; set; }

    public virtual ICollection<DetaCompra> DetaCompras { get; set; } = new List<DetaCompra>();
}
