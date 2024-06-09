using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class Clima
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Temperatura { get; set; }

    public string DescripcionClima { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
