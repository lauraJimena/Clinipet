﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinipet.Dtos;
using Clinipet.Services;
using Clinipet.Repositories;

namespace Clinipet.Services
{
    public class AsistenteService
    {
        //Metodo Registro de asistente
        public UserDto RegistrarAsistente(UserDto nuevoAsist)
        {
            GeneralRepository userRepository = new GeneralRepository();
            UserDto userResponse = new UserDto();
            Console.WriteLine("Estoy en registro de asistente");

            //Validar datos
            try
            {
                nuevoAsist.id_rol = 2;
                nuevoAsist.id_nivel = 1;
                nuevoAsist.id_estado = 1;

                if (userRepository.ExisteCorreo(nuevoAsist.correo_usu))
                {
                    userResponse.Response = -1;
                    userResponse.Mensaje = "El correo ingresado ya existe";
                }
                else
                {
                    //Que no se repita el documento
                    if (userRepository.RegistrarUsuario(nuevoAsist) != 0)
                    {
                        userResponse.Response = 1;
                        userResponse.Mensaje = "Asistente registrado con exito";
                    }
                    else
                    {
                        userResponse.Response = 0;
                        userResponse.Mensaje = "Ocurrió un error al registrar asistente";
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
    }
}