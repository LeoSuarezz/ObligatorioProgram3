using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioProgram3.Models
{
    public partial class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede tener más de 100 caracteres")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email no válido.")]
        [StringLength(100, ErrorMessage = "El email no puede tener más de 100 caracteres")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de cliente es obligatorio")]
        [StringLength(50, ErrorMessage = "El tipo de cliente no puede tener más de 50 caracteres")]
        public string TipoCliente { get; set; } = null!;

        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

        public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
    }
}