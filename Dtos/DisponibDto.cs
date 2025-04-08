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
        public TimeSpan id_hora { get; set; }
        public int id_usu { get; set; }
        public int Response { get; set; }

        public string Mensaje { get; set; } = string.Empty;

    }
}
