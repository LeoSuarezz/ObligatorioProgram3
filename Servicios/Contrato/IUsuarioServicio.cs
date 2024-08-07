﻿using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;

namespace ObligatorioProgram3.Servicios.Contrato
{
    public interface IUsuarioServicio
    {

        Task<Usuario> GetUsuario(string email, string contrasena);
        Task<Usuario> GetUsuarioById(int id);
        Task<List<string>> ObtenerPermisosPorRol(int idRol);

    }
}
