using System;
using System.Web;
using capa_negocio;
using Capa_Datos;

namespace Monolito4_B.Seguridad
{
    public partial class OtpQr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string t = Request.QueryString["t"];
            if (string.IsNullOrEmpty(t))
            {
                lblMsg.Text = "<p class='err'>Falta el parámetro del código QR.</p>";
                return;
            }

            if (!OtpQrTokenHelper.TryValidarToken(t, out int usuId, out string otpPlano, out string err))
            {
                lblMsg.Text = "<p class='err'>" + HttpUtility.HtmlEncode(err) + "</p>";
                return;
            }

            if (!CN_tbl_usuario.VerificarOTPEncriptadoPorId(usuId, otpPlano))
            {
                lblMsg.Text = "<p class='err'>El código ya no coincide con el servidor (puede haber expirado o se generó uno nuevo).</p>";
                return;
            }

            tbl_usuario u = CN_tbl_usuario.ObtenerPorId(usuId);
            if (u == null || u.usu_estado != 'A')
            {
                lblMsg.Text = "<p class='err'>Usuario no disponible.</p>";
                return;
            }

            CN_tbl_usuario.LimpiarOTPporId(usuId);

            BrowserSensitiveCookies.ExpirarLegacyRememberCedula(Response);
            Session.Clear();
            Session["usuario"] = u.usu_id;
            Session["tusu_id"] = u.tusu_id ?? 0;
            string nombre = (u.usu_nombres + " " + u.usu_apellidos).Trim();
            Session["NombreCompleto"] = nombre;

            if (u.tusu_id == 1)
            {
                Session["adm"] = nombre;
                Session.Remove("usu");
            }
            else
            {
                Session["usu"] = nombre;
                Session.Remove("adm");
            }

            lblMsg.Text = "<p class='ok'>Acceso validado correctamente.</p>";
            string dest = VirtualPathUtility.ToAbsolute("~/Mantenimiento/Inicio.aspx");
            Response.Redirect(dest, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
