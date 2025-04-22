using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class CitaGeneralDto
    {
        public int id_cita_gen { get; set; }
        public string id_dispon { get; set; }
        public string id_estado { get; set; }
        public string id_mascota { get; set; }
        public string id_servicio { get; set; }
    }
}