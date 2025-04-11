using System;
using Clinipet.Dtos;
using Clinipet.Repositories;

namespace Clinipet.Services
{
    public class AsistenteService
    {
        public UserDto RegistrarAsistente(UserDto nuevoAsist)
        {
            GeneralRepository AsistRepo = new GeneralRepository(); 
            UserDto userResponse = new UserDto();

            try
            {
                Console.WriteLine("Estoy en registro de asistente");

                // Seteo de datos por defecto
                nuevoAsist.id_rol = 2;        // Asistente
                nuevoAsist.id_nivel = 1;      // Nivel básico
                nuevoAsist.id_estado = 1;     // Activo
                nuevoAsist.id_espec = 10;     // No aplica

                //System.Diagnostics.Debug.WriteLine($"Nombre: {nuevoAsist.nom_usu}, Apellido: {nuevoAsist.apel_usu}, ID Tipo Doc: {nuevoAsist.id_tipo_ident}, Especialidad: {nuevoAsist.id_espec}");

                // Validaciones
                if (AsistRepo.ExisteCorreo(nuevoAsist.correo_usu))
                {
                    userResponse.Response = -1;
                    userResponse.Mensaje = "El correo ya existe. Intente nuevamente.";
                    //userResponse.Mensaje = "Asistente registrado con éxito";
                }
                else
                {
                    if (AsistRepo.ExisteDocumento(nuevoAsist.num_ident))
                    {
                        userResponse.Response = -2;
                        userResponse.Mensaje = "El numero de documento ya existe. Intente nuevamente.";
                        //userResponse.Mensaje = "Asistente registrado con éxito";
                    }
                    else
                    {
                        if (AsistRepo.RegistrarUsuario(nuevoAsist) != 0)
                        {
                            userResponse.Response = 1;
                            userResponse.Mensaje = "Creación exitosa";

                        }
                        else
                        {
                            userResponse.Response = 0;
                            userResponse.Mensaje = "Algo pasó";
                        }
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
