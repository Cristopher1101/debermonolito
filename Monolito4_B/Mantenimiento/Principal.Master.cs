using System;
using System.Text;
using System.Web.UI;
using capa_negocio;
using Capa_Datos;

namespace Monolito4_B.Mantenimiento
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        private static readonly string PlaceholderSidebarAvatar =
            "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='52' height='52'%3E%3Ccircle cx='26' cy='26' r='26' fill='%23334155'/%3E%3Ctext x='50%25' y='54%25' dominant-baseline='middle' text-anchor='middle' font-family='system-ui' font-size='22' fill='%2394a3b8'%3E%E2%80%A2%3C/text%3E%3C/svg%3E";

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

                if (!CN_tbl_usuario.UsuarioSigueValido(usuId))
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
                    lblRolBadge.Text = "Administrador";
                    lblRolBadge.CssClass = "role-pill role-admin";
                    lbl_nse.Text = nombre;
                    adm.Visible = true;
                    usu.Visible = false;
                }
                else
                {
                    lblRolBadge.Text = "Usuario";
                    lblRolBadge.CssClass = "role-pill";
                    lbl_nse.Text = nombre;
                    adm.Visible = false;
                    usu.Visible = true;
                }

                CargarAvatarSidebar(usuId);
            }
            catch
            {
                try { Session.Clear(); } catch { /* ignore */ }
                Response.Redirect("~/Seguridad/Login.aspx", false);
            }
        }

        private void CargarAvatarSidebar(int usuId)
        {
            if (imgSidebarAvatar == null)
                return;
            imgSidebarAvatar.ImageUrl = PlaceholderSidebarAvatar;
            tbl_usuario u = CN_tbl_usuario.ObtenerPorId(usuId);
            if (u?.usu_foto_perfil != null && u.usu_foto_perfil.Length > 0)
                imgSidebarAvatar.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(u.usu_foto_perfil.ToArray());
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
