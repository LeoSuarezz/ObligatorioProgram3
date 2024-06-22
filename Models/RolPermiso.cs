using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class RolPermiso
{
    public int? IdRol { get; set; }

    public int? IdPermisos { get; set; }

    public virtual Rol Rol { get; set; }
    public virtual Permiso Permiso { get; set; }
}
