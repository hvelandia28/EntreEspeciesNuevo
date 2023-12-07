using System;
using System.Collections.Generic;

namespace EntreEspeciesNuevo.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string? NomRol { get; set; }

    public virtual ICollection<Configuracion> Configuracions { get; set; } = new List<Configuracion>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
