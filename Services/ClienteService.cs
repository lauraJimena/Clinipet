using Clinipet.Dtos;
using Clinipet.Repositories;
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
        public CitaEspecDto RegistrarCitaEspec(CitaEspecDto citaModel)

        {
            CitaEspecDto responseUserDto = new CitaEspecDto();
            ClienteRepository clienteRepository = new ClienteRepository();
            Console.WriteLine("Estoy en el servicio");
            try
            {
                citaModel.id_motivo = 1;
                citaModel.id_servicio = clienteRepository.ObtenerIdServicioPorEspecialidad(citaModel.nom_espec);

                //citaModel.id_servicio = 1;

                if (clienteRepository.RegistrarCitaEspecializada(citaModel) != 0)
                {
                    responseUserDto.Response = 1;
                    responseUserDto.Mensaje = "Creación exitosa";

                }
                else
                {
                    responseUserDto.Response = 0;
                    responseUserDto.Mensaje = "Algo pasó";
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
        public List<MascotaDto> ListadoMascotas(int id_usu)

        {
            
            ClienteRepository clienteRepository = new ClienteRepository();
            List<MascotaDto> mascotas = clienteRepository.ListadoMascotas(id_usu);

            return mascotas;
        }
    }
}