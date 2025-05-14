using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class HistorialCitasDto
    {
        
            public List<CitaEspecDto> Citas { get; set; }
            public List<DisponibDto> Disponib { get; set; }
        public List<DisponibilidadAgrupadaDto> DisponibAgrupada { get; set; } // para la vista agrupada

    }
}