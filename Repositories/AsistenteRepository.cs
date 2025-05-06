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

       
    }
}