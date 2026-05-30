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

        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                int cod = Convert.ToInt32(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument);
                var obj = db.tbl_proveedor.FirstOrDefault(p => p.prov_id == cod);
                if (obj != null)
                {
                    // En lugar de borrar de la base, marcamos como inactivo para no romper llaves foráneas.
                    obj.prov_estado = 'I';
                    db.SubmitChanges();
                    
                    // Mostrar Label (si la tenemos)
                    var lblMensaje = (System.Web.UI.WebControls.Label)UpdatePanel1.FindControl("lblMensaje");
                    if(lblMensaje != null) {
                        lblMensaje.Visible = true;
                        lblMensaje.Text = "¡Proveedor inactivado correctamente!";
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
                      lblMensaje.Text = "Error al inactivar: " + ex.Message;
                      lblMensaje.ForeColor = System.Drawing.Color.Red;
                 }
            }
        }
    }
}