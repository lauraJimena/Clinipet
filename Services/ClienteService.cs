using Clinipet.Dtos;
using Clinipet.Repositories;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Services
{
    public class ClienteService
    {
        public DisponibDto obtenerDisponPorId(int id_dispon)

        {
            System.Diagnostics.Debug.WriteLine("Estoy en obtenerCitaPorId");
            ClienteRepository clienteRepository= new ClienteRepository();
            return clienteRepository.ObtenerDisponPorId(id_dispon);

        }
        public MascotaDto ObtenerMascotaPorId(int id_mascota)

        {
            System.Diagnostics.Debug.WriteLine("Estoy en obtenerCitaPorId");
            ClienteRepository clienteRepository = new ClienteRepository();
            return clienteRepository.ObtenerMascotaPorId(id_mascota);

        }
        public CitaEspecDto RegistrarCitaEspec(CitaEspecDto citaModel, UserDto usuModel, MascotaDto mascotaModel)

        {
            CitaEspecDto citaEspecDto = new CitaEspecDto();
            ClienteRepository clienteRepository = new ClienteRepository();
            FechaUtility fechaUtility = new FechaUtility();
            Console.WriteLine("Estoy en el servicio");
            try
            {
                citaModel.id_motivo = 1;
                citaModel.id_servicio = clienteRepository.ObtenerIdServicioPorEspecialidad(citaModel.nom_espec);
                citaModel.id_estado = 3; //Agendada
                //citaModel.id_estado_dispon = 2;
                //citaModel.id_servicio = 1;

                string nom_dia = citaModel.nom_dia; 
                DateTime fecha_cita = fechaUtility.ObtenerProximaFecha(nom_dia);
                citaModel.fecha_cita = fecha_cita;

                
               
                int respuesta= clienteRepository.RegistrarCitaEspecializada(citaModel);
                if (respuesta != 0)
                {
                    citaEspecDto.Response = 1;
                    clienteRepository.ActualizarEstadoDispon(citaModel);
                    EmailConfigUtility gestorCorreo = new EmailConfigUtility();
                    String destinatario = usuModel.correo_usu;
                    String asunto = "Cita confirmada exitosamente!";
                    gestorCorreo.EnviarCorreoCita(destinatario, asunto, usuModel, citaModel, mascotaModel);
                    citaEspecDto.Mensaje = "Cita confirmada exitosamente";

                }
                else
                {
                    citaEspecDto.Response = 0;
                    citaEspecDto.Mensaje = "Algo pasó";
                }

                return citaEspecDto;
            }
            catch (Exception ex)
            {
                citaEspecDto.Response = 0;
              
                // Verificar si el mensaje contiene el error del trigger
                if (ex.Message.Contains("La mascota ya tiene una cita activa"))
                {
                    citaEspecDto.Mensaje = "La mascota ya tiene una cita activa en esa fecha y hora. Por favor selecciona otra disponibilidad.";
                }
                else
                {
                 

                    // Mensaje genérico en caso de otro tipo de error
                    citaEspecDto.Mensaje = "Ocurrió un error inesperado. Intenta de nuevo más tarde.";
                }

            }
            return citaEspecDto;

        }
        /*public CitaGeneralDto AgendarCitaGeneral(CitaGeneralDto citaModel)

        {
            CitaGeneralDto responseCitaGenDto = new CitaGeneralDto();
            ClienteRepository clienteRepository = new ClienteRepository();
            Console.WriteLine("Estoy en el servicio");
            try
            {

                citaModel.id_servicio = citaModel.id_servicio;
                citaModel.id_estado = 3;
                if (clienteRepository.RegistrarCitaGeneral(citaModel) != 0)
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

        }*/
        public CitaGeneralDto AgendarCitaGeneral(CitaGeneralDto citaModel)
        {

            CitaGeneralDto responseCitaGenDto = new CitaGeneralDto();
            ClienteRepository clienteRepository = new ClienteRepository();

            try
            {
                // Siempre usamos estado 3 (pendiente)
                citaModel.id_estado = 3;

                // Registramos la cita general con los datos que vienen
                int resultado = clienteRepository.RegistrarCitaGeneral(citaModel);

                if (resultado != 0)
                {
                    responseCitaGenDto.Response = 1;
                    responseCitaGenDto.Mensaje = "Cita registrada con éxito.";
                }
                else
                {
                    responseCitaGenDto.Response = 0;
                    responseCitaGenDto.Mensaje = "No se pudo registrar la cita.";
                }
            }
            catch (Exception e)
            {
                responseCitaGenDto.Response = 0;
                responseCitaGenDto.Mensaje = e.InnerException?.Message ?? e.Message;
            }

            return responseCitaGenDto;
        }

        public List<MascotaDto> ListadoMascotas(int id_usu)

        {
            
            ClienteRepository clienteRepository = new ClienteRepository();
            List<MascotaDto> mascotas = clienteRepository.ListadoMascotas(id_usu);

            return mascotas;
        }
        public List<ServicioDto> ListadoServiciosGenerales()

        {

            ClienteRepository clienteRepository = new ClienteRepository();
            List<ServicioDto> serviciosGenerales = clienteRepository.ListadoServiciosGenerales();

            return serviciosGenerales;
        }
        public List<DisponibDto> ObtenerCitasGenDispon(int id_servicio)
        {
            ClienteRepository clienteRepository = new ClienteRepository();
            List<DisponibDto> citasGenDispon = clienteRepository.ObtenerCitasGenDispon(id_servicio);

            return citasGenDispon;

        }
        public List<CitaGeneralDto> HistorialCitasGenerales(int id_usu)
        {
            ClienteRepository clienteRepository = new ClienteRepository();
            List<CitaGeneralDto> citasGenerales = clienteRepository.HistorialCitasGenerales(id_usu);

            return citasGenerales;

        }
        public List<CitaEspecDto> HistorialCitasEspec(int id_usu)
        {
            ClienteRepository clienteRepository = new ClienteRepository();
            List<CitaEspecDto> citasEsp = clienteRepository.HistorialCitasEspec(id_usu);

            return citasEsp;

        }
    }
}