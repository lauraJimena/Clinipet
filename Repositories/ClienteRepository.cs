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
                             "(id_servicio, id_motivo, diagnost, id_mascota, id_dispon, id_estado, recomen, fecha_cita) " +
                             "VALUES (@id_servicio, @id_motivo, @diagnost, @id_mascota, @id_dispon, @id_estado, @recomen, @fecha_cita)";

                using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
                {
                    command.Parameters.AddWithValue("@id_servicio", cita.id_servicio);
                    command.Parameters.AddWithValue("@id_motivo", cita.id_motivo);
                    command.Parameters.AddWithValue("@diagnost", cita.diagnost);
                    command.Parameters.AddWithValue("@id_mascota", cita.id_mascota);
                    command.Parameters.AddWithValue("@id_dispon", cita.id_dispon);
                    command.Parameters.AddWithValue("@id_estado", cita.id_estado);
                    command.Parameters.AddWithValue("@recomen", cita.recomen);
                    command.Parameters.AddWithValue("@fecha_cita", cita.fecha_cita);


                    comando = command.ExecuteNonQuery();
                }

                Connection.Disconnect();
                return comando;
            }
        public int RegistrarCitaGeneral(CitaGeneralDto citaGeneral)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "INSERT INTO [clinipet].[dbo].[cita_general] " +
                         "(id_dispon, id_estado, id_mascota, id_servicio) " +
                         "VALUES (@id_dispon, @id_estado, @id_mascota, @id_servicio)";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                
                command.Parameters.AddWithValue("@id_dispon", citaGeneral.id_dispon);                           
                command.Parameters.AddWithValue("@id_estado", citaGeneral.id_estado);
                command.Parameters.AddWithValue("@id_mascota", citaGeneral.id_mascota);
                command.Parameters.AddWithValue("@id_servicio", citaGeneral.id_servicio);

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
        
        public List<ServicioDto> ListadoServiciosGenerales()

        {
            List<ServicioDto> citasGenerales = new List<ServicioDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT " +
             "id_servicio, " +
             "nombre " +            
             "FROM servicio " +            
             "WHERE tipo_servicio = 0";

            using (SqlCommand cmd = new SqlCommand(sql, Connection.CONN()))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ServicioDto dispon = new ServicioDto
                        {

                            id_servicio = reader.GetInt32(0),
                            nom_serv = reader.GetString(1),
                            
                        };
                        citasGenerales.Add(dispon);
                    }
                }
            }

            return citasGenerales;

        }
        public int ActualizarEstadoDispon(CitaEspecDto citaEspec)
        {

            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "UPDATE disponibilidad SET " +
                "id_estado = 2 " +
                "WHERE id_dispon = @id_dispon";

            using (SqlCommand cm = new SqlCommand(SQL, Connection.CONN()))
            {

                cm.Parameters.AddWithValue("@id_dispon", citaEspec.id_dispon);
                


                comando = cm.ExecuteNonQuery();
            }

            return comando;

        }
        public List<DisponibDto> ObtenerCitasGenDispon(int id_servicio)
        {
            List<DisponibDto> citasGenerales = new List<DisponibDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT " +
             "u.id_usu idUsuario, " +
             "s.nombre nombreServicio, " +
             "di.nombre Dia, " +
             "h.nom_hora Hora, " +
             "d.id_dispon idDispon, " +
             "s.id_servicio IdServicio " +
             "FROM serv_dispon sd " +
             "JOIN disponibilidad d ON sd.id_dispon = d.id_dispon " +
             "JOIN usuario u ON d.id_usu = u.id_usu " +
             "JOIN servicio s ON sd.id_servicio = s.id_servicio " +
             "JOIN dia di ON d.id_dia = di.id_dia " +
             "JOIN hora h ON d.id_hora = h.id_hora " +
             "WHERE s.id_estado = 1 AND u.id_rol = 2 AND sd.id_servicio = @id_servicio";//En donde sea el asistente quien público la dispon

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_servicio", id_servicio);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DisponibDto dispon = new DisponibDto
                        {
                            id_usu = reader.GetInt32(0),
                            nom_serv = reader.GetString(1),                          
                            nom_dia = reader.GetString(2),
                            nom_hora = reader.GetString(3),
                            id_dispon = reader.GetInt32(4),
                            id_servicio = reader.GetInt32(5)
                        };
                        citasGenerales.Add(dispon);
                    }
                }
            }

            return citasGenerales;
        }
        public List<CitaGeneralDto> HistorialCitasGenerales(int id_usu)
        {
            List<CitaGeneralDto> citasGenerales = new List<CitaGeneralDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT " +          
             "cg.id_cita_gen, " +
             "di.nombre, " +
             "h.nom_hora, " +
             "e.nom_estado, " +
             "m.nom_masc, " +
             "s.nombre " +
             "FROM cita_general cg " +
             "JOIN disponibilidad d ON cg.id_dispon = d.id_dispon " +
             "JOIN mascota m ON cg.id_mascota = m.id_mascota " +
             "JOIN servicio s ON cg.id_servicio = s.id_servicio " +
             "JOIN dia di ON d.id_dia = di.id_dia " +
             "JOIN hora h ON d.id_hora = h.id_hora " +
             "JOIN estado e ON cg.id_estado = e.id_estado " +
             "WHERE m.id_usu = @id_usu"; //Las citas generales del usuario 

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_usu", id_usu);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CitaGeneralDto citaGeneral = new CitaGeneralDto
                        {
                            id_cita_gen = reader.GetInt32(0),                           
                            nom_dia = reader.GetString(1),
                            nom_hora = reader.GetString(2),
                            nom_estado = reader.GetString(3),
                            nom_masc = reader.GetString(4),
                            nom_serv= reader.GetString(5),
                            
                        };
                        citasGenerales.Add(citaGeneral);
                    }
                }
            }

            return citasGenerales;
        }
        public List<CitaEspecDto> HistorialCitasEspec(int id_usu)
        {
            List<CitaEspecDto> citasGenerales = new List<CitaEspecDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT ce.id_cita_esp, mo.nombre, s.nombre, ce.diagnost, ce.fecha_cita, " +
               "h.nom_hora, e.nom_estado, m.nom_masc, ce.recomen,  u.nom_usu + ' ' + u.apel_usu " +
               "FROM cita_espec ce " +
               "JOIN disponibilidad d ON ce.id_dispon = d.id_dispon " +
               "JOIN mascota m ON ce.id_mascota = m.id_mascota " +
                "JOIN usuario u ON d.id_usu = u.id_usu " +
               "JOIN servicio s ON ce.id_servicio = s.id_servicio " +
               "JOIN dia di ON d.id_dia = di.id_dia " +
               "JOIN hora h ON d.id_hora = h.id_hora " +
               "JOIN estado e ON ce.id_estado = e.id_estado " +
               "JOIN motivo mo ON ce.id_motivo = mo.id_motivo " +
               "WHERE m.id_usu = @id_usu;"; //Las citas generales del usuario 

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_usu", id_usu);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CitaEspecDto citaGeneral = new CitaEspecDto
                        {
                            id_cita_esp = reader.GetInt32(0),
                            nom_motivo = reader.GetString(1),
                            nom_serv = reader.GetString(2),
                            diagnost = reader.GetString(3),
                            fecha_cita = reader.GetDateTime(4),
                            nom_hora = reader.GetString(5),
                            nom_estado = reader.GetString(6),
                            nom_masc = reader.GetString(7),
                            recomen = reader.GetString(8),
                            nom_usu = reader.GetString(9),

                        };
                        citasGenerales.Add(citaGeneral);
                    }
                }
            }

            return citasGenerales;
        }



    }
}