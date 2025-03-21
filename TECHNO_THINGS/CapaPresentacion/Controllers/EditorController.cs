using CapaNegocio;
using CapaEntidad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using System.Data;
using ClosedXML.Excel;

namespace CapaPresentacion.Controllers
{
    public class EditorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Ubicaciones()
        {
            return View();
        }

        public ActionResult Usuarios()
        {

            return View();
        }

        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        //---------------------------UBICACIONES----------------------
        #region UBICACIONES



        [HttpPost]
        public JsonResult ListarProvincias()
        {
            List<Provincia> oLista = new List<Provincia>();
            oLista = new CN_Ubicacion().ListarProvincias();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public JsonResult ListarProvincias()
        //{
        //    List<Provincia> oLista = new List<Provincia>();
        //    oLista = new CN_Ubicacion().ListarProvincias();

        //    return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        //}



        [HttpPost]
        public JsonResult ListarCiudades()
        {
            List<Ciudad> oLista = new List<Ciudad>();
            oLista = new CN_Ubicacion().ListarCiudades();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult GuardarProvincia(Provincia objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.Id == 0)
            {
                resultado = new CN_Ubicacion().RegistrarProvincia(objeto, out mensaje);

            }
            else
            {
                resultado = new CN_Ubicacion().EditarProvincia(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProvincia(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Ubicacion().EliminarProvincia(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarCiudad(Ciudad objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.Id == 0)
            {
                resultado = new CN_Ubicacion().RegistrarCiudad(objeto, out mensaje);

            }
            else
            {
                resultado = new CN_Ubicacion().EditarCiudad(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCiudad(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Ubicacion().EliminarCiudad(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //---------------------------CATEGORIA--------------------------

        #region CATEGORIA

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if(objeto.Id == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);

            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Categoria().Eliminar(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        //-----------------------------MARCA----------------------------

        #region MARCA

        [HttpGet] 
        public JsonResult ListarMarcas()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = new CN_Marca().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.Id == 0)
            {
                resultado = new CN_Marca().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Marca().Eliminar(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //-----------------------------PRODUCTO----------------------------

        #region PRODUCTO

        [HttpGet]
        public JsonResult ListarProductos()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Producto().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto); //OJO ERRORES CON FORMATO DEL PRECIO

            //decimal precio = 0;

            //if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-AR"), out precio))
            //{
            //    // primer parámetro toma el PrecioTexto, luego le da el formato, y el tercer parámetro es la variable precio donde lo va a guardar


            //    oProducto.Precio = precio;

            //}
            //else
            //{
            //    return Json(new { operacion_exitosa = false, mensaje = "Error editor controller" }, JsonRequestBehavior.AllowGet);
            //}

            if (oProducto.Id == 0)
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if (idProductoGenerado > 0)
                {
                    oProducto.Id = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.Id.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {

                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)
                    {
                        oProducto.RutaImagen = ruta_guardar;
                        oProducto.NombreImagen = nombre_imagen;
                        bool respuesta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardó el producto pero hubo problemas con la imagen";
                    }

                }

            }

            return Json(new { operacion_exitosa = operacion_exitosa, idGenerado = oProducto.Id, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            //los objetos lista tiene ese metodo where
            Producto oProducto = new CN_Producto().Listar().Where(p => p.Id == id).FirstOrDefault();
            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.NombreImagen)
            },
            JsonRequestBehavior.AllowGet
            );

        }


        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Producto().Eliminar(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion



        //---------------------------USUARIOS---------------------
        #region  USUARIOS

        [HttpGet]
        public JsonResult ListarAdmin()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuario().ListarUsuarios(true);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarClientes()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuario().ListarUsuarios(false);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.Id == 0)
            {
                resultado = new CN_Usuario().Registrar(objeto, out mensaje,true);
            }
            else
            {
                resultado = new CN_Usuario().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Usuario().Eliminar(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        //---------------------------REPORTES---------------------
        #region REPORTES


        [HttpGet]
        public JsonResult ListaReporte(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();

            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);

            DataTable dt = new DataTable();

            //le asignamos la cultura de la region
            dt.Locale = new CultureInfo("es-AR");

            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Id Transacción", typeof(string));

            foreach (Reporte rp in oLista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {

                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    //guardamos el documento
                    wb.SaveAs(stream);

                    //ahora le damos como parametros: ruta del archivo, tipo de archivo (archivo de excel), nombre de archivo :
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                    //stream.ToArray() es donde esta almacenado el archivo
                    //"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" para especificar que es un archivo excel
                    // "ReporteVenta" + DateTime.Now.ToString() + ".xlsx"  es el nombre del archivo
                }

            }
        }


        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            DashBoard objeto = new CN_Reporte().VerDashBoard();
            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}