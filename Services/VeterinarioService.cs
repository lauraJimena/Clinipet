using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinipet.Dtos;
using Clinipet.Services;
using Clinipet.Repositories;
using System.Web.Mvc;
using Clinipet.Utilities;
using System.Threading.Tasks;

namespace Clinipet.Services
{
    public class VeterinarioService
    {
        //Metodo Registro de veterinatio
        public UserDto RegistrarVeterinario(UserDto nuevoVete)
        {
            GeneralRepository VeteRepo = new GeneralRepository();
            UserDto userResponse = new UserDto();

            try
            {
                Console.WriteLine("Estoy en registro de Veterinario");

                // Seteo de datos por defecto
                nuevoVete.id_rol = 4;        // Veterinario
                nuevoVete.id_nivel = 1;      // Nivel básico
                nuevoVete.id_estado = 1;     // Activo
                nuevoVete.cambio_contras = true; // Requiere cambiar contraseña al iniciar
                                                 //nuevoVete.id_espec = 10;     

                // Hashear la contraseña antes de registrar
                nuevoVete.contras_usu = EncriptContrasUtility.EncripContras(nuevoVete.contras_usu);

                // Validaciones
                if (VeteRepo.ExisteCorreo(nuevoVete.correo_usu))
                {
                    userResponse.Response = -1;
                    userResponse.Mensaje = "El correo ya existe. Intente nuevamente.";
                    //userResponse.Mensaje = "Asistente registrado con éxito";
                }
                else
                {
                    if (VeteRepo.ExisteDocumento(nuevoVete.num_ident))
                    {
                        userResponse.Response = -2;
                        userResponse.Mensaje = "El numero de documento ya existe. Intente nuevamente.";
                    }
                    else
                    {
                        if (VeteRepo.RegistrarUsuario(nuevoVete) != 0)
                        {
                            userResponse.Response = 1;
                            userResponse.Mensaje = "Veterinario registrado exitosamente.";
                            // Enviar correo en segundo plano
                            Task.Run(() =>
                            {
                                try
                                {
                                    EmailConfigUtility gestorCorreo = new EmailConfigUtility();
                                    String destinatario = nuevoVete.correo_usu;
                                    String asunto = "Bienvenido al sistema de CliniPet!";
                                    gestorCorreo.EnviarCorreoBienv(destinatario, asunto, nuevoVete);
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
        
        public List<UserDto> ObtenerEspecialidad()
        {
            GeneralRepository generalRepository = new GeneralRepository();
            //List<UserDto> especialidad = generalRepository.ObtenerEspecialidad();
            return generalRepository.ObtenerEspecialidad();
        }
        public DisponibDto PublicarDisponibilidad(DisponibDto dispon)
        {           

            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            DisponibDto disponibRespuesta = new DisponibDto();
            
            
                dispon.id_estado = 1; //Disponibilidad Activa
             // Esto puede lanzar una excepción (por ejemplo, por trigger), se deja para que el controlador muestre el mensaje específico
             int id_dispon = veterinarioRepository.PublicarDispon(dispon);
                
                if (id_dispon != 0)
                {                                     
                    ServicioDto servicio = new ServicioDto();                    
                    servicio.id_dispon = id_dispon;
                    int resultado = veterinarioRepository.RegistrarServicio_Dispon(servicio);

                if (resultado != 0)
                {
                    return new DisponibDto
                    {
                        Response = 1,
                        Mensaje = "Cita publicada correctamente",
                        id_dispon = id_dispon
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
        
        public List<SelectListItem> ObtenerDiasSelect()
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<DisponibDto> dias = veterinarioRepository.ObtenerDias();

            return dias.Select(d => new SelectListItem // Convertir la lista raza en SelectList para pasar a la vista
            {
                Value = d.id_dia.ToString(),
                Text = d.nom_dia,
            }).ToList();
        }
        public List<SelectListItem> ObtenerHorasSelect()
        {   
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<DisponibDto> horas = veterinarioRepository.ObtenerHoras();

            return horas.Select(h => new SelectListItem // Convertir la lista raza en SelectList para pasar a la vista
            {
                Value = h.id_hora.ToString(),
                Text = h.nom_hora,
                
            }).ToList();

            
        }
        public List<SelectListItem> ObtenerServiciosGenSelect()
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<DisponibDto> serv = veterinarioRepository.ObtenerServiciosGen();

            return serv.Select(h => new SelectListItem // Convertir la lista raza en SelectList para pasar a la vista
            {
                Value = h.id_servicio.ToString(),
                Text = h.nom_serv,

            }).ToList();


        }
        public List<SelectListItem> ObtenerMotivoSelect()
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<CitaEspecDto> motivo= veterinarioRepository.ObtenerMotivo();

            return motivo.Select(m => new SelectListItem // Convertir la lista raza en SelectList para pasar a la vista
            {
                Value = m.id_motivo.ToString(),
                Text = m.nom_motivo,

            }).ToList();


        }
        public List<MascotaDto> ListadoMascotas(string num_ident)

        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<MascotaDto> mascotas = veterinarioRepository.ListadoMascotas(num_ident);

            return mascotas;
        }
        public UserDto ObtenerUsuarioPorNumIdent(string num_ident)
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            UserDto usu = veterinarioRepository.ObtenerUsuarioPorNumIdent(num_ident);
            return usu;
        }
        public List<CitaEspecDto> ObtenerCitasEspecAgend(int id_usu, int id_mascota)
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<CitaEspecDto> citasDispon = veterinarioRepository.ObtenerCitasEspecAgend(id_usu, id_mascota);

            return citasDispon;

        }
        public List<CitaEspecDto> ObtenerHistorialCitas(int id_usu)
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<CitaEspecDto> citas = veterinarioRepository.ObtenerHistorialCitas(id_usu);

            return citas;

        }
        public List<DisponibDto> ObtenerDisponib(int id_usu)
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<DisponibDto> citas = veterinarioRepository.ObtenerDisponib(id_usu);

            return citas;

        }
        public CitaEspecDto ActualizarDescripConsulta(CitaEspecDto citaEspec)
        {
                  
            CitaEspecDto citaEspecDto = new CitaEspecDto();
            VeterinarioRepository veterRepository = new VeterinarioRepository();
            
            try
            {
                citaEspec.id_estado = 5;//Cita completada

                if (veterRepository.ActualizarDescripConsulta(citaEspec) != 0)
                {
                    citaEspecDto.Response = 1;
                    citaEspecDto.Mensaje = "Creación exitosa";

                }
                else
                {
                    citaEspecDto.Response = 0;
                    citaEspecDto.Mensaje = "Algo pasó";
                }

                return citaEspecDto;
            }
            catch (Exception e)
            {
                citaEspecDto.Response = 0;
                citaEspecDto.Mensaje = e.InnerException?.ToString();
                return citaEspecDto;
            }

        }
        public List<CitaEspecDto> ObtenerHistorialMascota(int id_usu)
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<CitaEspecDto> citas = veterinarioRepository.ObtenerHistorialMascota(id_usu);

            return citas;

        }
        public List<MascotaDto> ObtenerMascotas(int id_usu)
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            List<MascotaDto> mascotas = veterinarioRepository.ObtenerMascotas(id_usu);

            return mascotas;

        }
        public List<SelectListItem> ObtenerMotivoSelectPorEspecialidad(int idEspecialidad)
        {
            VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
            return veterinarioRepository.ObtenerMotivosPorEspecialidad(idEspecialidad);

        }






    }
}