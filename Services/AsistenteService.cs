using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Clinipet.Dtos;
using Clinipet.Repositories;
using Clinipet.Utilities;

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
                            //Enviar Correo Bienvenida
                            EmailConfigUtility gestorCorreo = new EmailConfigUtility();
                            String destinatario = nuevoAsist.correo_usu;
                            String asunto = "Bienvenido al sistema de CliniPet!";
                            gestorCorreo.EnviarCorreoBienv(destinatario, asunto, nuevoAsist);
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

        public UserDto RegistrarUsuario(UserDto userModel)
        {
            UserDto responseUserDto = new UserDto();
            AsistenteRepository userRepository = new AsistenteRepository();

            try
            {
                userModel.id_rol = 3;
                userModel.id_espec = 1;
                userModel.id_nivel = 1;
                userModel.id_estado = 1;
                userModel.cambio_contras = false;

                int resultado = userRepository.RegistrarUsuario(userModel);

                switch (resultado)
                {
                    case 1:
                        responseUserDto.Response = 1;
                        responseUserDto.Mensaje = "Creación exitosa";
                        break;
                    case -1:
                        responseUserDto.Response = -1;
                        responseUserDto.Mensaje = "El correo ya existe. Intente nuevamente.";
                        break;
                    case -2:
                        responseUserDto.Response = -2;
                        responseUserDto.Mensaje = "El número de documento ya existe. Intente nuevamente.";
                        break;
                    default:
                        responseUserDto.Response = 0;
                        responseUserDto.Mensaje = "Ocurrió un error inesperado.";
                        break;
                }

                return responseUserDto;
            }
            catch (Exception e)
            {
                responseUserDto.Response = 0;
                responseUserDto.Mensaje = e.InnerException?.ToString() ?? e.Message;
                return responseUserDto;
            }
        }

        public MascotaDto RegistrarMascota(MascotaDto mascModel)
        {

            MascotaDto responseMascDto = new MascotaDto();
            AsistenteRepository mascRepository = new AsistenteRepository();
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


        public List<MascotaDto> ObtenerRazas()

        {

            AsistenteRepository asistenteRepository = new AsistenteRepository();
            List<MascotaDto> razas = asistenteRepository.ObtenerRazas();

            return razas;
        }


        public List<SelectListItem> ObtenerRazasSelect()
        {
            AsistenteRepository asistenteRepository = new AsistenteRepository();
            List<MascotaDto> razas = asistenteRepository.ObtenerRazas();

            return razas.Select(r => new SelectListItem // Convertir la lista raza en SelectList para pasar a la vista
            {
                Value = r.id_raza.ToString(),
                Text = r.nom_raza,
                Group = new SelectListGroup { Name = r.nom_tipo }
            }).ToList();
        }

        public List<MascotaDto> ObtenerTipos()

        {

            AsistenteRepository asistenteRepository = new AsistenteRepository();
            List<MascotaDto> tiposDisponibles = asistenteRepository.ObtenerTipos();
            return tiposDisponibles;
        }
        public List<SelectListItem> ObtenerTiposSelect()
        {
            AsistenteRepository asistenteRepository = new AsistenteRepository();
            List<MascotaDto> tipos = asistenteRepository.ObtenerTipos();

            return tipos.Select(t => new SelectListItem
            {
                Value = t.id_tipo.ToString(),
                Text = t.nom_tipo
            }).ToList();
        }

        public List<SelectListItem> ObtenerClientesSelect()
        {
            AsistenteRepository repo = new AsistenteRepository();
            return repo.ObtenerClientesSelect();
        }

        public List<CitaEspecVistaDto> ObtenerCitasEspecializadas()
        {
            AsistenteRepository asistenteRepository = new AsistenteRepository();
            return asistenteRepository.ObtenerCitasEspecializadas();
        }

        public void ActualizarEstadoCita(int id_cita_esp, int nuevo_estado)
        {
            try
            {
                AsistenteRepository repo = new AsistenteRepository();
                repo.ActualizarEstadoCita(id_cita_esp, nuevo_estado);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en el servicio al actualizar estado: " + ex.Message);
                throw;
            }
        }

        public List<CitaEspecVistaDto> ObtenerCitasPorDia(int id_dia)
        {
            AsistenteRepository repo = new AsistenteRepository();
            return repo.ObtenerCitasPorDia(id_dia);
        }

       
    }
}
