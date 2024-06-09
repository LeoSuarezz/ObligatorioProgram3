using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class Cotizacion
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    public string Moneda { get; set; } = null!;

    public decimal Monto { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
