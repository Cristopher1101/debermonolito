using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace capa_negocio
{
    /// <summary>
    /// Envío SMTP (Gmail). Toda la configuración está aquí; no se usa Web.config para SMTP.
    /// Si cambias de cuenta o contraseña de aplicación, edita solo las constantes de abajo.
    /// </summary>
    public class Mail
    {
        /// <summary>Mensaje del último fallo de envío (sin incluir la contraseña).</summary>
        public string UltimoErrorEnvio { get; private set; }

        // === Configuración SMTP (Gmail) — edita solo estas líneas ===
        private const string CorreoRemitenteYUsuario = "cristoferazuero@gmail.com";
        /// <summary>Contraseña de aplicación tal como la muestra Google (con o sin espacios).</summary>
        private const string ContrasenaAplicacionGoogle = "rclx rats zkta uwni";
        private const string ServidorSmtp = "smtp.gmail.com";
        private const int PuertoSmtp = 587;
        private const bool UsarSsl = true;
        private const int TimeoutMilisegundos = 60000;

        private struct ConfiguracionSmtp
        {
            public string Host;
            public int Port;
            public string CorreoRemitente;
            public string UsuarioAutenticacion;
            public string Password;
            public bool EnableSsl;
            public int TimeoutMs;
        }

        private static string QuitarEspaciosContrasena(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return string.Empty;
            var sb = new StringBuilder(raw.Length);
            foreach (char c in raw)
            {
                if (!char.IsWhiteSpace(c)) sb.Append(c);
            }
            return sb.ToString();
        }

        private static ConfiguracionSmtp ObtenerConfiguracionFija()
        {
            return new ConfiguracionSmtp
            {
                Host = ServidorSmtp,
                Port = PuertoSmtp,
                CorreoRemitente = CorreoRemitenteYUsuario,
                UsuarioAutenticacion = CorreoRemitenteYUsuario,
                Password = QuitarEspaciosContrasena(ContrasenaAplicacionGoogle),
                EnableSsl = UsarSsl,
                TimeoutMs = TimeoutMilisegundos
            };
        }

        private bool ValidarConfig(ref ConfiguracionSmtp cfg)
        {
            UltimoErrorEnvio = null;
            if (string.IsNullOrWhiteSpace(cfg.Host))
            {
                UltimoErrorEnvio = "SMTP: host no configurado en Mail.cs.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(cfg.CorreoRemitente) || string.IsNullOrWhiteSpace(cfg.UsuarioAutenticacion))
            {
                UltimoErrorEnvio = "SMTP: correo/usuario no configurado en Mail.cs.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(cfg.Password))
            {
                UltimoErrorEnvio = "SMTP: contraseña de aplicación vacía en Mail.cs.";
                return false;
            }
            return true;
        }

        private static SmtpClient CrearClienteSmtp(ConfiguracionSmtp cfg)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var smtp = new SmtpClient(cfg.Host, cfg.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(cfg.UsuarioAutenticacion, cfg.Password),
                EnableSsl = cfg.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = cfg.TimeoutMs
            };

            // Gmail en 587 usa STARTTLS; en .NET esto evita fallos de autenticación/canal en algunos entornos.
            try
            {
                smtp.TargetName = "STARTTLS/smtp.gmail.com";
            }
            catch
            {
                // Propiedad no disponible en runtimes muy antiguos (no debería ocurrir en 4.8.1).
            }

            return smtp;
        }

        public bool enviar_correo(string to, string msj)
        {
            try
            {
                ConfiguracionSmtp cfg = ObtenerConfiguracionFija();
                if (!ValidarConfig(ref cfg))
                    return false;

                using (var m = new MailMessage())
                {
                    m.From = new MailAddress(cfg.CorreoRemitente);
                    m.To.Add(new MailAddress(to));
                    m.Body = msj;
                    m.Subject = "Recuperación de contraseña";
                    m.IsBodyHtml = true;
                    m.BodyEncoding = Encoding.UTF8;
                    m.SubjectEncoding = Encoding.UTF8;

                    using (SmtpClient smtp = CrearClienteSmtp(cfg))
                    {
                        smtp.Send(m);
                    }
                }
                UltimoErrorEnvio = null;
                return true;
            }
            catch (Exception ex)
            {
                UltimoErrorEnvio = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        /// <summary>OTP por correo con QR generado en servidor (imagen incrustada) y enlace escaneable.</summary>
        public bool enviar_correo_otp_con_qr(string to, string otp, string nombreUsuario, string urlEscaneoQrAbsoluta)
        {
            try
            {
                ConfiguracionSmtp cfg = ObtenerConfiguracionFija();
                if (!ValidarConfig(ref cfg))
                    return false;

                byte[] pngQr = QrPngService.GenerarPng(urlEscaneoQrAbsoluta, 6);

                string htmlBody = @"
<div style='font-family: Arial, sans-serif; max-width: 520px; margin: auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 10px; text-align: center; background-color: #f9f9f9;'>
  <h2 style='color: #333;'>Monolito 4B — Seguridad</h2>
  <p style='color: #555; font-size: 16px;'>Hola <b>" + System.Web.HttpUtility.HtmlEncode(nombreUsuario ?? "") + @"</b>,</p>
  <p style='color: #555; font-size: 16px;'>Código de verificación (también válido al escanear el QR):</p>
  <h1 style='letter-spacing: 6px; color: #2e7d32; background: #fff; padding: 15px; border-radius: 8px; border: 2px dashed #2e7d32; display: inline-block;'>" + System.Web.HttpUtility.HtmlEncode(otp ?? "") + @"</h1>
  <p style='color: #555; font-size: 14px; margin-top: 20px;'>Puede escanear este QR con la cámara (por ejemplo desde otro teléfono) para completar el acceso con el mismo OTP:</p>
  <img src='cid:otpqr' alt='QR OTP' style='border: 1px solid #ccc; border-radius: 10px; padding: 10px; background: white;' />
  <hr style='border: 0; border-top: 1px solid #ddd; margin: 24px 0;' />
  <p style='font-size: 12px; color: #999;'>Si usted no inició sesión, ignore este mensaje.</p>
</div>";

                using (var m = new MailMessage())
                using (var smtp = CrearClienteSmtp(cfg))
                using (var ms = new MemoryStream(pngQr))
                {
                    m.From = new MailAddress(cfg.CorreoRemitente);
                    m.To.Add(new MailAddress(to));
                    m.Subject = "Código OTP — Monolito 4B";
                    m.SubjectEncoding = Encoding.UTF8;
                    m.BodyEncoding = Encoding.UTF8;
                    m.IsBodyHtml = true;

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
                    var linked = new LinkedResource(ms, "image/png")
                    {
                        ContentId = "otpqr",
                        TransferEncoding = TransferEncoding.Base64
                    };
                    htmlView.LinkedResources.Add(linked);
                    m.AlternateViews.Add(htmlView);

                    smtp.Send(m);
                }
                UltimoErrorEnvio = null;
                return true;
            }
            catch (Exception ex)
            {
                UltimoErrorEnvio = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        /// <summary>Correo con el código OTP en HTML, sin imagen QR.</summary>
        public bool enviar_correo_otp_solo_texto(string to, string otp, string nombreUsuario)
        {
            try
            {
                ConfiguracionSmtp cfg = ObtenerConfiguracionFija();
                if (!ValidarConfig(ref cfg))
                    return false;

                string html = @"
<div style='font-family: Arial,sans-serif;max-width:480px;margin:auto;padding:20px;'>
  <p>Hola <b>" + System.Web.HttpUtility.HtmlEncode(nombreUsuario ?? "") + @"</b>,</p>
  <p>Su código de verificación OTP es:</p>
  <p style='font-size:28px;letter-spacing:10px;font-weight:bold;color:#1b5e20;'>" + System.Web.HttpUtility.HtmlEncode(otp ?? "") + @"</p>
  <p style='font-size:12px;color:#666;'>Si no solicitó este acceso, ignore este mensaje.</p>
</div>";

                using (var m = new MailMessage())
                using (var smtp = CrearClienteSmtp(cfg))
                {
                    m.From = new MailAddress(cfg.CorreoRemitente);
                    m.To.Add(new MailAddress(to));
                    m.Subject = "Código OTP — Monolito 4B";
                    m.SubjectEncoding = Encoding.UTF8;
                    m.Body = html;
                    m.BodyEncoding = Encoding.UTF8;
                    m.IsBodyHtml = true;

                    smtp.Send(m);
                }
                UltimoErrorEnvio = null;
                return true;
            }
            catch (Exception ex)
            {
                UltimoErrorEnvio = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }
    }
}
