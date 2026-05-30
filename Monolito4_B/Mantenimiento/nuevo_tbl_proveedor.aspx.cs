using Capa_Datos;
using System;
using System.Linq;

namespace Monolito4_B.Mantenimiento
{
    public partial class nuevo_tbl_proveedor : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["cod"] != null)
                {
                    int cod = Convert.ToInt32(Request["cod"]);
                    var obj = db.tbl_proveedor.FirstOrDefault(p => p.prov_id == cod);
                    if (obj != null)
                    {
                        txtNombre.Text = obj.prov_nombre;
                        txtTelefono.Text = obj.prov_telefono;
                        txtDireccion.Text = obj.prov_direccion;
                        ddlEstado.SelectedValue = obj.prov_estado.ToString();
                    }
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    lblMensaje.Text = "El nombre es obligatorio.";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                bool isNew = Request["cod"] == null;
                tbl_proveedor obj = new tbl_proveedor();
                
                if (!isNew)
                {
                    int cod = Convert.ToInt32(Request["cod"]);
                    obj = db.tbl_proveedor.FirstOrDefault(p => p.prov_id == cod);
                }

                obj.prov_nombre = txtNombre.Text.Trim();
                obj.prov_telefono = txtTelefono.Text.Trim();
                obj.prov_direccion = txtDireccion.Text.Trim();
                obj.prov_estado = Convert.ToChar(ddlEstado.SelectedValue);

                if (isNew) db.tbl_proveedor.InsertOnSubmit(obj);
                db.SubmitChanges();
                
                // Si el estado es inactivo, anular (o actualizar) en los productos (Req)
                if (obj.prov_estado == 'I') {
                    db.ExecuteCommand("UPDATE tbl_producto SET prov_id = NULL WHERE prov_id = {0}", obj.prov_id);
                }

                lblMensaje.Text = "¡Proveedor guardado correctamente!";
                lblMensaje.ForeColor = System.Drawing.Color.Green;

                if (isNew)
                {
                    txtNombre.Text = ""; txtTelefono.Text = ""; txtDireccion.Text = ""; ddlEstado.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al guardar: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mantenimiento/listar_tbl_proveedor.aspx");
        }
    }
}
