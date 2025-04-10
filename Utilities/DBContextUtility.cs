using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Clinipet.Utilities
{
    public class DBContextUtility
    {
        //LAURA POSADA
        static string SERVER = "LAURA";  // Nombre de la instancia del servidor SQL
        static string DB_NAME = "clinipet";
        static string DB_USER = "SA";
        static string DB_PASSWORD = "12345";


        //LAURA HERREÑO
        /*static string SERVER = "LAPTOP-8RV6USKP";  // Nombre de la instancia del servidor SQL
        static string DB_NAME = "clinipet";
        static string DB_USER = "userapp";
        static string DB_PASSWORD = "12345";*/

        static readonly string Conn = "server=" + SERVER + ";database=" + DB_NAME + ";user id=" + DB_USER + ";password=" + DB_PASSWORD + ";MultipleActiveResultSets=true";

        SqlConnection Con = new SqlConnection(Conn);

        //abre la conexion sqlsever
        public void Connect()
        {
            try
            {
                Con.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo conectar a la base de datos. Detalles: " + ex.Message);
                //Console.WriteLine(ex.Message);
            }
        }
        //cierra la conexion sqlserver
        public void Disconnect()
        {
            if (Con.State == System.Data.ConnectionState.Open)
            {
                Con.Close();
            }              
        }

        //devuelve la conexion sqlserver
        public SqlConnection CONN()
        { 
            return Con;
        }
    }
    
}