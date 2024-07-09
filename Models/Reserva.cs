using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioProgram3.Models
{
    public partial class Reserva
    {
        public int Id { get; set; }

        public int? Idcliente { get; set; }

        public int? Idmesa { get; set; }

        [Required(ErrorMessage = "La fecha de reserva es requerida.")]
        public DateOnly FechaReserva { get; set; }

        [Required(ErrorMessage = "El estado de la reserva es requerido.")]
        public string Estado { get; set; } = null!;

        public virtual Cliente? IdclienteNavigation { get; set; }

        public virtual Mesa? IdmesaNavigation { get; set; }

        public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();

        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
