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
                else if (userRepository.ExisteDocumento(userModel.num_ident))
                {
                    responseUserDto.Response = -2;
                    responseUserDto.Mensaje = "El número de documento ya existe. Intente nuevamente.";
                }
                else
                {
                    // Se encripta la contraseña
                    userModel.contras_usu = EncriptContrasUtility.EncripContras(userModel.contras_usu);

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
            UserDto userResponse = userRepository.Login(userModel.num_ident);
            System.Diagnostics.Debug.WriteLine("Llegué al servicio");

            // Si se encontro el usuuaro entonces 1
            if (userResponse.Response == 1)
            {
                // Verificar la contraseña ingresada contra la guardada
                if (EncriptContrasUtility.VerificaContras(userModel.contras_usu, userResponse.contras_usu))
                {
                    userResponse.Mensaje = "Inicio de sesión exitoso";
                }
                else
                {
                    userResponse.Response = 0;
                    userResponse.Mensaje = "Contraseña incorrecta";
                }
            }
            else
            {
                userResponse.Mensaje = "Error en el inicio de sesión, número de documento no encontrado";
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


        public bool CambiarContraseña(string numIdent, string contrasenaActual, string nuevaContrasena, string confirmar_contras)
        {
            GeneralRepository contrasRepository = new GeneralRepository();
            try
            {
                // Validación prevenir que la nueva contraseña sea igual a la actual
                if (contrasenaActual == nuevaContrasena)
                {
                    throw new Exception("La nueva contraseña no puede ser igual a la anterior.");
                }
                if(confirmar_contras != nuevaContrasena)
                {
                    throw new Exception("Las contraseñas no coinciden");
                }
                // Ejecutar el cambio de contraseña (incluye la validación dentro del repositorio)
                bool cambioExitoso = contrasRepository.CambiarContraseña(numIdent, contrasenaActual, nuevaContrasena);

                return cambioExitoso;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar la contraseña: " + ex.Message);
            }
        }
        public UserDto BuscarPorIdentidad(string num_ident)
        {                 
            try
            {

                GeneralRepository repo = new GeneralRepository();
                return repo.ObtenerUsuarioPorIdentidad(num_ident);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar la contraseña: " + ex.Message);
            }
        }
       
        public void EnviarCorreoRestablecimiento(UserDto usu)
        {
            GeneralRepository repo = new GeneralRepository();
            //UserDto usuario = repo.BuscarPorCorreo(correo);

            if (usu != null)
            {
                string token = Guid.NewGuid().ToString();
                DateTime expiracion = DateTime.Now.AddHours(1);

                // Guardar el token en la base de datos
                repo.GuardarTokenRecuperacion(usu.id_usu, token, expiracion);


                //Enviar Correo
                EmailConfigUtility gestorCorreo = new EmailConfigUtility();
                String destinatario = usu.correo_usu;
                String asunto = "Restablecimiento de Contraseña";
                gestorCorreo.EnviarCorreoRestablecimiento(destinatario, asunto, usu, token);
            }
        }
        public bool RestablecerContrasena(int idUsuario, string nuevaContrasena)
        {
            GeneralRepository repo = new GeneralRepository();
            string hashContrasena = EncriptContrasUtility.EncripContras(nuevaContrasena); //PBKDF2
            int resultado = repo.RestablecerContrasena(idUsuario, hashContrasena);
            return resultado > 0;
        }
        public int? ObtenerIdUsuarioPorToken(string token)
        {
            GeneralRepository repo = new GeneralRepository();
            return repo.ObtenerIdUsuarioPorToken(token);
        }








    }
}