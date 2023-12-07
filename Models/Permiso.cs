using System;
using System.Collections.Generic;

namespace EntreEspeciesNuevo.Models;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    public string? NomPermiso { get; set; }

    public virtual ICollection<Configuracion> Configuracions { get; set; } = new List<Configuracion>();
}
