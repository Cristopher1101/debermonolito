using Capa_Datos;
using System;
using System.Linq;

namespace Monolito4_B.Mantenimiento
{
    public partial class listar_tbl_proveedor : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProveedores();
            }
        }

        private void CargarProveedores()
        {
            var listaProveedores = from p in db.tbl_proveedor
                                   orderby p.prov_estado ascending, p.prov_id ascending
                                   select new
                                   {
                                       p.prov_id,
                                       p.prov_nombre,
                                       p.prov_telefono,
                                       p.prov_direccion,
                                       p.prov_estado
                                   };

            gvProveedores.DataSource = listaProveedores.ToList();
            gvProveedores.DataBind();
        }

        protected void btnLogico_Click(object sender, EventArgs e)
        {
            try
            {
                int cod = Convert.ToInt32(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument);
                var obj = db.tbl_proveedor.FirstOrDefault(p => p.prov_id == cod);
                if (obj != null)
                {
                    // Alternar estado lógico
                    obj.prov_estado = (obj.prov_estado == 'A') ? 'I' : 'A';
                    db.SubmitChanges();
                    
                    // Mostrar Label
                    var lblMensaje = (System.Web.UI.WebControls.Label)UpdatePanel1.FindControl("lblMensaje");
                    if(lblMensaje != null) {
                        lblMensaje.Visible = true;
                        lblMensaje.Text = "¡Estado lógico actualizado correctamente!";
                        lblMensaje.ForeColor = System.Drawing.Color.Green;
                    }
                    CargarProveedores();
                }
            }
            catch (Exception ex)
            {
                 var lblMensaje = (System.Web.UI.WebControls.Label)UpdatePanel1.FindControl("lblMensaje");
                 if(lblMensaje != null) {
                      lblMensaje.Visible = true;
                      lblMensaje.Text = "Error al cambiar estado: " + ex.Message;
                      lblMensaje.ForeColor = System.Drawing.Color.Red;
                 }
            }
        }

        protected void btnFisico_Click(object sender, EventArgs e)
        {
            try
            {
                int cod = Convert.ToInt32(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument);
                var obj = db.tbl_proveedor.FirstOrDefault(p => p.prov_id == cod);
                if (obj != null)
                {
                    // Al eliminar físicamente un proveedor, seteamos en NULL el prov_id de sus productos
                    db.ExecuteCommand("UPDATE tbl_producto SET prov_id = NULL WHERE prov_id = {0}", cod);
                    
                    // Borrado físico
                    db.tbl_proveedor.DeleteOnSubmit(obj);
                    db.SubmitChanges();
                    
                    // Mostrar Label
                    var lblMensaje = (System.Web.UI.WebControls.Label)UpdatePanel1.FindControl("lblMensaje");
                    if(lblMensaje != null) {
                        lblMensaje.Visible = true;
                        lblMensaje.Text = "¡Proveedor eliminado físicamente por completo!";
                        lblMensaje.ForeColor = System.Drawing.Color.Green;
                    }
                    CargarProveedores();
                }
            }
            catch (Exception ex)
            {
                 var lblMensaje = (System.Web.UI.WebControls.Label)UpdatePanel1.FindControl("lblMensaje");
                 if(lblMensaje != null) {
                      lblMensaje.Visible = true;
                      lblMensaje.Text = "Error al eliminar físico: " + ex.Message;
                      lblMensaje.ForeColor = System.Drawing.Color.Red;
                 }
            }
        }
    }
}