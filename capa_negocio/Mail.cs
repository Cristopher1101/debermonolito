using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace capa_negocio
{
    public class Mail
    {
        MailMessage m = new MailMessage();
        SmtpClient smtp = new SmtpClient();
        string from = "cristoferazuero@gmail.com";
        string password = "password";



        public bool enviar_correo(string to, string msj)
        {
            try
            {
                m.From = new MailAddress(from);
                m.To.Add(new MailAddress(to);
                m.Body = msj;
                m.Subject = "Recuperacion de Contraseña";
                m.IsBodyHtml = true;

                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(from, password);
                smtp.EnableSsl = true;
                smtp.Send(m);
                smtp.Dispose();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
                throw;
            }
        }
    }
}