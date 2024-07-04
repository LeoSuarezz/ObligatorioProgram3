using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class OrdenDetalle
{
    public int Id { get; set; }

    public int Cantidad { get; set; }

    public int? Idorden { get; set; }

    public int? Idmenu { get; set; }

    public virtual Menu? IdmenuNavigation { get; set; }

    public virtual Ordene? IdordenNavigation { get; set; }

}
