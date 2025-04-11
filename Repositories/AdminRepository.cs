using System;
using System.Data.SqlClient;
using Clinipet.Dtos;
using Clinipet.Utilities;

namespace Clinipet.Repositories
{
    public class AdminRepository
    {
    /*    public int RegistrarAsistente(UserDto asistente)
        {
            int resultado = 0;
            DBContextUtility db = new DBContextUtility();
            db.Connect();

            string sql = "INSERT INTO [clinipet].[dbo].[usuario] " +
                         "(nom_usu, apel_usu, num_ident, correo_usu, tel_usu, contras_usu, id_rol, id_estado, id_nivel, id_tipo_ident, id_espec) " +
                         "VALUES (@nom_usu, @apel_usu, @num_ident, @correo_usu, @tel_usu, @contras_usu, @id_rol, @id_estado, @id_nivel, @id_tipo_ident, @id_espec)";

            using (SqlCommand cmd = new SqlCommand(sql, db.CONN()))
            {
                cmd.Parameters.AddWithValue("@nom_usu", asistente.nom_usu);
                cmd.Parameters.AddWithValue("@apel_usu", asistente.apel_usu);
                cmd.Parameters.AddWithValue("@num_ident", asistente.num_ident);
                cmd.Parameters.AddWithValue("@correo_usu", asistente.correo_usu);
                cmd.Parameters.AddWithValue("@tel_usu", asistente.tel_usu);
                cmd.Parameters.AddWithValue("@contras_usu", asistente.contras_usu);
                cmd.Parameters.AddWithValue("@id_rol", asistente.id_rol);
                cmd.Parameters.AddWithValue("@id_estado", asistente.id_estado);
                cmd.Parameters.AddWithValue("@id_nivel", asistente.id_nivel);
                cmd.Parameters.AddWithValue("@id_tipo_ident", asistente.id_tipo_ident);
                cmd.Parameters.AddWithValue("@id_espec", asistente.id_espec);

                resultado = cmd.ExecuteNonQuery();
            }

            db.Disconnect();
            return resultado;
        }
     */
        public bool ExisteCorreo(string correo)
        {
            bool existe = false;
            DBContextUtility db = new DBContextUtility();
            db.Connect();

            string sql = "SELECT COUNT(*) FROM usuario WHERE correo_usu = @correo";

            using (SqlCommand cmd = new SqlCommand(sql, db.CONN()))
            {
                cmd.Parameters.AddWithValue("@correo", correo);
                int count = (int)cmd.ExecuteScalar();
                existe = count > 0;
            }

            db.Disconnect();
            return existe;
        }

        public bool ExisteDocumento(string documento)
        {
            bool existe = false;
            DBContextUtility db = new DBContextUtility();
            db.Connect();

            string sql = "SELECT COUNT(*) FROM usuario WHERE num_ident = @doc";

            using (SqlCommand cmd = new SqlCommand(sql, db.CONN()))
            {
                cmd.Parameters.AddWithValue("@doc", documento);
                int count = (int)cmd.ExecuteScalar();
                existe = count > 0;
            }

            db.Disconnect();
            return existe;
        }
    }
}
