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
                nuevoAsist.id_espec = 1;     // No aplica
                nuevoAsist.cambio_contras = true; // Requiere cambiar contraseña al iniciar

                // Validaciones
                if (AsistRepo.ExisteCorreo(nuevoAsist.correo_usu))
                {
                    userResponse.Response = -1;
                    userResponse.Mensaje = "El correo ya existe. Intente nuevamente.";
                }
                else
                {
                    if (AsistRepo.ExisteDocumento(nuevoAsist.num_ident))
                    {
                        userResponse.Response = -2;
                        userResponse.Mensaje = "El numero de documento ya existe. Intente nuevamente.";
                    }
                    else
                    {
                        if (AsistRepo.RegistrarUsuario(nuevoAsist) != 0)
                        {
                            userResponse.Response = 1;
                            userResponse.Mensaje = "Asistente registrado exitosamente.";

                        }
                        else
                        {
                            userResponse.Response = 0;
                            userResponse.Mensaje = "Algo salió mal durante el registro.";
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
