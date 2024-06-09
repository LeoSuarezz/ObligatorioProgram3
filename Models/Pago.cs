using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class Pago
{
    public int Id { get; set; }

    public int? Idreserva { get; set; }

    public decimal Monto { get; set; }

    public DateOnly FechaPago { get; set; }

    public int? Idcotizacion { get; set; }

    public string MetodoPago { get; set; } = null!;

    public int? Idclima { get; set; }

    public virtual Clima? IdclimaNavigation { get; set; }

    public virtual Cotizacion? IdcotizacionNavigation { get; set; }

    public virtual Reserva? IdreservaNavigation { get; set; }
}
