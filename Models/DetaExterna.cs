using System;
using System.Collections.Generic;

namespace EntreEspeciesNuevo.Models;

public partial class DetaExterna
{
    public int IdCitaExt { get; set; }

    public int? IdMascota { get; set; }

    public int? DocumentoCliente { get; set; }


    public virtual Cliente? DocumentoClienteNavigation { get; set; }

    public virtual Mascota? IdMascotaNavigation { get; set; }
}
