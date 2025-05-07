using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinipet.Utilities
{
    public class FechaUtility
    {
        public DateTime ObtenerProximaFecha(string nombreDia)
        {
            // Traducir de español a inglés
        var diasTraducidos = new Dictionary<string, DayOfWeek>(StringComparer.OrdinalIgnoreCase)
        {
            { "domingo", DayOfWeek.Sunday },
            { "lunes", DayOfWeek.Monday }, 
            { "martes", DayOfWeek.Tuesday },
            { "miércoles", DayOfWeek.Wednesday },
            { "miercoles", DayOfWeek.Wednesday }, // por si no lleva tilde
            { "jueves", DayOfWeek.Thursday },
            { "viernes", DayOfWeek.Friday },
            { "sábado", DayOfWeek.Saturday },
            { "sabado", DayOfWeek.Saturday } 
        };

            if (!diasTraducidos.TryGetValue(nombreDia.ToLower(), out DayOfWeek diaDeseado))
            {
                throw new ArgumentException("Día no válido: " + nombreDia);
            }

            DateTime hoy = DateTime.Today;
            int diasFaltantes = ((int)diaDeseado - (int)hoy.DayOfWeek + 7) % 7;
            diasFaltantes = diasFaltantes == 0 ? 7 : diasFaltantes;

            return hoy.AddDays(diasFaltantes);
        }
    }
}