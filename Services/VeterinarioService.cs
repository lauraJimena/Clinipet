using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinipet.Dtos;
using Clinipet.Services;
using Clinipet.Repositories;
using System.Web.Mvc;

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
                //nuevoVete.id_espec = 10;     

                //System.Diagnostics.Debug.WriteLine($"Nombre: {nuevoAsist.nom_usu}, Apellido: {nuevoAsist.apel_usu}, ID Tipo Doc: {nuevoAsist.id_tipo_ident}, Especialidad: {nuevoAsist.id_espec}");

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
                        //userResponse.Mensaje = "Asistente registrado con éxito";
                    }
                    else
                    {
                        if (VeteRepo.RegistrarUsuario(nuevoVete) != 0)
                        {
                            userResponse.Response = 1;
                            userResponse.Mensaje = "Creación exitosa";

                        }
                        else
                        {
                            userResponse.Response = 0;
                            userResponse.Mensaje = "Algo pasó";
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
            DisponibDto disponibResponse = new DisponibDto();
            
            try
            {
                int id_dispon = veterinarioRepository.PublicarDispon(dispon);
                if (id_dispon != 0)
                {                                     
                    ServicioDto servicio = new ServicioDto();                    
                    servicio.id_dispon = id_dispon;                   
                    if (veterinarioRepository.RegistrarServicio_Dispon(servicio) != 0){
                        disponibResponse.Response = 1;
                        disponibResponse.Mensaje = "Cita publicada correctamente";
                        System.Diagnostics.Debug.WriteLine(servicio.id_servicio);
                    }                                      

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
    }
}