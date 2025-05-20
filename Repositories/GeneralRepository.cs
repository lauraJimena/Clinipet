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
             "(nom_usu, apel_usu, num_ident, correo_usu, tel_usu, contras_usu, id_rol, id_estado, id_nivel, id_tipo_ident, id_espec, cambio_contras) " +
             "VALUES (@nom_usu, @apel_usu, @num_ident, @correo_usu, @tel_usu, @contras_usu, @id_rol, @id_estado, @id_nivel, @id_tipo_ident, @id_espec, @cambio_contras)";

            

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
                command.Parameters.AddWithValue("@cambio_contras", usu.cambio_contras);

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
        

        public bool ExisteDocumento(string documento)
        {
            bool existe = false;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "SELECT COUNT(*) FROM usuario WHERE num_ident = @num_ident";
            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@num_ident", documento);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    existe = true; // El numIdent ya existe
                }
            }

            Connection.Disconnect();
            return existe;
        }

        public UserDto Login(string num_ident)
        {
            UserDto userResult = new UserDto();

            string SQL = "SELECT id_usu, nom_usu, apel_usu, num_ident, correo_usu, tel_usu, contras_usu, id_rol, id_estado, id_nivel, id_tipo_ident, id_espec, cambio_contras " +
                         "FROM usuario " +
                         "WHERE num_ident = @num_ident";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@num_ident", num_ident);

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
                        userResult.cambio_contras = reader.GetBoolean(reader.GetOrdinal("cambio_contras"));

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
        public int RegistrarMascota(MascotaDto masc)
        {

            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "INSERT INTO [clinipet].[dbo].[mascota] " +
             "(nom_masc, edad_masc, id_raza, id_tipo, id_usu) " +
             "VALUES (@nom_masc, @edad_masc, @id_raza, @id_tipo, @id_usu)";


            System.Diagnostics.Debug.WriteLine("Hola mascota " + masc.nom_masc);
            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@nom_masc", masc.nom_masc);
                command.Parameters.AddWithValue("@edad_masc", masc.edad_masc);
                command.Parameters.AddWithValue("@id_raza", masc.id_raza);
                command.Parameters.AddWithValue("@id_tipo", masc.id_tipo);
                command.Parameters.AddWithValue("@id_usu", masc.id_usu);
                comando = command.ExecuteNonQuery();

            }
            System.Diagnostics.Debug.WriteLine(comando);
            Connection.Disconnect();
            return comando;
        }
        public List<MascotaDto> ObtenerRazas()
        {
            List<MascotaDto> razas= new List<MascotaDto>();

            string sql = "SELECT r.id_raza, r.nom_raza, t.nom_tipo " +
                 "FROM [clinipet].[dbo].[raza_masc] r " +
                 "JOIN [clinipet].[dbo].[tipo_masc] t ON r.id_tipo = t.id_tipo"; // Unir con la tabla de tipos

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
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
            return razas;
        }
        public List<MascotaDto> ObtenerTipos()
        {
            List<MascotaDto> razas = new List<MascotaDto>();

            string sql = "SELECT id_tipo, nom_tipo " +
                             "FROM [clinipet].[dbo].[tipo_masc] ";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MascotaDto tipo = new MascotaDto
                        {
                            id_tipo = reader.GetByte(0), //TinyInt se usa Byte
                            nom_tipo = reader.GetString(1),
                            

                        };
                        razas.Add(tipo);
                    }

                }
            }
            return razas;
        }

        public List<UserDto> ObtenerEspecialidad()
        {
            List<UserDto> especialidad = new List<UserDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT e.id_espec, e.nom_espec " +
                 "FROM [clinipet].[dbo].[especialidad] e ";


            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        especialidad.Add(new UserDto
                        //UserDto espec = new UserDto
                        {
                            id_espec = reader.GetInt16(0),
                            nom_espec = reader.GetString(1),
                        });
                    }
                    Connection.Disconnect();
                    return especialidad;
                }
            }
        }
        public List<DisponibDto> ObtenerCitasEspecDispon()
        {
            List<DisponibDto> lista = new List<DisponibDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = "SELECT " +
             "e.nom_espec AS EspecialidadServicio, " +
             "u.nom_usu + ' ' + u.apel_usu AS Veterinario, " +
             "di.nombre AS Dia, " +
             "h.nom_hora AS Hora, " +
             "d.id_dispon AS idDispon " +
             "FROM serv_dispon sd " +
             "JOIN disponibilidad d ON sd.id_dispon = d.id_dispon " +
             "JOIN usuario u ON d.id_usu = u.id_usu " +
             "JOIN servicio s ON sd.id_servicio = s.id_servicio " +
             "JOIN especialidad e ON u.id_espec = e.id_espec " +
             "JOIN dia di ON d.id_dia = di.id_dia " +
             "JOIN hora h ON d.id_hora = h.id_hora " +
             "WHERE s.id_estado = 1 AND d.id_estado=1 AND u.id_rol = 4";//En donde sea el veterinario quien público la dispon

            using (SqlCommand cmd = new SqlCommand(sql, Connection.CONN()))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DisponibDto dispon = new DisponibDto
                        {

                            nom_serv = reader.GetString(0),
                            nom_usu = reader.GetString(1),
                            nom_dia = reader.GetString(2),
                            nom_hora = reader.GetString(3),
                            id_dispon = reader.GetInt32(4)
                        };
                        lista.Add(dispon);
                    }
                }
            }

            return lista;
        }

        //Cambiar contraseña
        public bool CambiarContraseña(string numIdent, string contrasenaActual, string nuevaContrasena)
        {
            bool cambioExitoso = false;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            // Primero, verificamos si la contraseña actual es correcta
            string SQL = "SELECT contras_usu FROM usuario WHERE num_ident = @num_ident";
            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@num_ident", numIdent);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hashAlmacenado = reader.GetString(0);
                        if (EncriptContrasUtility.VerificaContras(contrasenaActual, hashAlmacenado))
                        {
                            // Cerrar el reader antes de reutilizar la conexión
                            reader.Close();

                            // Hashear la nueva contraseña con PBKDF2
                            string hashNueva = EncriptContrasUtility.EncripContras(nuevaContrasena);

                            string updateSQL = "UPDATE usuario SET contras_usu = @contras_nueva, cambio_contras = 0 WHERE num_ident = @num_ident";
                            using (SqlCommand updateCommand = new SqlCommand(updateSQL, Connection.CONN()))
                            {
                                updateCommand.Parameters.AddWithValue("@contras_nueva", hashNueva);
                                updateCommand.Parameters.AddWithValue("@num_ident", numIdent);
                                int rowsAffected = updateCommand.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    cambioExitoso = true;
                                }
                            }
                        }
                    }
                }
            }

            Connection.Disconnect();
            return cambioExitoso;
        }
        //Para obtener el correo del usuario
        public UserDto ObtenerUsuarioPorIdentidad(string num_ident)
        {
            string sql = "SELECT id_usu, correo_usu, contras_usu " +
                         "FROM usuario " +
                         "WHERE num_ident = @num_ident";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@num_ident", num_ident);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserDto
                        {
                            id_usu = reader.GetInt32(0),
                            correo_usu = reader.GetString(1),
                            contras_usu = reader.GetString(2)
                        };
                    }
                }
            }

            return null; // Si no se encuentra el usuario
        }
        public int GuardarTokenRecuperacion(int idUsuario, string token, DateTime fechaExpiracion)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "INSERT INTO [clinipet].[dbo].[recuperacion_contrasena] " +
                         "(IdUsuario, Token, FechaCreacion, FechaExpiracion) " +
                         "VALUES (@IdUsuario, @Token, @FechaCreacion, @FechaExpiracion)";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                command.Parameters.AddWithValue("@Token", token);
                command.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                command.Parameters.AddWithValue("@FechaExpiracion", fechaExpiracion);

                comando = command.ExecuteNonQuery();
            }

            Connection.Disconnect();
            return comando;
        }
        public int RestablecerContrasena(int idUsuario, string nuevaContrasena)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "UPDATE usuario SET contras_usu = @NuevaContrasena WHERE id_usu = @IdUsuario";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@NuevaContrasena", nuevaContrasena);
                command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                comando = command.ExecuteNonQuery();
            }

            Connection.Disconnect();
            return comando;
        }
        public int? ObtenerIdUsuarioPorToken(string token)
        {
            int? idUsuario = null;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "SELECT IdUsuario FROM [clinipet].[dbo].[recuperacion_contrasena] " +
                         "WHERE Token = @Token AND FechaExpiracion > GETDATE()";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@Token", token);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    idUsuario = reader.GetInt32(0);
                }

                reader.Close();
            }

            Connection.Disconnect();
            return idUsuario;
        }








    }




}
