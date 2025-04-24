using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinipet.Dtos;
using Clinipet.Repositories;

namespace Clinipet.Services
{
    public class AdminService
    {
        
        //Obtener lista de veterinarios
        public List<UserDto> ObtenerVeterinarios()
        {
            System.Diagnostics.Debug.WriteLine("Estoy en ObtenerVetePorId");
            AdminRepository adminRepository = new AdminRepository();
            return adminRepository.ObtenerVeterinarios();
        }

        // Método para eliminar un veterinario
        public void EliminVete(int id_usu)
        {
            AdminRepository repo = new AdminRepository();
            repo.EliminVete(id_usu);
        }

        //Obtener lista de asistentes
        public List<UserDto> ObtenerAsistentes()
        {
            System.Diagnostics.Debug.WriteLine("Estoy en ObtenerAsistPorId");
            AdminRepository adminRepository = new AdminRepository();
            return adminRepository.ObtenerVeterinarios();
        }

        // Método para eliminar un asistente
        public void EliminAsis(int id_usu)
        {
            AdminRepository repo = new AdminRepository();
            repo.EliminAsis(id_usu);
        }
    }
}