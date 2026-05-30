using Capa_Datos;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections.Generic;

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
                CargarProductos();
            }
        }

        private void CargarProductos(string busqueda = "")
        {
            // Usamos ADO.NET temporalmente para el listado para asegurarnos de que la tabla de imágenes
            // funcione aún si el DBML no está 100% actualizado.
            
            string query = @"
                SELECT p.pro_id, p.pro_nombre, p.pro_precio, p.pro_cantidad,
                       pr.prov_nombre AS ProveedorNombre,
                       (SELECT TOP 1 pimg_path FROM tbl_producto_imagen img WHERE img.pro_id = p.pro_id ORDER BY pimg_es_principal DESC, pimg_id ASC) AS ImagenPrincipal
                FROM tbl_producto p
                LEFT JOIN tbl_proveedor pr ON p.prov_id = pr.prov_id
                WHERE p.pro_estado = 'A'
            ";

            if (!string.IsNullOrEmpty(busqueda))
            {
                query += " AND (p.pro_nombre LIKE @busqueda OR pr.prov_nombre LIKE @busqueda)";
            }

            query += " ORDER BY p.pro_id DESC";

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarProductos(txtBusqueda.Text.Trim());
        }

        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductos.PageIndex = e.NewPageIndex;
            CargarProductos(txtBusqueda.Text.Trim());
        }

        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
                var prod = db.tbl_producto.FirstOrDefault(p => p.pro_id == id);
                if (prod != null)
                {
                    prod.pro_estado = 'I'; // Borrado lógico
                    db.SubmitChanges();
                    lblMensaje.Text = "Producto eliminado.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    CargarProductos(txtBusqueda.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
