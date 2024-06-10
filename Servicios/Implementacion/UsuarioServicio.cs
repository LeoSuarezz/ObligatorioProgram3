using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.Servicios.Contrato;

namespace ObligatorioProgram3.Servicios.Implementacion
{
    public class UsuarioServicio : IUsuarioServicio
    {

        private readonly ObligatorioProgram3Context _dbContext;

        public UsuarioServicio(ObligatorioProgram3Context _context){
            _dbContext = _context;
        }


        public async Task<Usuario> GetUsuario(string email, string contrasena)
        {
            Usuario usuarioEncontrado = await _dbContext.Usuarios.Where(u=>u.Email == email && u.Contraseña==contrasena)
                .FirstOrDefaultAsync();

            return usuarioEncontrado;
        }
    }
}
