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
            //List<DisponibDto> citasDisponibles = new List<DisponibDto>();


            string sql = "SELECT " +
             "e.nom_espec AS EspecialidadServicio, " +
             "u.nom_usu + ' ' + u.apel_usu AS Veterinario, " +
             "di.nombre AS Dia, " +
             "h.nom_hora AS Hora, " +
             "d.id_dispon AS dispon " +
             "FROM serv_dispon sd " +
             "JOIN disponibilidad d ON sd.id_dispon = d.id_dispon " +
             "JOIN usuario u ON d.id_usu = u.id_usu " +           
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
                            nom_usu = reader.GetString(1),//Veterinario
                            nom_dia = reader.GetString(2),
                            nom_hora = reader.GetString(3),
                            id_dispon = reader.GetInt32(4),
                            

                        };
                    }
                }
            }
            return null; // Si no se encuentra la disponibilidad
        }
        public MascotaDto ObtenerMascotaPorId(int id_mascota)

        {
            //List<DisponibDto> citasDisponibles = new List<DisponibDto>();


            string sql = "SELECT m.id_mascota, m.nom_masc, m.edad_masc, " +
                         "t.nom_tipo, r.nom_raza, u.nom_usu + ' ' + u.apel_usu " +
                         "FROM mascota m " +
                         "JOIN tipo_masc t ON m.id_tipo = t.id_tipo " +
                         "JOIN raza_masc r ON m.id_raza = r.id_raza " +
                         "JOIN usuario u ON m.id_usu = u.id_usu " +
                         "WHERE m.id_mascota = @id_mascota";


            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_mascota", id_mascota);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MascotaDto
                        {
                            id_mascota = reader.GetInt32(0),                           
                            nom_masc = reader.GetString(1),
                            edad_masc = reader.GetInt16(2),
                            nom_tipo = reader.GetString(3),
                            nom_raza = reader.GetString(4),
                            nom_usu = reader.GetString(5)

                        };
                    }
                }
            }
            return null; // Si no se encuentra la disponibilidad
        }

        public List<MascotaDto> ListadoMascotas(int id_usu)

        {
            List<MascotaDto> mascotas = new List<MascotaDto>();


            string sql = "SELECT r.nom_raza, t.nom_tipo, m.nom_masc, m.edad_masc, m.id_mascota " +
                 "FROM [clinipet].[dbo].[raza_masc] r " +
                 "JOIN [clinipet].[dbo].[tipo_masc] t ON r.id_tipo = t.id_tipo " +
                 "JOIN [clinipet].[dbo].[mascota] m ON m.id_raza = r.id_raza " +
                 "JOIN [clinipet].[dbo].[usuario] u ON m.id_usu = u.id_usu " +
                  "WHERE u.id_usu = @id_usu";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_usu", id_usu);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MascotaDto mascota = new MascotaDto
                        {
                            nom_raza = reader.GetString(0),
                            nom_tipo = reader.GetString(1),
                            nom_masc = reader.GetString(2),
                            edad_masc= reader.GetInt16(3),
                            id_mascota = reader.GetInt32(4),



                        };
                        mascotas.Add(mascota);
                    }

                }
            }
            return mascotas;
        }
    }
}