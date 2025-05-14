using Clinipet.Dtos;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;

namespace Clinipet.Repositories
{
    public class AsistenteRepository
    {
        public int RegistrarUsuario(UserDto usu)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "SP_RegistrarUsuario";  //Stored Procedure

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;  //Se indica que es un SP

                command.Parameters.AddWithValue("@nom_usu", usu.nom_usu);
                command.Parameters.AddWithValue("@apel_usu", usu.apel_usu);
                command.Parameters.AddWithValue("@num_ident", usu.num_ident);
                command.Parameters.AddWithValue("@correo_usu", usu.correo_usu);
                command.Parameters.AddWithValue("@tel_usu", usu.tel_usu);
                command.Parameters.AddWithValue("@contras_usu", usu.contras_usu);
                command.Parameters.AddWithValue("@id_rol", usu.id_rol);
                command.Parameters.AddWithValue("@id_estado", usu.id_estado);
                command.Parameters.AddWithValue("@id_nivel", usu.id_nivel);
                command.Parameters.AddWithValue("@id_tipo_ident", usu.id_tipo_ident);
                command.Parameters.AddWithValue("@id_espec", usu.id_espec);
                command.Parameters.AddWithValue("@cambio_contras", usu.cambio_contras);

                // Parámetro de retorno
                SqlParameter returnParam = new SqlParameter();
                returnParam.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnParam);

                comando = command.ExecuteNonQuery();  //Se ejecuta el SP

