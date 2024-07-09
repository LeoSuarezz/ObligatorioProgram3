using System;
using System.Collections.Generic;

namespace ObligatorioProgram3.Models;

public partial class Reseña
{
    public int Id { get; set; }

    public int Puntaje { get; set; }

    public string? Comentario { get; set; }

    public DateOnly FechaReseña { get; set; }

    public int? Idcliente { get; set; }

    public int? Idrestaurante { get; set; }

    public virtual Cliente? IdclienteNavigation { get; set; }

    public virtual Restaurante? IdrestauranteNavigation { get; set; }
}
