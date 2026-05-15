using Capa_Datos;
using capa_negocio;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Monolito4_B.Seguridad
{
    public partial class Login : System.Web.UI.Page
    {
        private const string SessionCedulaOtp = "CedulaOTP";
        private const string SessionOtpPlano = "LoginOtpPlano";
        private const string SessionOtpExpira = "OtpExpiraUtc";
        private const string SessionRememberPref = "LoginRememberPref";

        private static readonly Regex RxCedula = new Regex(@"^\d{10}$", RegexOptions.Compiled);
        private static readonly Regex RxOtp6 = new Regex(@"^\d{6}$", RegexOptions.Compiled);

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            BrowserSensitiveCookies.ExpirarLegacyRememberCedula(Response);

            if (!IsPostBack)
            {
                try
                {
                    using (var dc = new DataClasses1DataContext())
                    {
                        dc.sp_reiniciarIntentosDiarios();
                    }
                }
                catch
                {
                }

                IrAPantallaLogin();

                if (string.Equals(Request.QueryString["reg"], "1", StringComparison.Ordinal))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "regOk",
                        "alert('Registro completado. Inicie sesión con su cédula y contraseña.');", true);
                }
            }
        }

        private void IrAPantallaLogin()
        {
            pnlLogin.Visible = true;
            pnlOTP.Visible = false;
            litSubtituloOtp.Text = "Ingrese el código de 6 dígitos que le enviamos a su <strong>correo</strong> y a su <strong>WhatsApp</strong>. Si el correo con el QR está en <strong>otro dispositivo</strong>, puede usar el botón <strong>Escanear QR del correo (cámara)</strong> en esta pantalla.";
            txt_otp.Text = string.Empty;
        }

        /// <summary>WhatsApp: solo los 6 dígitos del OTP (sin texto adicional).</summary>
        private bool EnviarOtpPorWhatsApp(tbl_usuario us, string codOtp, out string detalleError)
        {
            detalleError = null;
            if (string.IsNullOrWhiteSpace(us.usu_celular))
            {
                detalleError = "No hay celular registrado.";
                return false;
            }

            bool ok = WhatsAppService.EnviarSoloTextoOtp(us.usu_celular.Trim(), codOtp);
            if (!ok)
                detalleError = "WhatsApp: revise CallMeBotApiKey en Web.config y el número en CallMeBot.";
            return ok;
        }

        /// <summary>Correo: HTML con código y QR (mismo OTP).</summary>
        private bool EnviarOtpPorCorreo(tbl_usuario us, string codOtp, DateTime expiraUtcOtp, out string detalleError)
        {
            detalleError = null;
            if (string.IsNullOrWhiteSpace(us.usu_correo))
            {
                detalleError = "No hay correo registrado.";
                return false;
            }

            string urlQr;
            try
            {
                urlQr = CN_tbl_usuario.ConstruirUrlAbsolutaOtpQrCorreo(Request, us.usu_id, codOtp, expiraUtcOtp);
            }
            catch (Exception ex)
            {
                detalleError = ex.Message;
                return false;
            }

            var mail = new Mail();
            bool ok = mail.enviar_correo_otp_con_qr(us.usu_correo.Trim(), codOtp, us.usu_nombres, urlQr);
            if (!ok)
                detalleError = string.IsNullOrWhiteSpace(mail.UltimoErrorEnvio) ? "Error SMTP." : mail.UltimoErrorEnvio;
            return ok;
        }

        private void MostrarPanelOtpTrasLogin(tbl_usuario us, string codOtp, string mensajeExitoPersonalizado = null)
        {
            DateTime exp = Session[SessionOtpExpira] is DateTime d ? d : DateTime.UtcNow.AddMinutes(15);
            bool okWa = EnviarOtpPorWhatsApp(us, codOtp, out string errWa);
            bool okMail = EnviarOtpPorCorreo(us, codOtp, exp, out string errMail);

            if (!string.IsNullOrEmpty(mensajeExitoPersonalizado))
            {
                litSubtituloOtp.Text = mensajeExitoPersonalizado;
            }
            else if (okWa && okMail)
            {
                litSubtituloOtp.Text = "Se envió el código a su <strong>correo</strong> (con QR y opción de <strong>cámara</strong> en esta pantalla) y a su <strong>WhatsApp</strong> (solo los 6 dígitos). Revise ambos.";
            }
            else if (okMail && !okWa)
            {
                string extra = string.IsNullOrWhiteSpace(errWa) ? string.Empty : " " + Server.HtmlEncode(errWa.Length > 160 ? errWa.Substring(0, 160) + "…" : errWa);
                litSubtituloOtp.Text = "El código se envió por <strong>correo</strong> (QR: use <strong>Escanear QR</strong> con la cámara si el correo está en otro equipo). No se pudo enviar por WhatsApp." + extra + " Puede reenviar o use el código del correo.";
            }
            else if (okWa && !okMail)
            {
                string extra = string.IsNullOrWhiteSpace(errMail) ? string.Empty : " " + Server.HtmlEncode(errMail.Length > 160 ? errMail.Substring(0, 160) + "…" : errMail);
                litSubtituloOtp.Text = "El código se envió por <strong>WhatsApp</strong>. No se pudo enviar por correo." + extra + " Use el código recibido en WhatsApp.";
            }
            else
            {
                string e1 = string.IsNullOrWhiteSpace(errMail) ? "" : " Correo: " + Server.HtmlEncode(errMail.Length > 120 ? errMail.Substring(0, 120) + "…" : errMail);
                string e2 = string.IsNullOrWhiteSpace(errWa) ? "" : " WhatsApp: " + Server.HtmlEncode(errWa.Length > 120 ? errWa.Substring(0, 120) + "…" : errWa);
                litSubtituloOtp.Text = "No se pudo enviar el OTP por correo ni por WhatsApp." + e1 + e2 + " Pulse «Reenviar» o vuelva a iniciar sesión.";
            }

            pnlLogin.Visible = false;
            pnlOTP.Visible = true;
            txt_otp.Text = string.Empty;
        }

        private void LimpiarSesionOtpCompleta()
        {
            string ced = Session[SessionCedulaOtp] as string;
            Session.Remove(SessionCedulaOtp);
            Session.Remove(SessionOtpPlano);
            Session.Remove(SessionOtpExpira);
            if (!string.IsNullOrEmpty(ced))
            {
                try { CN_tbl_usuario.LimpiarOTP(ced); } catch { /* ignorar */ }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string cedRaw = txt_ced.Text != null ? txt_ced.Text.Trim() : string.Empty;
            if (string.IsNullOrEmpty(cedRaw) || !RxCedula.IsMatch(cedRaw))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "vced", "alert('La cédula debe tener exactamente 10 dígitos (solo números).');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_pass.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Por favor, ingrese su contraseña.');", true);
                return;
            }

            if (!CN_tbl_usuario.autentixced(cedRaw))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Usuario no encontrado o cuenta bloqueada/inactiva.');", true);
                return;
            }

            tbl_usuario usinfo = CN_tbl_usuario.traterced(cedRaw);
            if (usinfo == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Usuario no encontrado.');", true);
                return;
            }

            int intentosActuales = usinfo.usu_intentos ?? 0;
            if (intentosActuales >= 3)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Cuenta bloqueada por intentos. Solicite desbloqueo al administrador.');", true);
                return;
            }

            if (!CN_tbl_usuario.autentixpass(cedRaw, txt_pass.Text))
            {
                CN_tbl_usuario.modificarintentos(cedRaw);
                int nuevos = intentosActuales + 1;
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    $"alert('Credenciales incorrectas. Intento {nuevos} de 3.');", true);
                return;
            }

            string codOtp;
            try
            {
                codOtp = CN_tbl_usuario.GenerarYActualizarOtpEncriptado(cedRaw);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertOtpGen",
                    "alert('No se pudo generar el código de verificación. Intente de nuevo.');", true);
                return;
            }

            Session[SessionCedulaOtp] = cedRaw;
            Session[SessionOtpPlano] = codOtp;
            Session[SessionOtpExpira] = DateTime.UtcNow.AddMinutes(15);
            Session[SessionRememberPref] = chkRemember.Checked;

            MostrarPanelOtpTrasLogin(usinfo, codOtp);
        }

        private void EstablecerSesionTrasOtp(tbl_usuario usinfo)
        {
            Session.Remove(SessionCedulaOtp);
            Session.Remove(SessionOtpPlano);
            Session.Remove(SessionOtpExpira);

            bool recordar = Session[SessionRememberPref] is bool b && b;
            Session.Remove(SessionRememberPref);
            Session.Timeout = recordar ? 240 : 20;
            BrowserSensitiveCookies.ExpirarLegacyRememberCedula(Response);

            Session["usuario"] = usinfo.usu_id;
            Session["tusu_id"] = usinfo.tusu_id ?? 0;
            string nombre = $"{usinfo.usu_nombres} {usinfo.usu_apellidos}".Trim();
            Session["NombreCompleto"] = nombre;

            if (usinfo.tusu_id == 1)
            {
                Session["adm"] = nombre;
                Session.Remove("usu");
            }
            else
            {
                Session["usu"] = nombre;
                Session.Remove("adm");
            }
        }

        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            if (Session[SessionCedulaOtp] == null)
            {
                Session.Remove(SessionRememberPref);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Sesión de OTP expirada. Vuelva a iniciar sesión.');", true);
                LimpiarSesionOtpCompleta();
                IrAPantallaLogin();
                return;
            }

            if (Session[SessionOtpExpira] is DateTime exp && DateTime.UtcNow > exp)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('El código OTP expiró. Use Reenviar o vuelva a ingresar.');", true);
                return;
            }

            string otpIngreso = txt_otp.Text != null ? txt_otp.Text.Trim() : string.Empty;
            if (!RxOtp6.IsMatch(otpIngreso))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "votp", "alert('El OTP debe ser exactamente 6 dígitos (solo números).');", true);
                return;
            }

            string cedula = Session[SessionCedulaOtp].ToString();
            tbl_usuario usinfo = CN_tbl_usuario.traterced(cedula);

            if (usinfo == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Usuario no encontrado.');", true);
                return;
            }

            if (!CN_tbl_usuario.VerificarOTPEncriptado(cedula, otpIngreso))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Código incorrecto.');", true);
                return;
            }

            CN_tbl_usuario.LimpiarOTP(cedula);
            EstablecerSesionTrasOtp(usinfo);
            Response.Redirect("~/Mantenimiento/Inicio.aspx", false);
        }

        protected void lnkResend_Click(object sender, EventArgs e)
        {
            if (Session[SessionCedulaOtp] == null)
                return;

            string cedula = Session[SessionCedulaOtp].ToString();
            tbl_usuario usinfo = CN_tbl_usuario.traterced(cedula);
            if (usinfo == null)
                return;

            string codOtp;
            try
            {
                codOtp = CN_tbl_usuario.GenerarYActualizarOtpEncriptado(cedula);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertOtpRe",
                    "alert('No se pudo generar un nuevo código. Intente de nuevo.');", true);
                return;
            }

            Session[SessionOtpPlano] = codOtp;
            Session[SessionOtpExpira] = DateTime.UtcNow.AddMinutes(15);

            MostrarPanelOtpTrasLogin(usinfo, codOtp, null);
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            LimpiarSesionOtpCompleta();
            IrAPantallaLogin();
        }
    }
}
