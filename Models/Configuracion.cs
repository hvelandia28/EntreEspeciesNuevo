using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntreEspeciesNuevo.Models;

public partial class Configuracion
{
    public int IdConfiguracion { get; set; }

    public int? IdRol { get; set; }

    public int? IdPermiso { get; set; }
    [NotMapped]

    public List<Permiso>? Permisos { get; set; }
    public Configuracion()
    {
        Permisos = new List<Permiso>();
    }
    public virtual Permiso? IdPermisoNavigation { get; set; }

    public virtual Role? IdRolNavigation { get; set; }
}
