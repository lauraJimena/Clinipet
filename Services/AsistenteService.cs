using System;
using Clinipet.Dtos;
using Clinipet.Repositories;

namespace Clinipet.Services
{
    public class AsistenteService
    {
        public UserDto RegistrarAsistente(UserDto nuevoAsist)
        {
            AdminRepository adminRepo = new AdminRepository(); 
            UserDto userResponse = new UserDto();

            try
            {
                Console.WriteLine("Estoy en registro de asistente");

                // Seteo de datos por defecto
                nuevoAsist.id_rol = 2;        // Asistente
                nuevoAsist.id_nivel = 1;      // Nivel básico
                nuevoAsist.id_estado = 1;     // Activo
                nuevoAsist.id_espec = 10;     // No aplica

                System.Diagnostics.Debug.WriteLine($"Nombre: {nuevoAsist.nom_usu}, Apellido: {nuevoAsist.apel_usu}, ID Tipo Doc: {nuevoAsist.id_tipo_ident}, Especialidad: {nuevoAsist.id_espec}");

                // Validaciones
                if (adminRepo.ExisteCorreo(nuevoAsist.correo_usu))
                {
                    userResponse.Response = -1;
                    userResponse.Mensaje = "El correo ingresado ya existe";
                }
                else if (adminRepo.ExisteDocumento(nuevoAsist.num_ident))
                {
                    userResponse.Response = -2;
                    userResponse.Mensaje = "El número de documento ya está registrado";
                }
                else
                {
                    if (adminRepo.RegistrarAsistente(nuevoAsist) != 0)
                    {
                        userResponse.Response = 1;
                        userResponse.Mensaje = "Asistente registrado con éxito";
                    }
                    else
                    {
                        userResponse.Response = 0;
                        userResponse.Mensaje = "Ocurrió un error al registrar asistente";
                    }
                }
            }
            catch (Exception e)
            {
                userResponse.Response = 0;
                userResponse.Mensaje = $"Error interno: {e.Message}";
            }

            return userResponse;
        }
    }
}
