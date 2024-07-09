using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioProgram3.Models
{
    public partial class Permiso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del permiso es requerido.")]
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Rol> IdRols { get; set; } = new List<Rol>();

        public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    }
}
