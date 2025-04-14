using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class MascotaCitaDto
    {
        public List<MascotaDto> Mascotas { get; set; }
        public MascotaDto Mascota { get; set; }
        public DisponibDto Disponib { get; set; }
        public int IdDispon { get; set; }
        public int IdUsu { get; set; }
    }
}