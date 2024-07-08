using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObligatorioProgram3.Models;

public partial class Clima
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public float Temperatura { get; set; }

    public string DescripcionClima { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
