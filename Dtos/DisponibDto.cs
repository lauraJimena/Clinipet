using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class DisponibDto
    {
        public int id_dispon { get; set; }
        public byte id_dia { get; set; }
        public int id_hora { get; set; }
        public int id_usu { get; set; }
        public string nom_usu { get; set; }= string.Empty;
        public string nom_dia{ get; set; } = string.Empty;
        public string nom_hora { get; set; } = string.Empty;
        public string nom_espec { get; set; } = string.Empty;
        public int id_servicio { get; set; }
        public string nom_serv { get; set; } = string.Empty;
        public string nom_masc { get; set; } = string.Empty;
        
        public int Response { get; set; }

        public string Mensaje { get; set; } = string.Empty;

    }
}
