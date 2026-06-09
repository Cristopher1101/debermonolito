using Capa_Datos;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;

namespace Monolito4_B.Mantenimiento
{
    public partial class listar_tbl_producto : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        string connStr;

        protected void Page_Load(object sender, EventArgs e)
        {
            connStr = db.Connection.ConnectionString;
            if (!IsPostBack)
            {
                CargarFiltroProveedores();
                CargarProductos();
            }
        }

        private void CargarFiltroProveedores()
        {
            var provs = db.tbl_proveedor.Where(p => p.prov_estado == 'A').OrderBy(p => p.prov_nombre).ToList();
            ddlProveedor.DataSource = provs;
            ddlProveedor.DataTextField = "prov_nombre";
            ddlProveedor.DataValueField = "prov_id";
            ddlProveedor.DataBind();
            ddlProveedor.Items.Insert(0, new ListItem("Todos los Proveedores", "0"));
        }

        private void CargarProductos(string busqueda = "")
        {
            // Usamos ADO.NET temporalmente para el listado para asegurarnos de que la tabla de imágenes
            // funcione aún si el DBML no está 100% actualizado.
            
            string query = @"
                SELECT p.pro_id, p.pro_nombre, p.pro_precio, p.pro_cantidad, p.pro_estado,
                       CASE 
                           WHEN pr.prov_id IS NULL THEN 'N/A'
                           WHEN pr.prov_estado = 'I' THEN 'N/A'
                           ELSE pr.prov_nombre 
                       END AS ProveedorNombre,
                       (SELECT TOP 1 pimg_path FROM tbl_producto_imagen img WHERE img.pro_id = p.pro_id ORDER BY pimg_es_principal DESC, pimg_id ASC) AS ImagenPrincipal
                FROM tbl_producto p
                LEFT JOIN tbl_proveedor pr ON p.prov_id = pr.prov_id
                WHERE 1=1
            ";

            if (!string.IsNullOrEmpty(busqueda))
            {
                query += " AND (p.pro_nombre LIKE @busqueda OR pr.prov_nombre LIKE @busqueda)";
            }
            
            if (ddlProveedor.SelectedValue != "0" && !string.IsNullOrEmpty(ddlProveedor.SelectedValue))
            {
                query += " AND p.prov_id = @provId";
            }

            // Ordenado estrictamente por ID para que se vea "bien bonito" (1, 2, 3...)
            query += " ORDER BY p.pro_id ASC";

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
                    }
                    if (ddlProveedor.SelectedValue != "0" && !string.IsNullOrEmpty(ddlProveedor.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@provId", ddlProveedor.SelectedValue);
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            gvProductos.DataSource = dt;
            gvProductos.DataBind();
        }

        protected void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            CargarProductos(txtBusqueda.Text.Trim());
        }

        protected void ddlProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarProductos(txtBusqueda.Text.Trim());
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarProductos(txtBusqueda.Text.Trim());
        }

        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductos.PageIndex = e.NewPageIndex;
            CargarProductos(txtBusqueda.Text.Trim());
        }

        protected void gvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int pro_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "pro_id"));
                Repeater rpt = (Repeater)e.Row.FindControl("rptImagenes");
                Label lblNoImage = (Label)e.Row.FindControl("lblNoImage");
                
                var imagenes = db.tbl_producto_imagen.Where(img => img.pro_id == pro_id).OrderByDescending(img => img.pimg_es_principal).ThenBy(img => img.pimg_id).ToList();
                
                if (imagenes.Count > 0)
                {
                    rpt.DataSource = imagenes;
                    rpt.DataBind();
                }
                else
                {
                    lblNoImage.Visible = true;
                }
            }
        }

        protected void btnLogico_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
                var prod = db.tbl_producto.FirstOrDefault(p => p.pro_id == id);
                if (prod != null)
                {
                    prod.pro_estado = (prod.pro_estado == 'A') ? 'I' : 'A';
                    db.SubmitChanges();
                    lblMensaje.Text = "Estado lógico del producto actualizado.";
                    lblMensaje.Visible = true;
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    CargarProductos(txtBusqueda.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
                lblMensaje.Visible = true;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnFisico_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
                var prod = db.tbl_producto.FirstOrDefault(p => p.pro_id == id);
                if (prod != null)
                {
                    // Eliminar las imágenes físicas y registros
                    var imagenes = db.tbl_producto_imagen.Where(img => img.pro_id == id).ToList();
                    foreach (var img in imagenes)
                    {
                        try
                        {
                            string ruta = Server.MapPath(img.pimg_path);
                            if (System.IO.File.Exists(ruta)) System.IO.File.Delete(ruta);
                        }
                        catch { /* Ignorar errores de archivo al borrar */ }
                    }
                    db.tbl_producto_imagen.DeleteAllOnSubmit(imagenes);
                    
                    // Eliminar producto
                    db.tbl_producto.DeleteOnSubmit(prod);
                    db.SubmitChanges();
                    
                    lblMensaje.Text = "Producto eliminado físicamente por completo.";
                    lblMensaje.Visible = true;
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    CargarProductos(txtBusqueda.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al eliminar físico: " + ex.Message;
                lblMensaje.Visible = true;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
