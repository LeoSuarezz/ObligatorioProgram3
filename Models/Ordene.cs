using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioProgram3.Models
{
    public partial class Ordene
    {
        public int Id { get; set; }

        public int? Idreserva { get; set; }

        public decimal? Total { get; set; }

        public string? Estado { get; set; }

        public virtual Reserva? IdreservaNavigation { get; set; }

        public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
    }
}
