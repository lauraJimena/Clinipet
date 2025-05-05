using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class CitaEspecDto
    {
        public int id_cita_esp { get; set; }
        public int id_servicio { get; set; }
        public int id_motivo { get; set; }
        public string nom_motivo { get; set; }
        public string nom_masc { get; set; }
        public string nom_serv { get; set; }
        public string nom_dia { get; set; }
        public string nom_hora { get; set; }
        public string nom_usu { get; set; }
        public string diagnost { get; set; } = "Pendiente";
        public int id_mascota { get; set; }
        public int id_dispon { get; set; }
        public string num_ident { get; set; }
        public int id_estado { get; set; } = 1; //Activo
        public string recomen { get; set; } = "Pendiente";
        public string nom_espec { get; set; }
        public string nom_estado { get; set; }
        public DateTime fecha_cita{ get; set; }
        public int Response { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}