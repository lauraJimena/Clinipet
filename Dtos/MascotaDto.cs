using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class MascotaDto
    {
        public int id_mascota { get; set; }
        public string nom_masc { get; set; } = string.Empty;
        public int edad_masc { get; set; }
        public int id_raza { get; set; }
        public string nom_raza { get; set; } = string.Empty;
        public int id_tipo { get; set; }
        public string nom_tipo { get; set; }
        public int id_usu { get; set; }
        public string nom_usu{ get; set; } = string.Empty;
        public string num_ident { get; set; } = string.Empty;
        public int Response { get; set; }
        public string Mensaje { get; set; } = string.Empty;

    }
}