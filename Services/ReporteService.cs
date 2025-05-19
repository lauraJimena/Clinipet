using Clinipet.Dtos;
using Clinipet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Services
{
    public class ReporteService
    {
        public ReporteDto ObtenerDatosReporteServicios()
        {

            var repo = new ReporteRepository();
            return new ReporteDto
            {
                Generales = repo.ObtenerServGeneralesMasPrestados(),
                Especializados = repo.ObtenerServEspecMasPrestados(),
                CitasPorVeterinario = repo.ObtenerCitasVet()
            };
        }
        public List<VeterinarioCitasDto> ObtenerCitasVet()
        {
            ReporteRepository repo = new ReporteRepository();
            List<VeterinarioCitasDto> mascotas = repo.ObtenerCitasVet();

            return mascotas;

        }

    }
}