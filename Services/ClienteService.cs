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
    }
}