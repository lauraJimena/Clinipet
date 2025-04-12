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
        public List<DisponibDto> ObtenerCitasDisponibles()
        {
            List<DisponibDto> lista = new List<DisponibDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string sql = @"
        SELECT 
        s.nombre EspecialidadServicio,
        u.nom_usu + ' ' + u.apel_usu Veterinario,
        di.nombre AS Dia,
        h.nom_hora AS Hora
        FROM serv_dispon sd
        JOIN disponibilidad d ON sd.id_dispon = d.id_dispon
        JOIN usuario u ON d.id_usu = u.id_usu
        JOIN servicio s ON sd.id_servicio = s.id_servicio
        JOIN dia di ON d.id_dia=di.id_dia
        JOIN hora h ON d.id_hora = h.id_hora
        WHERE s.id_estado = 1";

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
                            nom_hora = reader.GetString(3)
                        };
                        lista.Add(dispon);
                    }
                }
            }

            return lista;
        }


    }




}
