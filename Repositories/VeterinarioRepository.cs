using Clinipet.Dtos;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinipet.Repositories
{
    public class VeterinarioRepository
    {
        public int PublicarDispon(DisponibDto dispo)
        {
            //int comando = 0;
            int id_disponGenerado = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            //Este insert usa un trigger para evitar disponibilidades duplicadas
            string SQL = @"
                        INSERT INTO disponibilidad (id_dia, id_hora, id_usu, id_estado) VALUES (@id_dia, @id_hora, @id_usu, @id_estado); 
                        SELECT SCOPE_IDENTITY();"; // Devuelve el ID recién creado
            try
            {
                using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
                {
                command.Parameters.AddWithValue("@id_dia", dispo.id_dia);
                command.Parameters.AddWithValue("@id_hora", dispo.id_hora);
                command.Parameters.AddWithValue("@id_usu", dispo.id_usu);
                command.Parameters.AddWithValue("@id_estado", dispo.id_estado);

                var result = command.ExecuteScalar(); // Obtiene el id de la disponibilidad
                    if (result != null && result != DBNull.Value)
                    {
                        id_disponGenerado = Convert.ToInt32(result);
                    }
                    else
                    {
                        // No se insertó, pero pudo haberse reactivado entonces busca el ID
                        string SQLBuscar = @"
                        SELECT id_dispon FROM disponibilidad 
                        WHERE id_dia = @id_dia AND id_hora = @id_hora AND id_usu = @id_usu AND id_estado = 1";

                        using (SqlCommand buscar = new SqlCommand(SQLBuscar, Connection.CONN()))
                        {
                            buscar.Parameters.AddWithValue("@id_dia", dispo.id_dia);
                            buscar.Parameters.AddWithValue("@id_hora", dispo.id_hora);
                            buscar.Parameters.AddWithValue("@id_usu", dispo.id_usu);

                            var idExistente = buscar.ExecuteScalar();
                            if (idExistente != null && idExistente != DBNull.Value)
                            {
                                id_disponGenerado = Convert.ToInt32(idExistente);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Si el trigger lanza un error se atrapa aquí
                if (ex.Message.Contains("Ya existe una disponibilidad activa"))
                {
                    
                    throw new Exception("Ya publicaste una disponibilidad para ese día y hora.");
                }
                else
                {
                    // Otros errores de base de datos
                    throw new Exception("Error al publicar la disponibilidad: " + ex.Message);
                }
            }

            return id_disponGenerado;
        }


        public int RegistrarServicio_Dispon(ServicioDto servicio)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            // Obtener el id_espec del veterinario que publicó la disponibilidad
            int id_espec = 0;
            string queryServicio = @"
            SELECT u.id_espec
            FROM disponibilidad d
            JOIN usuario u ON d.id_usu = u.id_usu
            WHERE d.id_dispon = @id_dispon";

            using (SqlCommand cmdServicio = new SqlCommand(queryServicio, Connection.CONN()))
            {
                cmdServicio.Parameters.AddWithValue("@id_dispon", servicio.id_dispon);
                var result = cmdServicio.ExecuteScalar();
                
                if (result != null)
                {
                    id_espec = Convert.ToInt16(result);
                }
            }

            // Insertar en la tabla intermedia serv_dispon con el id_espec obtenido
            if (id_espec > 0)
            {
                string SQL = "INSERT INTO [clinipet].[dbo].[serv_dispon] (id_servicio, id_dispon) " +
                    "VALUES (@id_servicio, @id_dispon)";
                using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
                {
                    command.Parameters.AddWithValue("@id_servicio", id_espec);
                    command.Parameters.AddWithValue("@id_dispon", servicio.id_dispon);
                    comando = command.ExecuteNonQuery();
                }
            }

            return comando;
        }
        public int ActualizarDescripConsulta(CitaEspecDto citaEspec)
        {

            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "UPDATE cita_espec SET id_motivo = @id_motivo, " +
                "diagnost = @diagnost, recomen = @recomen, id_estado = @id_estado " +
                "WHERE id_cita_esp = @id_cita_esp";

            using (SqlCommand cm = new SqlCommand(SQL, Connection.CONN()))
            {

                cm.Parameters.AddWithValue("@id_motivo", citaEspec.id_motivo);
                cm.Parameters.AddWithValue("@diagnost", citaEspec.diagnost);
                cm.Parameters.AddWithValue("@recomen", citaEspec.recomen);
                cm.Parameters.AddWithValue("@id_estado", citaEspec.id_estado);
                cm.Parameters.AddWithValue("@id_cita_esp", citaEspec.id_cita_esp);


                comando = cm.ExecuteNonQuery();
            }

            return comando;

        }
        //Obtener las citas especializadas (que esten agendadas o en curso ) de la mascota seleccionada
        public List<CitaEspecDto> ObtenerCitasEspecAgend(int id_usu, int id_mascota)
        {
            List<CitaEspecDto> lista = new List<CitaEspecDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT " +
             "ce.id_cita_esp, " +
             "s.nombre AS Servicio, " +
             "u.nom_usu + ' ' + u.apel_usu AS Veterinario, " +
             "di.nombre AS Dia, " +
             "h.nom_hora AS Hora, " +
             "d.id_dispon AS idDispon " +
             "FROM cita_espec ce " +
             "JOIN disponibilidad d ON ce.id_dispon = d.id_dispon " +
             "JOIN mascota m ON ce.id_mascota = m.id_mascota " +
             "JOIN usuario u ON d.id_usu = u.id_usu " +
             "JOIN servicio s ON ce.id_servicio = s.id_servicio " +
             "JOIN dia di ON d.id_dia = di.id_dia " +
             "JOIN hora h ON d.id_hora = h.id_hora " +
             "WHERE (ce.id_estado = 3 OR ce.id_estado = 4) AND d.id_usu = @id_usu AND ce.id_mascota = @id_mascota";

            using (SqlCommand cmd = new SqlCommand(sql, Connection.CONN()))
            {
                cmd.Parameters.AddWithValue("@id_usu", id_usu);
                cmd.Parameters.AddWithValue("@id_mascota", id_mascota);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CitaEspecDto dispon = new CitaEspecDto
                        {
                            id_cita_esp = reader.GetInt32(0),           
                            nom_serv = reader.GetString(1),
                            nom_usu = reader.GetString(2),
                            nom_dia = reader.GetString(3),
                            nom_hora = reader.GetString(4),
                            id_dispon = reader.GetInt32(5)
                        };
                        lista.Add(dispon);
                    }
                }
            }

            return lista;
        }
        //Historial de todas las citas que tiene el veterinario (agendadas, en curso y completadas)
        public List<CitaEspecDto> ObtenerHistorialCitas(int id_usu)
        {
            List<CitaEspecDto> lista = new List<CitaEspecDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT di.nombre, h.nom_hora, m.nom_masc, CONCAT(u.nom_usu,' ', u.apel_usu), mo.nombre, ce.fecha_cita, e.nom_estado " +
                  "FROM cita_espec ce " +
                  "JOIN disponibilidad d ON ce.id_dispon=d.id_dispon " +
                  "JOIN dia di ON d.id_dia=di.id_dia " +
                  "JOIN hora h ON d.id_hora=h.id_hora " +
                  "JOIN mascota m ON ce.id_mascota= m.id_mascota " +
                  "JOIN usuario u ON m.id_usu = u.id_usu " +
                  "JOIN motivo mo ON ce.id_motivo = mo.id_motivo " +
                  "JOIN estado e ON ce.id_estado = e.id_estado " +
                  "WHERE d.id_usu=@id_usu ";

            using (SqlCommand cmd = new SqlCommand(sql, Connection.CONN()))
            {
                cmd.Parameters.AddWithValue("@id_usu", id_usu);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CitaEspecDto dispon = new CitaEspecDto
                        {
                                                     
                            nom_dia = reader.GetString(0),
                            nom_hora = reader.GetString(1),
                            nom_masc = reader.GetString(2),
                            nom_usu = reader.GetString(3),
                            nom_motivo = reader.GetString(4),
                            fecha_cita = reader.GetDateTime(5),
                            nom_estado = reader.GetString(6),
                        };
                        lista.Add(dispon);
                    }
                }
            }

            return lista;
        }
        //Obtener el historial médico de una mascota con su ID (citas ya completadas para saber su descrip)
        public List<CitaEspecDto> ObtenerHistorialMascota(int id_usu)
        {
            List<CitaEspecDto> lista = new List<CitaEspecDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT ce.id_cita_esp, s.nombre, " +
               "u.nom_usu + ' ' + u.apel_usu, u.num_ident, mo.nombre, " +
               "ce.diagnost, ce.recomen, m.nom_masc, h.nom_hora, ce.fecha_cita, d.id_dispon " +
               "FROM cita_espec ce " +
               "JOIN disponibilidad d ON ce.id_dispon = d.id_dispon " +
               "JOIN mascota m ON ce.id_mascota = m.id_mascota " +
               "JOIN usuario u ON m.id_usu = u.id_usu " +
               "JOIN servicio s ON ce.id_servicio = s.id_servicio " +
               "JOIN motivo mo ON ce.id_motivo = mo.id_motivo " +
               "JOIN hora h ON d.id_hora = h.id_hora " +
               "WHERE ce.id_estado = 5  " + //Citas completadas
               "AND d.id_usu = @id_usu ";

            using (SqlCommand cmd = new SqlCommand(sql, Connection.CONN()))
            {
                cmd.Parameters.AddWithValue("@id_usu", id_usu);
                //cmd.Parameters.AddWithValue("@id_mascota", id_mascota);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CitaEspecDto dispon = new CitaEspecDto
                        {

                            id_cita_esp = reader.GetInt32(0),
                            nom_serv = reader.GetString(1),
                            nom_usu = reader.GetString(2),
                            num_ident= reader.GetString(3),
                            nom_motivo = reader.GetString(4),
                            diagnost = reader.GetString(5),
                            recomen = reader.GetString(6),
                            nom_masc = reader.GetString(7),
                            nom_hora = reader.GetString(8),
                            fecha_cita = reader.GetDateTime(9),
                            id_dispon = reader.GetInt32(10),
                        };
                        lista.Add(dispon);
                    }
                }
            }

            return lista;
        }
        public List<DisponibDto> ObtenerDisponib(int id_usu)
        {
            List<DisponibDto> lista = new List<DisponibDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT di.nombre, h.nom_hora, CONCAT(u.nom_usu, ' ', u.apel_usu) " +
               "FROM disponibilidad d " +
               "JOIN dia di ON d.id_dia = di.id_dia " +
               "JOIN hora h ON d.id_hora = h.id_hora " +
               "JOIN usuario u ON d.id_usu = u.id_usu " +
               "WHERE d.id_estado = 1 AND d.id_usu = @id_usu";

            using (SqlCommand cmd = new SqlCommand(sql, Connection.CONN()))
            {
                cmd.Parameters.AddWithValue("@id_usu", id_usu);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DisponibDto dispon = new DisponibDto
                        {

                            nom_dia = reader.GetString(0),
                            nom_hora = reader.GetString(1),                           
                            nom_usu = reader.GetString(2)
                            
                        };
                        lista.Add(dispon);
                    }
                }
            }

            return lista;
        }


        public List<MascotaDto> ListadoMascotas(string num_ident)

        {
            List<MascotaDto> mascotas = new List<MascotaDto>();


            string sql = "SELECT r.nom_raza, t.nom_tipo, m.nom_masc, m.edad_masc, m.id_mascota, u.nom_usu " +
                 "FROM [clinipet].[dbo].[raza_masc] r " +
                 "JOIN [clinipet].[dbo].[tipo_masc] t ON r.id_tipo = t.id_tipo " +
                 "JOIN [clinipet].[dbo].[mascota] m ON m.id_raza = r.id_raza " +
                 "JOIN [clinipet].[dbo].[usuario] u ON m.id_usu = u.id_usu " +
                  "WHERE u.num_ident = @num_ident";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@num_ident", num_ident);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MascotaDto mascota = new MascotaDto
                        {
                            nom_raza = reader.GetString(0),
                            nom_tipo = reader.GetString(1),
                            nom_masc = reader.GetString(2),
                            edad_masc = reader.GetInt16(3),
                            id_mascota = reader.GetInt32(4),
                            nom_usu = reader.GetString(5)

                        };
                        mascotas.Add(mascota);
                    }

                }
            }
            return mascotas;
        }

        public List<DisponibDto> ObtenerDias()
        {
            List<DisponibDto> lista = new List<DisponibDto>();
            string sql = "SELECT id_dia, nombre FROM dia";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new DisponibDto
                        {
                            id_dia = reader.GetByte(0),
                            nom_dia = reader.GetString(1),
                        });
                    }
                }
            }
            return lista;
        }
        public List<DisponibDto> ObtenerHoras()
        {
            List<DisponibDto> listaHoras = new List<DisponibDto>();
            string sql = "SELECT id_hora, nom_hora FROM hora";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaHoras.Add(new DisponibDto
                        {
                            id_hora = reader.GetInt16(0),
                            nom_hora = reader.GetString(1),
                        });
                    }
                }
            }
            return listaHoras;
        }
        public List<DisponibDto> ObtenerServiciosGen()
        {
            List<DisponibDto> listaServ = new List<DisponibDto>();
            string sql = "SELECT id_servicio, nombre FROM servicio WHERE tipo_servicio=0";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaServ.Add(new DisponibDto
                        {
                            id_servicio = reader.GetInt32(0), 
                            nom_serv = reader.GetString(1) 
                        });
                    }
                }
            }
            return listaServ;
        }
        public List<CitaEspecDto> ObtenerMotivo()
        {
            List<CitaEspecDto> listaMotivos = new List<CitaEspecDto>();
            string sql = "SELECT id_motivo, nombre FROM motivo";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaMotivos.Add(new CitaEspecDto
                        {
                            id_motivo = reader.GetInt32(0),
                            nom_motivo = reader.GetString(1),
                        });
                    }
                }
            }
            return listaMotivos;
        }
        public UserDto ObtenerUsuarioPorNumIdent(string num_ident)
        {
            
            
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT id_usu, nom_usu FROM usuario WHERE num_ident = @num_ident";

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@num_ident", num_ident);
                       
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserDto
                            {
                                id_usu= reader.GetInt32(0),
                                nom_usu = reader.GetString(1),
                                

                            };
                        }
                    }
            }   
         return null; 
        }
        //Obtener mascotas que han tenido citas con el veterinario
        public List<MascotaDto> ObtenerMascotas(int id_usu)

        {
            List<MascotaDto> mascotas = new List<MascotaDto>();


            string sql = "SELECT u.nom_usu + ' ' + u.apel_usu, m.nom_masc, " +
               "t.nom_tipo, r.nom_raza, ce.id_mascota " +
               "FROM cita_espec ce " +
               "JOIN disponibilidad d ON ce.id_dispon = d.id_dispon " +
               "JOIN mascota m ON ce.id_mascota = m.id_mascota " +
               "JOIN tipo_masc t ON m.id_tipo = t.id_tipo " +
               "JOIN raza_masc r ON m.id_raza = r.id_raza " +
               "JOIN usuario u ON m.id_usu = u.id_usu " +
               "WHERE d.id_usu = @id_usu" ;
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
                            
                            nom_usu = reader.GetString(0),                           
                            nom_masc = reader.GetString(1),
                            nom_tipo = reader.GetString(2),
                            nom_raza = reader.GetString(3),
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