using System;
using System.Web.UI;
using capa_negocio;
using Capa_Datos;

namespace Monolito4_B.Mantenimiento
{
    public partial class Inicio : Page
    {
        private static readonly string PlaceholderDashAvatar =
            "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='88' height='88'%3E%3Ccircle cx='44' cy='44' r='44' fill='rgba(255,255,255,0.2)'/%3E%3Ctext x='50%25' y='54%25' dominant-baseline='middle' text-anchor='middle' font-family='system-ui,sans-serif' font-size='36' fill='%23fff'%3E%E2%80%A2%3C/text%3E%3C/svg%3E";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CargarFotoPerfilDashboard();

                    var stats = capa_negocio.CN_tbl_usuario.ObtenerEstadisticas();
                    lblActivos.Text = stats.activos.ToString();
                    lblBloq.Text = stats.bloqueados.ToString();
                    lblTotal.Text = stats.total.ToString();

                    int tusu = Session["tusu_id"] != null ? Convert.ToInt32(Session["tusu_id"]) : 0;
                    pnlAdminInfo.Visible = tusu == 1;
                    pnlUsuarioInfo.Visible = tusu != 1;
                }
                catch (Exception ex)
                {
                    lblErrorDash.Visible = true;
                    lblErrorDash.Text = "No se pudieron cargar las estadísticas: " + Server.HtmlEncode(ex.Message);
                }
            }
        }

        private void CargarFotoPerfilDashboard()
        {
            if (imgDashAvatar == null)
                return;
            imgDashAvatar.ImageUrl = PlaceholderDashAvatar;
            if (Session["usuario"] == null || !int.TryParse(Session["usuario"].ToString(), out int uid))
                return;
            tbl_usuario u = CN_tbl_usuario.ObtenerPorId(uid);
            if (u?.usu_foto_perfil != null && u.usu_foto_perfil.Length > 0)
                imgDashAvatar.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(u.usu_foto_perfil.ToArray());
        }
    }
}
