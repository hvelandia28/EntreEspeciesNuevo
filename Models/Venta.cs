using System;
using System.Collections.Generic;

namespace EntreEspeciesNuevo.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int? DocumentoCliente { get; set; }

    public DateTime? FechaVenta { get; set; }

    public string? FormaPago { get; set; }

    public int? Total { get; set; }
    public virtual ICollection<DetaVentum> DetaVenta { get; set; } = new List<DetaVentum>();

    public virtual Cliente? DocumentoClienteNavigation { get; set; }
}
