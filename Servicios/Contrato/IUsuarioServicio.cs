using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;

namespace ObligatorioProgram3.Servicios.Contrato
{
    public interface IUsuarioServicio
    {

        Task<Usuario> GetUsuario(string email, string contrasena);
    }
}
