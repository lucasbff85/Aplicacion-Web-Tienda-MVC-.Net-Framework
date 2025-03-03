using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Ubicacion
    {


        public List<Provincia> ListarProvincias()
        {
            List<Provincia> lista = new List<Provincia>();


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from PROVINCIA";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Provincia()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Provincia>();
            }
            return lista;
        }


        public List<Ciudad> ObtenerCiudad(int idprovincia)
        {
            List<Ciudad> lista = new List<Ciudad>();


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from CIUDAD where IdProvincia = @IdProvincia";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idprovincia", idprovincia);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Ciudad()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                IdProvincia = Convert.ToInt32(dr["IdProvincia"]),
                                CP = dr["CP"].ToString()
                            });
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Ciudad>();
            }
            return lista;
        }
    }
}
