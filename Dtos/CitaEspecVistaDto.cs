using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class CitaEspecVistaDto
    {
        public int id_cita_esp { get; set; }
        public string nom_mascota { get; set; }
        public string nom_servicio { get; set; }
        public string nom_veterinario { get; set; }
        public int id_dia { get; set; }
        public string nom_dia { get; set; }
        public string nom_hora { get; set; }
        public string estado { get; set; }
        public int Response { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}