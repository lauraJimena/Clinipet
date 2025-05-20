using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

                // Seteo de datos por defecto
                nuevoAsist.id_rol = 2;        // Asistente
                nuevoAsist.id_nivel = 1;      // Nivel básico
                nuevoAsist.id_estado = 1;     // Activo
                nuevoAsist.id_espec = 1;     // No aplica
                nuevoAsist.cambio_contras = true; // Requiere cambiar contraseña al iniciar

                // Encriptar la contraseña antes de registrar
                nuevoAsist.contras_usu = EncriptContrasUtility.EncripContras(nuevoAsist.contras_usu);

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
                            // Enviar correo en segundo plano
                            Task.Run(() =>
                            {
                                try
                                {
                                    EmailConfigUtility gestorCorreo = new EmailConfigUtility();
                                    String destinatario = nuevoAsist.correo_usu;
                                    String asunto = "Bienvenido al sistema de CliniPet!";
                                    gestorCorreo.EnviarCorreoBienv(destinatario, asunto, nuevoAsist);
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

                // Se encripta la contraseña
                userModel.contras_usu = EncriptContrasUtility.EncripContras(userModel.contras_usu);

                int resultado = userRepository.RegistrarUsuario(userModel);

                switch (resultado)
                {
                    case 1:
                        responseUserDto.Response = 1;
                        responseUserDto.Mensaje = "Creación exitosa";
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


        public DisponibDto obtenerDisponPorId(int id_dispon)

        {
            System.Diagnostics.Debug.WriteLine("Estoy en obtenerCitaPorId");
            AsistenteRepository asisRepository = new AsistenteRepository();
            return asisRepository.ObtenerDisponPorId(id_dispon);

        }

        public MascotaDto ObtenerMascotaPorId(int id_mascota)

        {
            System.Diagnostics.Debug.WriteLine("Estoy en obtenerMascotaPorId");
            AsistenteRepository asisRepository = new AsistenteRepository();
            return asisRepository.ObtenerMascotaPorId(id_mascota);

        }

        public List<MascotaDto> ListadoMascotas(int id_usu)

        {

            AsistenteRepository asisRepository = new AsistenteRepository();
            List<MascotaDto> mascotas = asisRepository.ListadoMascotas(id_usu);

            return mascotas;
        }

        public List<MascotaDto> ListadoTodasMascotas()
        {
            AsistenteRepository asisRepository = new AsistenteRepository();
            System.Diagnostics.Debug.WriteLine(">>> Entrando a ListadoTodasMascotas");
            List<MascotaDto> mascotas = asisRepository.ListarTodasLasMascotas();
            System.Diagnostics.Debug.WriteLine($">>> Mascotas encontradas: {mascotas.Count}");
            return mascotas;
        }

        public List<ServicioDto> ListadoServiciosGenerales()

        {

            AsistenteRepository asisRepository = new AsistenteRepository();
            List<ServicioDto> serviciosGenerales = asisRepository.ListadoServiciosGenerales();

            return serviciosGenerales;
        }

        public List<DisponibDto> ObtenerCitasGenDispon(int id_servicio)
        {
            AsistenteRepository asisclienteRepository = new AsistenteRepository();
            List<DisponibDto> citasGenDispon = asisclienteRepository.ObtenerCitasGenDispon(id_servicio);

            return citasGenDispon;

        }

        public CitaGeneralDto AgendarCitaGeneral(CitaGeneralDto citaModel)

        {
            CitaGeneralDto responseCitaGenDto = new CitaGeneralDto();
            AsistenteRepository asisRepository = new AsistenteRepository();
            Console.WriteLine("Estoy en el servicio");
            try
            {

                citaModel.id_servicio = citaModel.id_servicio;
                citaModel.id_estado = 3; //Agendada
                if (asisRepository.RegistrarCitaGeneral(citaModel) != 0)
                {
                    responseCitaGenDto.Response = 1;
                    responseCitaGenDto.Mensaje = "Creación exitosa";

                }
                else
                {
                    responseCitaGenDto.Response = 0;
                    responseCitaGenDto.Mensaje = "Algo pasó";
                }

                return responseCitaGenDto;
            }
            catch (Exception e)
            {
                responseCitaGenDto.Response = 0;
                responseCitaGenDto.Mensaje = e.InnerException?.ToString();
                return responseCitaGenDto;
            }

        }

        public List<MascotaDto> BuscarMascotas(string nombreMascota, string cedulaDueno)
        {
            AsistenteRepository asisRepository = new AsistenteRepository();
            List<MascotaDto> mascotas = asisRepository.BuscarMascotas(nombreMascota, cedulaDueno);
            return mascotas;
        }
        public DisponibDto PublicarDisponGen(DisponibDto dispon)
        {

            AsistenteRepository asistenteRepository = new AsistenteRepository();
            DisponibDto disponibRespuesta = new DisponibDto();


            dispon.id_estado = 1; //Disponibilidad Activa
                                  // Esto puede lanzar una excepción (por ejemplo, por trigger), se deja para que el controlador muestre el mensaje específico
            disponibRespuesta = asistenteRepository.PublicarDisponGen(dispon);

            if (disponibRespuesta != null)
            {
                ServicioDto servicio = new ServicioDto();
                servicio.id_dispon = disponibRespuesta.id_dispon;
                servicio.id_servicio = disponibRespuesta.id_servicio;

                int resultado = asistenteRepository.RegistrarServicio_Dispon(servicio);

                if (resultado != 0)
                {
                    return new DisponibDto
                    {
                        Response = 1,
                        Mensaje = "Cita publicada correctamente",
                        id_dispon = disponibRespuesta.id_dispon
                };
                }
                else
                {
                    throw new Exception("No se pudo registrar el servicio.");
                }
            }

            // Si no se generó el ID de disponibilidad
            throw new Exception("No se pudo registrar la disponibilidad.");


        }

    }
}
