using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinipet.Dtos;
using Clinipet.Services;
using Clinipet.Repositories;

namespace Clinipet.Services
{
    public class VeterinarioService
    {
        //Metodo Registro de veterinatio
        public UserDto RegistrarVeterinario(UserDto nuevoVete)
        {
            GeneralRepository userRepository = new GeneralRepository();
            UserDto userResponse = new UserDto();
            Console.WriteLine("Estoy en registro de veterinario");

            //Validar datos
            try
            {
                nuevoVete.id_rol = 4;
                nuevoVete.id_espec = 1; //Para medicina general (prueba)
                nuevoVete.id_nivel = 1;
                nuevoVete.id_estado = 1;

                if (userRepository.ExisteCorreo(nuevoVete.correo_usu))
                {
                    userResponse.Response = -1;
                    userResponse.Mensaje = "El correo ingresado ya existe";
                }
                else
                {
                    //Que no se repita el documento
                    if (userRepository.RegistrarUsuario(nuevoVete) != 0)
                    {
                        userResponse.Response = 1;
                        userResponse.Mensaje = "Veterinario registrado con exito";
                    }
                    else
                    {
                        userResponse.Response = 0;
                        userResponse.Mensaje = "Ocurrió un error al registrar veterinario";
                    }
                }
                return userResponse;
            }
            catch (Exception e)
            {
                userResponse.Response = 0;
                userResponse.Mensaje = e.InnerException?.ToString() ?? e.Message;
                return userResponse;
            }

        }


        public DisponibDto PublicarDisponibilidad(DisponibDto dispon)
        {
            //GeneralRepository userRepository = new GeneralRepository();

            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            DisponibDto disponibResponse = new DisponibDto();
            try
            {


                if (veterinarioRepository.PublicarDispon(dispon) != 0)
                {
                    disponibResponse.Response = 1;
                    disponibResponse.Mensaje = "Cita publicada correctamente";

                }
            }
            catch (Exception e)
            {
                disponibResponse.Response = 0;
                disponibResponse.Mensaje = e.InnerException?.ToString() ?? e.Message;
                return disponibResponse;
            }
            return disponibResponse;

        }
    }
}