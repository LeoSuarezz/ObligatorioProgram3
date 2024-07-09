using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioProgram3.Models
{
    public partial class Reseña
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El puntaje es requerido.")]
        [Range(1, 5, ErrorMessage = "El puntaje debe estar entre 1 y 5.")]
        public int Puntaje { get; set; }

        [Required(ErrorMessage = "El comentario es requerido.")]
        public string? Comentario { get; set; }

        public DateOnly FechaReseña { get; set; }
        public int? Idcliente { get; set; }

        public int? Idrestaurante { get; set; }

        public virtual Cliente? IdclienteNavigation { get; set; }

        public virtual Restaurante? IdrestauranteNavigation { get; set; }
    }
}
