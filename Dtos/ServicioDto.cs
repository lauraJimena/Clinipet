using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class ServicioDto
    {
        public int id_servicio { get; set; }
        public string nom_serv{ get; set; }
        public byte tipo_servicio { get; set; }
        public string descrip{ get; set; }
        public int id_estado { get; set; }
        public int serv_dispon { get; set; }
        public int id_dispon{ get; set; }
        public int total_citas { get; set; }

    }
}