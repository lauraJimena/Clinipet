using Clinipet.Dtos;
using Clinipet.Repositories;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//En GeneralService van todos los metodos en comun entre roles
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
                userModel.cambio_contras = false; // No necesita cambiar contraseña


                if (userRepository.ExisteCorreo(userModel.correo_usu))
                {
                    responseUserDto.Response = -1;                   
                    responseUserDto.Mensaje = "El correo ya existe. Intente nuevamente.";
                  
                }
                else
                {
                    if (userRepository.ExisteDocumento(userModel.num_ident))
                    {
                        responseUserDto.Response = -2;
                        responseUserDto.Mensaje = "El numero de documento ya existe. Intente nuevamente.";
                       
                    }
                    else
                    {
                        if (userRepository.RegistrarUsuario(userModel) != 0)
                        {
                            responseUserDto.Response = 1;

                            // Enviar correo en segundo plano
                            Task.Run(() =>
                            {
                                try
                                {
                                    EmailConfigUtility gestorCorreo = new EmailConfigUtility();
                                    String destinatario = userModel.correo_usu;
                                    String asunto = "Bienvenido al sistema de CliniPet!";
                                    gestorCorreo.EnviarCorreoBienv(destinatario, asunto, userModel);
                                }
                                catch (Exception ex)
                                {
                                    // Aquí puedes loguear el error o tomar acciones, pero no debe afectar el registro
                                    Console.WriteLine("Error al enviar el correo: " + ex.Message);
                                }
                            });
                        }
                        else
                        {
                            responseUserDto.Response = 0;
                            responseUserDto.Mensaje = "Algo pasó";
                        }
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
        public MascotaDto RegistrarMascota(MascotaDto mascModel)
        {

            MascotaDto responseMascDto = new MascotaDto();
            GeneralRepository mascRepository = new GeneralRepository();
            Console.WriteLine("Estoy en el servicio");
            try
            {                           
                
                    if (mascRepository.RegistrarMascota(mascModel) != 0)
                    {
                        responseMascDto.Response = 1;
                        responseMascDto.Mensaje = "Creación exitosa";

                    }
                    else
                    {
                        responseMascDto.Response = 0;
                        responseMascDto.Mensaje = "Algo pasó";
                    }

                
                return responseMascDto;
            }
            catch (Exception e)
            {
                responseMascDto.Response = 0;
                responseMascDto.Mensaje = e.InnerException?.ToString();
                return responseMascDto;
            }
        }
        public List<DisponibDto> ObtenerCitasDispon()
        {
            GeneralRepository generalRepository = new GeneralRepository();
            List<DisponibDto> citasDispon = generalRepository.ObtenerCitasEspecDispon();

            return citasDispon;
            
        }
       
        public List<MascotaDto> ObtenerRazas()

        {

            GeneralRepository generalRepository = new GeneralRepository();
            List<MascotaDto> razas = generalRepository.ObtenerRazas();

            return razas;
        }


        public List<SelectListItem> ObtenerRazasSelect()
        {
            GeneralRepository generalRepository = new GeneralRepository();
            List<MascotaDto> razas = generalRepository.ObtenerRazas();

            return razas.Select(r => new SelectListItem // Convertir la lista raza en SelectList para pasar a la vista
            {
                Value = r.id_raza.ToString(),
                Text = r.nom_raza,
                Group = new SelectListGroup { Name = r.nom_tipo }
            }).ToList();
        }

        public List<MascotaDto> ObtenerTipos()

        {

            GeneralRepository generalRepository = new GeneralRepository();
            List<MascotaDto> tiposDisponibles = generalRepository.ObtenerTipos();
            return tiposDisponibles;
        }
        public List<SelectListItem> ObtenerTiposSelect()
        {
            GeneralRepository generalRepository = new GeneralRepository();
            List<MascotaDto> tipos = generalRepository.ObtenerTipos(); 

            return tipos.Select(t => new SelectListItem
            {
                Value = t.id_tipo.ToString(),
                Text = t.nom_tipo
            }).ToList();
        }


        public bool CambiarContraseña(string numIdent, string contrasenaActual, string nuevaContrasena)
        {
            GeneralRepository contrasRepository = new GeneralRepository();
            try
            {
                return contrasRepository.CambiarContraseña(numIdent, contrasenaActual, nuevaContrasena);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar la contraseña: " + ex.Message);
            }
        }

    }
}