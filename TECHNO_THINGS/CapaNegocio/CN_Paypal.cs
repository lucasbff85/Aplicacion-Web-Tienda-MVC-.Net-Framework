using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CapaEntidad.Paypal;

namespace CapaNegocio
{
    public class CN_Paypal
    {
        private static string urlpaypal = ConfigurationManager.AppSettings["UrlPaypal"];
        private static string clientId = ConfigurationManager.AppSettings["ClientId"];
        private static string secret = ConfigurationManager.AppSettings["Secret"];


        //crear solicitud de pago dentro de los servicios de Paypal:
        public async Task<Response_Paypal<Response_Checkout>> CrearSolicitud(Checkout_Order orden)
        {
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();

            //Crear una solicitud HttpClient
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlpaypal);

                //crear una autenticación
                var authToken = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                //obtener la respuesta de la ejecución de la api
                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

                //leer la respuesta, fuardamos el valor en la propiedad Status.
                response_paypal.Status = response.IsSuccessStatusCode; //indica si la solicitud tuvo éxito
                                                                       //
                if (response.IsSuccessStatusCode)
                {
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result; //obtenemos la respuesta

                    Response_Checkout checkout = JsonConvert.DeserializeObject<Response_Checkout>(jsonRespuesta);

                    //ahora nuestro response de paypal va a obtener todo el objeto
                    response_paypal.Response = checkout;
                }
                return response_paypal;
            }
        }

        //Crear un método para realizar la aprobación del pago por parte de Paypal:
        public async Task<Response_Paypal<Response_Capture>> AprobarPago(string token)
        {
            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();

            //Crear una solicitud HttpClient
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlpaypal);

                //crear una autenticación
                var authToken = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));


                var data = new StringContent("{}", Encoding.UTF8, "application/json"); //enviamos un json vacio "{}"

                //obtener la respuesta de la ejecución de la api
                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);

                //leer la respuesta, fuardamos el valor en la propiedad Status.
                response_paypal.Status = response.IsSuccessStatusCode; //indica si la solicitud tuvo éxito
                                                                       //
                if (response.IsSuccessStatusCode)
                {
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result; //obtenemos la respuesta

                    Response_Capture capture = JsonConvert.DeserializeObject<Response_Capture>(jsonRespuesta);

                    //ahora nuestro response de paypal va a obtener todo el objeto
                    response_paypal.Response = capture;
                }
                return response_paypal;
            }
        }
    }
}
