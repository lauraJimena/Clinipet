using Clinipet.Dtos;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Clinipet.Repositories
{
    public class GeneralRepository
    {
        //Funcion para registrar usuario con sql
        public int RegistrarUsuario(UserDto usu)
        {

            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "INSERT INTO [clinipet].[dbo].[usuario] " +
             "(nom_usu, apel_usu, num_ident, correo_usu, tel_usu, contras_usu, id_rol, id_estado, id_nivel, id_tipo_ident, id_espec) " +
             "VALUES (@nom_usu, @apel_usu, @num_ident, @correo_usu, @tel_usu, @contras_usu, @id_rol, @id_estado, @id_nivel, @id_tipo_ident, @id_espec)";

            

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
                {
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

                comando = command.ExecuteNonQuery();
                
            }
          
            Connection.Disconnect();
            return comando;
        }
        public bool ExisteCorreo(string correo)
        {
            bool existe = false;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "SELECT COUNT(*) FROM usuario WHERE correo_usu = @correo_usu";//Cuenta todas las filas que cumplen con la condicion

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@correo_usu", correo);
                int count = (int)command.ExecuteScalar(); // Devuelve el número de correos iguales

                if (count > 0)
                {
                    existe = true; // El correo ya existe
                }
            }

            Connection.Disconnect();
            return existe;
        }
        public UserDto Login(UserDto user)
        {
            UserDto userResult = new UserDto();

            string SQL = "SELECT id_usu, nom_usu, apel_usu, num_ident, correo_usu, tel_usu, contras_usu, id_rol, id_estado, id_nivel, id_tipo_ident, id_espec" +
                         " FROM usuario " +
                         "WHERE num_ident = @num_ident AND contras_usu = @contras_usu;";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@num_ident", user.num_ident);
                command.Parameters.AddWithValue("@contras_usu", user.contras_usu);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        System.Diagnostics.Debug.WriteLine("Método de la base de datos");
                        userResult.id_usu = reader.GetInt32(reader.GetOrdinal("id_usu"));
                        userResult.nom_usu = reader.GetString(reader.GetOrdinal("nom_usu"));
                        userResult.apel_usu = reader.GetString(reader.GetOrdinal("apel_usu"));
                        userResult.num_ident = reader.GetString(reader.GetOrdinal("num_ident"));
                        userResult.correo_usu = reader.GetString(reader.GetOrdinal("correo_usu"));
                        userResult.tel_usu = reader.GetString(reader.GetOrdinal("tel_usu"));
                        userResult.contras_usu = reader.GetString(reader.GetOrdinal("contras_usu"));
                        userResult.id_rol = reader.GetInt32(reader.GetOrdinal("id_rol"));
                        userResult.id_estado = reader.GetInt32(reader.GetOrdinal("id_estado"));
                        userResult.id_nivel = reader.GetInt16(reader.GetOrdinal("id_nivel"));
                        userResult.id_tipo_ident = reader.GetInt16(reader.GetOrdinal("id_tipo_ident"));
                        userResult.id_espec = reader.GetInt16(reader.GetOrdinal("id_espec"));
                       
                       

                        userResult.Response = 1;  // Login exitoso
                    }
                    else
                    {
                        userResult.Response = 0;  // Credenciales incorrectas
                        userResult.Mensaje = "Numero de documento o contraseña incorrectos.";
                    }
                }
            }
            Connection.Disconnect();

            return userResult;
        }





    }
}