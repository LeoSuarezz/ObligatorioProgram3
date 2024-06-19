using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();


    public static bool getPermisos(Rol rol, string permiso)
    {
        
        foreach(var p in rol.Permisos)
        {
            if (p.Nombre.ToLower() == permiso.ToLower())
            {
                return true;
            }
        }
        return false;
    }
}
