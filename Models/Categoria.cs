using System;
using System.Collections.Generic;

namespace EntreEspeciesNuevo.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string? NomCategoria { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
