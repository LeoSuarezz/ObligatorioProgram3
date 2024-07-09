using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ObligatorioProgram3.Models;

public partial class Menu
{

    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del plato es requerido.")]
    public string NombrePlato { get; set; } = null!;

    [Required(ErrorMessage = "La descripción del plato es requerida.")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "El precio del plato es requerido.")]
    public decimal Precio { get; set; }

    public string? RutaImagen { get; set; }

    public string? Categoria { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();

}
