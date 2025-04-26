using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class CitaGeneralDto
    {
        public int id_cita_gen { get; set; }
        public int id_dispon { get; set; }
        public int id_estado { get; set; }
        public int id_mascota { get; set; }
        public int id_servicio { get; set; }
        public string nom_dia { get; set; }
        public string nom_hora { get; set; }
        public string nom_estado{ get; set; }
        public string nom_masc { get; set; }
        public string nom_serv { get; set; }
        public int Response { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}