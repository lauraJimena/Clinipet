using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class DisponibilidadAgrupadaDto
    {
        public string Dia { get; set; }
        public List<string> Horas { get; set; }
    }
}