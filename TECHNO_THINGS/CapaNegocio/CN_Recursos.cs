﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Net.NetworkInformation;

namespace CapaNegocio
{
    public class CN_Recursos
    {

        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }



        //encriptación de texto en SHA256
        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            //USAR LA REFERENCIA DE "System.Security.Cryptography"
            using (SHA256 hash = SHA256Managed.Create())
            {

                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }


        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("pruebasprogrlbff@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()  //se encarga de mandar el email
                {
                    Credentials = new NetworkCredential("pruebasprogrlbff@gmail.com", "oisv gvyf walk doot"),
                    Host = "smtp.gmail.com", //el servidor que utiliza gmail para enviar los correos
                    Port = 587,
                    EnableSsl = true  //habilitar el certificado de seguridad
                };
                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex)
            {

                resultado = false;
            }

            return resultado;
        }



        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch
            {

                conversion = false;
            }
            return textoBase64;
        }


    }
}
