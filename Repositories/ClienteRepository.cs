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
             "s.id_servicio, " +
             "e.nom_espec Especialidad, " +           
             "u.nom_usu + ' ' + u.apel_usu Veterinario, " +
             "di.nombre Dia, " +
             "h.nom_hora Hora, " +
             "d.id_dispon dispon " +
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
                            id_servicio=reader.GetInt32(0),
                            nom_espec = reader.GetString(1),
                            nom_usu = reader.GetString(2),//Veterinario
                            nom_dia = reader.GetString(3),
                            nom_hora = reader.GetString(4),
                            id_dispon = reader.GetInt32(5),
                            

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
        public int RegistrarCitaEspecializada(CitaEspecDto cita)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "INSERT INTO [clinipet].[dbo].[cita_espec] " +
                         "(id_servicio, id_motivo, diagnost, id_mascota, id_dispon, id_estado, recomen) " +
                         "VALUES (@id_servicio, @id_motivo, @diagnost, @id_mascota, @id_dispon, @id_estado, @recomen)";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_servicio", cita.id_servicio);
                command.Parameters.AddWithValue("@id_motivo", cita.id_motivo);
                command.Parameters.AddWithValue("@diagnost", cita.diagnost);
                command.Parameters.AddWithValue("@id_mascota", cita.id_mascota);
                command.Parameters.AddWithValue("@id_dispon", cita.id_dispon);
                command.Parameters.AddWithValue("@id_estado", cita.id_estado);
                command.Parameters.AddWithValue("@recomen", cita.recomen);

                comando = command.ExecuteNonQuery();
            }

            Connection.Disconnect();
            return comando;
        }
        public int ObtenerIdServicioPorEspecialidad(string nombreEspecialidad)
        {
            int idServicio = 0;
            //int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT id_servicio FROM servicio WHERE nombre = @nombre AND tipo_servicio = 1";

            using (SqlCommand cmd = new SqlCommand(sql, Connection.CONN()))
            {
                cmd.Parameters.AddWithValue("@nombre", nombreEspecialidad);

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    idServicio = Convert.ToInt32(result);
                }
            }

            Connection.Disconnect();
            return idServicio;
        }
        



    }
}