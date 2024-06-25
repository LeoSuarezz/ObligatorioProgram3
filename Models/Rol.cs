using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Permiso> IdPermisos { get; set; } = new List<Permiso>();

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();

}
