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
           SELECT * FROM dbo.Fn_CitasPorServicioEspecializado()
           ORDER BY total_citas DESC";

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

            string sql = @"SELECT * FROM dbo.Fn_CitasPorServicioGeneral()
                        ORDER BY total_citas DESC";


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

        //Obtiene numero de citas por veterinario con función Fn_CitasPorVeterinario()
        public List<VeterinarioCitasDto> ObtenerCitasVet()
        {
            string sql = "SELECT * FROM dbo.Fn_CitasPorVeterinario()";

            List<VeterinarioCitasDto> lista = new List<VeterinarioCitasDto>();

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(sql, Connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new VeterinarioCitasDto
                        {
                            IdVeterinario = reader.GetInt32(0),
                            NombreVeterinario = reader.GetString(1),
                            Espec = reader.GetString(2),
                            TotalCitas = reader.GetInt32(3)
                            
                        });
                    }
                }
            }

            return lista;
        }


    }
}