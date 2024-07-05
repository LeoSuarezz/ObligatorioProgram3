using ObligatorioProgram3.Models;
using System.Collections.Generic;

namespace ObligatorioProgram3.ViewModels
{
    public class MesaViewModel
    {
        public List<Mesa> Mesas { get; set; }
        public List<Menu> MenuItems { get; set; }
    }

    public class OrdenDetailsViewModel
    {
        public Mesa Mesa { get; set; }
        public Ordene Orden { get; set; }
    }
}
