using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class ReporteDto
    {
        public List<ServicioDto> Generales { get; set; }
        public List<ServicioDto> Especializados { get; set; }
        public List<VeterinarioCitasDto> CitasPorVeterinario { get; set; }
    }
}