                comando = (int)returnParam.Value;
            }

            Connection.Disconnect();
            return comando;
        }

        public int RegistrarMascota(MascotaDto masc)
        {

            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "SP_RegistrarMascota";

            System.Diagnostics.Debug.WriteLine("Hola mascota " + masc.nom_masc);
            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nom_masc", masc.nom_masc);
                command.Parameters.AddWithValue("@edad_masc", masc.edad_masc);
                command.Parameters.AddWithValue("@id_raza", masc.id_raza);
                command.Parameters.AddWithValue("@id_tipo", masc.id_tipo);
                command.Parameters.AddWithValue("@id_usu", masc.id_usu);

                SqlParameter returnParam = new SqlParameter();
                returnParam.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnParam);

                command.ExecuteNonQuery();
                comando = (int)returnParam.Value;

            }
            System.Diagnostics.Debug.WriteLine(comando);
            Connection.Disconnect();
            return comando;
        }

        public List<MascotaDto> ObtenerRazas()
        {
            List<MascotaDto> razas = new List<MascotaDto>();

            string sql = "SP_ObtenerRazas";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MascotaDto raza = new MascotaDto
                        {
                            id_raza = reader.GetInt16(0),
                            nom_raza = reader.GetString(1),
                            nom_tipo = reader.GetString(2)


                        };
                        razas.Add(raza);
                    }

                }
            }
            Connection.Disconnect();
            return razas;
        }

        public List<MascotaDto> ObtenerTipos()
        {
            List<MascotaDto> tipos = new List<MascotaDto>();

            string sql = "SP_ObtenerTipos";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MascotaDto tipo = new MascotaDto
                        {
                            id_tipo = reader.GetByte(0),
                            nom_tipo = reader.GetString(1)
                        };
                        tipos.Add(tipo);
                    }
                }
            }

            Connection.Disconnect();
            return tipos;
        }

        public List<SelectListItem> ObtenerClientesSelect()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            string sql = "SELECT id_usu, CONCAT(nom_usu, ' ', apel_usu, ' - ', num_ident) AS nombreCompleto FROM usuario WHERE id_rol = 3 AND id_estado = 1";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand cmd = new SqlCommand(sql, Connection.CONN()))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new SelectListItem
                        {
                            Value = reader["id_usu"].ToString(),
                            Text = reader["nombreCompleto"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
        public List<CitaEspecVistaDto> ObtenerCitasEspecializadas()
        {
            List<CitaEspecVistaDto> lista = new List<CitaEspecVistaDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand("SP_ConsultarCitas", Connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        lista.Add(new CitaEspecVistaDto
                        {
                            id_cita_esp = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            nom_mascota = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            nom_servicio = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            nom_veterinario = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            id_dia = reader.IsDBNull(4) ? 0 : reader.GetByte(4),
                            nom_dia = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            nom_hora = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            estado = reader.IsDBNull(7) ? "" : reader.GetString(7)
                        });
                    }
                }
            }

            Connection.Disconnect();
            return lista;
        }

        public void ActualizarEstadoCita(int id_cita_esp, int nuevo_estado)
        {
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            try
            {
                string sql = "UPDATE cita_espec SET id_estado = @nuevo_estado WHERE id_cita_esp = @id_cita_esp";

                using (SqlCommand command = new SqlCommand(sql, connection.CONN()))
                {
                    command.Parameters.AddWithValue("@nuevo_estado", nuevo_estado);
                    command.Parameters.AddWithValue("@id_cita_esp", id_cita_esp);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al actualizar el estado: " + ex.Message);
                throw;
            }
            finally
            {
                connection.Disconnect();
            }
        }

        public List<CitaEspecVistaDto> ObtenerCitasPorDia(int id_dia)
        {
            List<CitaEspecVistaDto> lista = new List<CitaEspecVistaDto>();
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            using (SqlCommand command = new SqlCommand("SP_ObtenerCitasPorDia", connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id_dia", id_dia);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new CitaEspecVistaDto
                        {
                            id_cita_esp = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            nom_mascota = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            nom_servicio = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            nom_veterinario = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            id_dia = reader.IsDBNull(4) ? 0 : reader.GetByte(4),
                            nom_dia = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            nom_hora = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            estado = reader.IsDBNull(7) ? "" : reader.GetString(7)
                        });
                    }
                }
            }

            connection.Disconnect();
            return lista;
        }

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
                            id_servicio = reader.GetInt32(0),
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
                            edad_masc = reader.GetInt16(3),
                            id_mascota = reader.GetInt32(4),

                        };
                        mascotas.Add(mascota);
                    }

                }
            }
            return mascotas;
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

        public List<MascotaDto> BuscarMascotas(string nombreMascota, string cedulaDueno)
        {
            List<MascotaDto> lista = new List<MascotaDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand("SP_BuscarMascotas", Connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NombreMascota", string.IsNullOrEmpty(nombreMascota) ? (object)DBNull.Value : nombreMascota);
                command.Parameters.AddWithValue("@CedulaDueno", string.IsNullOrEmpty(cedulaDueno) ? (object)DBNull.Value : cedulaDueno);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new MascotaDto
                        {
                            id_mascota = reader["id_mascota"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id_mascota"]),
                            nom_masc = reader["nom_masc"] == DBNull.Value ? "" : reader["nom_masc"].ToString(),
                            edad_masc = reader["edad_masc"] == DBNull.Value ? 0 : Convert.ToInt32(reader["edad_masc"]),
                            nom_tipo = reader["nom_tipo"] == DBNull.Value ? "" : reader["nom_tipo"].ToString(),
                            nom_raza = reader["nom_raza"] == DBNull.Value ? "" : reader["nom_raza"].ToString(),
                            nom_usu = reader["NombreDueno"] == DBNull.Value ? "" : reader["NombreDueno"].ToString(),
                            num_ident = reader["num_ident"] == DBNull.Value ? "" : reader["num_ident"].ToString()
                        });
                    }
                }
            }

            Connection.Disconnect();
            return lista;
        }

        public List<MascotaDto> ListarTodasLasMascotas()
        {
            List<MascotaDto> lista = new List<MascotaDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand("SP_ListarTodasLasMascotas", Connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        lista.Add(new MascotaDto
                        {
                            id_mascota = reader["id_mascota"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id_mascota"]),
                            nom_masc = reader["NombreMascota"] == DBNull.Value ? "" : reader["NombreMascota"].ToString(),
                            edad_masc = reader["Edad"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Edad"]),
                            nom_tipo = reader["Tipo"] == DBNull.Value ? "" : reader["Tipo"].ToString(),
                            nom_raza = reader["Raza"] == DBNull.Value ? "" : reader["Raza"].ToString(),
                            nom_usu = reader["NombreDueno"] == DBNull.Value ? "" : reader["NombreDueno"].ToString(),
                            num_ident = reader["CedulaDueno"] == DBNull.Value ? "" : reader["CedulaDueno"].ToString()

                        });
                    }
                }
            }

            Connection.Disconnect();
            return lista;
        }
        public DisponibDto PublicarDisponGen(DisponibDto dispo)
        {
            //int comando = 0;
            int id_disponGenerado = 0;
            int id_serv = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            //Este insert usa un trigger para evitar disponibilidades duplicadas
            string SQL = @"
                         INSERT INTO disponibilidad (id_dia, id_hora, id_usu, id_estado) VALUES (@id_dia, @id_hora, @id_usu, @id_estado);

SELECT id_dispon FROM disponibilidad 
WHERE id_dia = @id_dia AND id_hora = @id_hora AND id_usu = @id_usu AND id_estado = 1;"; // Devuelve el ID recién creado
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
                        dispo.id_dispon = Convert.ToInt32(result);
                    
                    

                            
                                //DisponibDto dispon = new DisponibDto();
                                return dispo;
                            
                        
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

            return null;
        }
        public int RegistrarServicio_Dispon(ServicioDto servicio)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

           
            
                string SQL = "INSERT INTO [clinipet].[dbo].[serv_dispon] (id_servicio, id_dispon) " +
                    "VALUES (@id_servicio, @id_dispon)";
                using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
                {
                    command.Parameters.AddWithValue("@id_servicio", servicio.id_servicio );
                    command.Parameters.AddWithValue("@id_dispon", servicio.id_dispon);
                    comando = command.ExecuteNonQuery();
                }
            

            return comando;
        }


    }
}