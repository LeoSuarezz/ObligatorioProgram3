using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class Permiso
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Rol> IdRols { get; set; } = new List<Rol>();

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();

}
