using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;
using ObligatorioProgram3.Servicios.Contrato;
using System.Security.Claims;

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

        public async Task<Usuario> GetUsuarioById(int id)
        {
            Usuario usuarioEncontrado = await _dbContext.Usuarios.Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            return usuarioEncontrado;
        }
        public async Task<List<string>> ObtenerPermisosPorRol(int idRol)
        {
            // Lógica para obtener permisos por el idRol
            var permisos = await _dbContext.Rol
                                .Where(r => r.Id == idRol)
                                .SelectMany(r => r.RolPermisos)
                                .Select(rp => rp.Permiso.Nombre)
                                .ToListAsync();

            return permisos;
        }
    }
}
