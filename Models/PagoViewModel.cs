using ObligatorioProgram3.Models;

namespace ObligatorioProgram3.ViewModels
{
    public class PagoViewModel
    {
        public Ordene Orden { get; set; }
        public decimal Descuento { get; set; }
        public decimal MontoConDescuento { get; set; }
        public float Temperatura { get; set; }
        public string DescripcionClima { get; set; }
        public decimal DescuentoCliente { get; set; } // Nuevo campo para descuento del cliente
    }
}

