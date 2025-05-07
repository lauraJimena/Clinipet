using Clinipet.Dtos;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Clinipet.Repositories
{
    public class ReporteRepository
    {
        //Método para obtner los servicios generales más prestados
        public List<ServicioDto> ObtenerServGeneralesMasPrestados()
        {
            List<ServicioDto> listaServicios = new List<ServicioDto>();

            string sql = @"
            SELECT s.nombre, COUNT(c.id_cita_esp) AS total_citas
            FROM cita_espec c
            JOIN servicio s ON c.id_servicio = s.id_servicio
            WHERE c.id_estado IN (3,4,5) AND s.tipo_servicio = 1
            GROUP BY s.nombre
            ORDER BY total_citas DESC;";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaServicios.Add(new ServicioDto
                        {
                            nom_serv = reader.GetString(0),
                            total_citas = reader.GetInt32(1)
                        });
                    }
                }
            }
            return listaServicios;
        }
        //Método para obtner los servicios especializados más prestados
        public List<ServicioDto> ObtenerServEspecMasPrestados()
        {
            List<ServicioDto> listaServicios = new List<ServicioDto>();

            string sql = @"
            SELECT s.nombre, COUNT(c.id_cita_gen) AS total_citas
            FROM cita_general c
            JOIN servicio s ON c.id_servicio = s.id_servicio
            WHERE c.id_estado IN (3,4,5) AND s.tipo_servicio = 0
            GROUP BY s.nombre
            ORDER BY total_citas DESC;";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaServicios.Add(new ServicioDto
                        {
                            nom_serv = reader.GetString(0),
                            total_citas = reader.GetInt32(1)
                        });
                    }
                }
            }
            return listaServicios;
        }


    }
}