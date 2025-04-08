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
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "INSERT INTO [clinipet].[dbo].[disponibilidad] " +
                         "(id_dia, id_hora, id_usu) " +
                         "VALUES (@id_dia, @id_hora, @id_usu)";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_dia", dispo.id_dia);
                command.Parameters.AddWithValue("@id_hora", dispo.id_hora);
                command.Parameters.AddWithValue("@id_usu", dispo.id_usu);

                comando = command.ExecuteNonQuery();
            }

            return comando;
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
        

    }
}