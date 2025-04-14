using Clinipet.Dtos;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Clinipet.Repositories
{
    public class ClienteRepository
    {
        public DisponibDto ObtenerDisponPorId(int id_dispon)

        {
            List<DisponibDto> citasDisponibles = new List<DisponibDto>();


            string sql = "SELECT " +
             "e.nom_espec AS EspecialidadServicio, " +
             "u.nom_usu + ' ' + u.apel_usu AS Veterinario, " +
             "di.nombre AS Dia, " +
             "h.nom_hora AS Hora, " +
             "d.id_dispon AS dispon " +
             
             "FROM serv_dispon sd " +
             "JOIN disponibilidad d ON sd.id_dispon = d.id_dispon " +
             "JOIN usuario u ON d.id_usu = u.id_usu " +
             "JOIN mascota m ON u.id_usu = m.id_usu " +
             "JOIN servicio s ON sd.id_servicio = s.id_servicio " +
             "JOIN especialidad e ON u.id_espec = e.id_espec " +
             "JOIN dia di ON d.id_dia = di.id_dia " +
             "JOIN hora h ON d.id_hora = h.id_hora " +
             "WHERE d.id_dispon = @id_dispon";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_dispon", id_dispon);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DisponibDto
                        {
                            nom_serv = reader.GetString(0),
                            nom_usu = reader.GetString(1),
                            nom_dia = reader.GetString(2),
                            nom_hora = reader.GetString(3),
                            id_dispon = reader.GetInt32(4),
                            //nom_masc= reader.GetString(5),

                        };
                    }
                }
            }
            return null; // Si no se encuentra la disponibilidad
        }
    }
}