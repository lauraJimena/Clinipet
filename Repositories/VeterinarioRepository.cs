using Clinipet.Dtos;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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

    }
}