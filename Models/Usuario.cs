using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public int? Idrol { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public virtual Rol? IdrolNavigation { get; set; }

  
}
