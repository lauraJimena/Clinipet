using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class CitaEspecDto
    {
        public int id_servicio { get; set; }
        public int id_motivo { get; set; }
        public string diagnost { get; set; } = "Pendiente";
        public int id_mascota { get; set; }
        public int id_dispon { get; set; }
        public int id_estado { get; set; } = 1; // Por ejemplo, 1 = Pendiente
        public string recomen { get; set; } = "Pendiente";
        public string nom_espec { get; set; }
        public int Response { get; set; }

        public string Mensaje { get; set; } = string.Empty;
    }
}