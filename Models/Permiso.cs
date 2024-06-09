using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class Permiso
{
    public int Id { get; set; }

    public int? Idrol { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual Rol? IdrolNavigation { get; set; }
}
