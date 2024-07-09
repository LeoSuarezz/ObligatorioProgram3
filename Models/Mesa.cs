using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioProgram3.Models;

public partial class Mesa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El número de mesa es requerido.")]
    public int NumeroMesa { get; set; }

    [Required(ErrorMessage = "La capacidad de la mesa es requerida.")]
    public int Capacidad { get; set; }

    [Required(ErrorMessage = "El estado de la mesa es requerido.")]
    public string Estado { get; set; } = null!;

    public int? Idrestaurante { get; set; }

    public virtual Restaurante? IdrestauranteNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
