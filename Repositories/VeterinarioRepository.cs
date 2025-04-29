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

            string SQL = @"
                        INSERT INTO disponibilidad (id_dia, id_hora, id_usu) VALUES (@id_dia, @id_hora, @id_usu); 
                        SELECT SCOPE_IDENTITY();"; // Devuelve el ID recién creado

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_dia", dispo.id_dia);
                command.Parameters.AddWithValue("@id_hora", dispo.id_hora);
                command.Parameters.AddWithValue("@id_usu", dispo.id_usu);

                var result = command.ExecuteScalar(); // Obtiene el id de la disponibilidad
                if (result != null)
                {
                    id_disponGenerado = Convert.ToInt32(result);
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
        



    }
}