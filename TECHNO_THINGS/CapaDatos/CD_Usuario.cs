﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Usuario
    {


        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select Id,Nombre,Apellido,Correo,Clave,Reestablecer,Activo,IsAdmin from USUARIO";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                IsAdmin = Convert.ToBoolean(dr["IsAdmin"])
                            });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Usuario>();
            }
            return lista;
        }


        public List<Usuario> ListarUsuarios(bool isAdmin)
        {
            List<Usuario> lista = new List<Usuario>();



            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ListarUsuarios", oconexion);
                    cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Usuario>();
            }
            return lista;
        }





        public int Registrar(Usuario obj, out string Mensaje, bool isAdmin)
        {
            int idautogenerado = 0;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellido);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("IsAdmin", isAdmin);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;

            }

            return idautogenerado;
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.Id);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellido);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
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

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top (1) from usuario where Id = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    //ExecuteNonQuery() devuelve el numero de filas afectadas
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {

                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update USUARIO set clave = @nuevaclave, Reestablecer = 0 where Id = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", idusuario);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);

                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    //ExecuteNonQuery() devuelve el numero de filas afectadas
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {

                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool ReestablecerClave(int idusuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update USUARIO set clave = @clave, Reestablecer = 1 where Id = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", idusuario);
                    cmd.Parameters.AddWithValue("@clave", clave);

                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    //ExecuteNonQuery() devuelve el numero de filas afectadas
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {

                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

    }
}
