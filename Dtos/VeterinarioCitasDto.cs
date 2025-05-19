using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Dtos
{
    public class VeterinarioCitasDto
    {
        public int IdVeterinario { get; set; }
        public string NombreVeterinario { get; set; }
        public int TotalCitas { get; set; }
        public string Espec { get; set; }
    }
}