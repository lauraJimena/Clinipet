using Clinipet.Dtos;
using Clinipet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Services
{
    public class GeneralService
    {
        public UserDto RegistrarCliente(UserDto userModel)
        {

            UserDto responseUserDto = new UserDto();
            GeneralRepository userRepository = new GeneralRepository();
            Console.WriteLine("Estoy en el servicio");


            try
            {
                userModel.id_rol = 3;
                userModel.id_espec = 1;
                userModel.id_nivel = 1;
                userModel.id_estado = 1;


                if (userRepository.ExisteCorreo(userModel.correo_usu))
                {
                    responseUserDto.Response = -1;
                    responseUserDto.Mensaje = "Correo ya existe";
                }
                else
                {
                    if (userRepository.RegistrarUsuario(userModel) != 0 )
                    {
                        responseUserDto.Response = 1;
                        responseUserDto.Mensaje = "Creación exitosa";

                    }
                    else
                    {
                        responseUserDto.Response = 0;
                        responseUserDto.Mensaje = "Algo pasó";
                    }
                    
                }
                return responseUserDto;
            }
            catch (Exception e)
            {
                responseUserDto.Response = 0;
                responseUserDto.Mensaje = e.InnerException?.ToString();
                return responseUserDto;
            }
        }
        //Aqui va metodo RegistrarAsistente
        public UserDto Login(UserDto userModel)
        {
            GeneralRepository userRepository = new GeneralRepository();
            UserDto userResponse = userRepository.Login(userModel);
            System.Diagnostics.Debug.WriteLine("Llegué al servicio");
            // Si se encontro el usuuaro entonces 1
            if (userResponse.Response == 1)
            {
                userResponse.Mensaje = "Inicio de sesión exitoso";
            }
            else
            {
                userResponse.Mensaje = "Error en el inicio de sesión, nombre de usuario o contraseña incorrectos";
            }
            return userResponse;
        }

    }        
}