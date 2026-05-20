using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using capa_negocio;

namespace Monolito4_B.Mantenimiento
{
    public partial class Desbloqueo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tusu_id"] == null || Convert.ToInt32(Session["tusu_id"]) != 1)
            {
                Response.Redirect("~/Mantenimiento/Inicio.aspx", false);
                return;
            }

            if (!IsPostBack)
                CargarGrid();
        }

        private void CargarGrid()
        {
            gvBloqueados.DataSource = CN_tbl_usuario.ListarUsuariosBloqueados().ToList();
            gvBloqueados.DataBind();
        }

        protected void gvBloqueados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Desbloquear") return;

            int usuId = int.Parse(e.CommandArgument.ToString());

            if (CN_tbl_usuario.DesbloquearUsuario(usuId))
            {
                lblMsg.Text = "<p style='color:#2e7d32'>Usuario desbloqueado.</p>";
            }
            else
            {
                lblMsg.Text = "<p style='color:#c62828'>No se pudo desbloquear.</p>";
            }

            CargarGrid();
        }
    }
}
