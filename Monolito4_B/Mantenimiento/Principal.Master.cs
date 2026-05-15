using System;
using System.Text;
using System.Web.UI;
using capa_negocio;

namespace Monolito4_B.Mantenimiento
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;

            try
            {
                if (Session["usuario"] == null || Session["tusu_id"] == null)
                {
                    Response.Redirect("~/Seguridad/Login.aspx", false);
                    return;
                }

                if (!int.TryParse(Session["usuario"].ToString(), out int usuId) ||
                    !int.TryParse(Session["tusu_id"].ToString(), out int tusuId))
                {
                    Session.Clear();
                    Response.Redirect("~/Seguridad/Login.aspx", false);
                    return;
                }

                if (!capa_negocio.CN_tbl_usuario.UsuarioSigueValido(usuId))
                {
                    Session.Clear();
                    Response.Redirect("~/Seguridad/Login.aspx?msg=sess", false);
                    return;
                }

                bool esAdmin = tusuId == 1;
                bool tieneMarcaAdmin = Session["adm"] != null;
                bool tieneMarcaUsuario = Session["usu"] != null;

                if (esAdmin && !tieneMarcaAdmin)
                {
                    Session.Clear();
                    Response.Redirect("~/Seguridad/Login.aspx", false);
                    return;
                }
                if (!esAdmin && !tieneMarcaUsuario)
                {
                    Session.Clear();
                    Response.Redirect("~/Seguridad/Login.aspx", false);
                    return;
                }

                string nombre = Session["NombreCompleto"]?.ToString();
                if (string.IsNullOrEmpty(nombre))
                    nombre = tieneMarcaAdmin ? Session["adm"].ToString() : Session["usu"].ToString();

                if (esAdmin)
                {
                    lbl_nse.Text = "Administrador: " + nombre;
                    adm.Visible = true;
                    usu.Visible = false;
                }
                else
                {
                    lbl_nse.Text = "Usuario: " + nombre;
                    adm.Visible = false;
                    usu.Visible = true;
                }
            }
            catch
            {
                try { Session.Clear(); } catch { /* ignore */ }
                Response.Redirect("~/Seguridad/Login.aspx", false);
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            BrowserSensitiveCookies.ExpirarLegacyRememberCedula(Response);
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Seguridad/Login.aspx", false);
        }
    }
}
