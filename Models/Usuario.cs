using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioProgram3.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public int? Idrol { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "El apellido no puede tener más de 100 caracteres")]
    public string Apellido { get; set; } = null!;


    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email no válido")]
    [StringLength(100, ErrorMessage = "El email no puede tener más de 100 caracteres")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, ErrorMessage = "La contraseña no puede tener más de 100 caracteres")]
    public string Contraseña { get; set; } = null!;

    public virtual Rol? IdrolNavigation { get; set; }
}
