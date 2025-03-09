using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteCarrito(int idusuario, int idproducto)
        {
            bool resultado = true;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", idusuario);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }

            }
            catch (Exception ex)
            {
                resultado = false;

            }

            return resultado;
        }


        public bool OperacionCarrito(int idusuario, int idproducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", idusuario);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }

            return resultado;
        }


        public int CantidadEnCarrito(int idusuario)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from CARRITO where IdUsuario = @idusuario", oconexion);
                    cmd.Parameters.AddWithValue("@idusuario", idusuario);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    //ExecuteNonQuery() devuelve el numero de filas afectadas
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {

                resultado = 0;
            }
            return resultado;
        }

        public List<Carrito> ListarProducto(int idusuario)
        {
            List<Carrito> lista = new List<Carrito>();


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_obtenerCarritoUsuario(@IdUsuario)";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", idusuario);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Carrito()
                            {
                                oProducto = new Producto()
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-AR")),
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString(),
                                    oMarca = new Marca() { Descripcion = dr["DesMarca"].ToString() }
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"])
                            });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Carrito>();
            }
            return lista;
        }

        public bool EliminarCarrito(int idusuario, int idproducto)
        {
            bool resultado = true;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", idusuario);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }

            }
            catch (Exception)
            {
                resultado = false;

            }

            return resultado;
        }

    }
}
