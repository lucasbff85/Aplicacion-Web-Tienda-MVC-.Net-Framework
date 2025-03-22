using CapaEntidad;
using CapaEntidad.Paypal;
using CapaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using CapaPresentacionTienda.Filter;
using DocumentFormat.OpenXml.Math;

namespace CapaPresentacionTienda.Controllers
{

    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idproducto = 0)
        {
            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar().Where(p => p.Id == idproducto).FirstOrDefault();
            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
            }


            return View(oProducto);
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categoria().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarMarcaporCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaporCategoria(idcategoria);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p => p.oCategoria.Id == (idcategoria == 0 ? p.oCategoria.Id : idcategoria) &&
                          p.oMarca.Id == (idmarca == 0 ? p.oMarca.Id : idmarca) &&
                          p.Stock > 0 && p.Activo == true).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue; //le decimos que no tenga limites en cuanto a su longitud



            return jsonresult;
        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            int idusuario = ((Usuario)Session["Usuario"]).Id;
            bool existe = new CN_Carrito().ExisteCarrito(idusuario, idproducto);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idusuario, idproducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idusuario = ((Usuario)Session["Usuario"]).Id;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idusuario);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idusuario = ((Usuario)Session["Usuario"]).Id;
            List<Carrito> oLista = new List<Carrito>();
            bool conversion;

            oLista = new CN_Carrito().ListarProducto(idusuario).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    Id = oc.oProducto.Id,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad
            }).ToList();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idusuario = ((Usuario)Session["Usuario"]).Id;
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Carrito().OperacionCarrito(idusuario, idproducto, sumar, out mensaje);


            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idusuario = ((Usuario)Session["Usuario"]).Id;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idusuario, idproducto);
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerCiudad(int idProvincia)
        {
            List<Ciudad> oLista = new List<Ciudad>();
            oLista = new CN_Ubicacion().ObtenerCiudad(idProvincia);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProvincias()
        {
            List<Provincia> oLista = new List<Provincia>();
            oLista = new CN_Ubicacion().ListarProvincias();

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        

        [ValidarSession]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = CultureInfo.InvariantCulture;
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            List<Item> oListaItem = new List<Item>();

            foreach (Carrito oCarrito in oListaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oProducto.Precio;

                total += subtotal;

                oListaItem.Add(new Item()
                {
                    name = oCarrito.oProducto.Nombre,
                    quantity = oCarrito.Cantidad,
                    //quantity = oCarrito.Cantidad.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = oCarrito.oProducto.Precio.ToString("G", CultureInfo.InvariantCulture)
                    }
                });

                detalle_venta.Rows.Add(new object[]
                {
                    oCarrito.oProducto.Id,
                    oCarrito.Cantidad,
                    subtotal
                });
            }

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString("G", CultureInfo.InvariantCulture),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = total.ToString("G", CultureInfo.InvariantCulture)
                        }
                    }
                },
                description = "compra de artículo de Techno Things",
                items = oListaItem
            };

            Checkout_Order oCheckOutOrder = new Checkout_Order()
            {
                intent = "CAPTURE", //ya que solo hacemos una solicitud
                purchase_units = new List<PurchaseUnit>() { purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "Techno Things",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44340/Tienda/PagoEfectuado", //paypal adjunta un token a esta url que luego utilizamos en PagoEfectuado()
                    cancel_url = "https://localhost:44340/Tienda/Carrito"    //url en caso de que el cliente cancele la operacion
                }
            };

            oVenta.MontoTotal = total;
            oVenta.IdUsuario = ((Usuario)Session["Usuario"]).Id;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;


            
            //ejecutar servicios de Paypal
            CN_Paypal opaypal = new CN_Paypal();
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();
            //recibir la respuesta de nuestro método de crear solicitud
            response_paypal = await opaypal.CrearSolicitud(oCheckOutOrder);

            //pasamos datos por defecto, pero esto lo va a manejar el servicio de paypal de manera dinamica
            return Json(response_paypal, JsonRequestBehavior.AllowGet);
        }


        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string tokken = Request.QueryString["token"];

            CN_Paypal opaypal = new CN_Paypal();
            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();
            //recibir la respuesta de la ejecucion del metodo de aprobar pago:
            response_paypal = await opaypal.AprobarPago(tokken);

            ViewData["Status"] = response_paypal.Status;
            if (response_paypal.Status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];
                //usamos el id de transaccion de la respuesta de la ejecucion de nuestro servicio de paypal
                oVenta.IdTransaccion = response_paypal.Response.purchase_units[0].payments.captures[0].id;
                string mensaje = string.Empty;
                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;

            }
            return View();
        }


        [ValidarSession]
        [Authorize]
        public ActionResult MisCompras()  //es un action result porque va a devolver una vista
        {
            int idcliente = ((Usuario)Session["Usuario"]).Id;
            List<DetalleVenta> oLista = new List<DetalleVenta>();
            bool conversion;

            oLista = new CN_Venta().ListarCompras(idcliente).Select(oc => new DetalleVenta()
            {
                oProducto = new Producto()
                {
                    Nombre = oc.oProducto.Nombre,
                    Precio = oc.oProducto.Precio,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                IdTransaccion = oc.IdTransaccion

            }).ToList();
            return View(oLista);
        }

    }
}