using Capa_Datos;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Monolito4_B.Mantenimiento
{
    public partial class nuevo_tbl_producto : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                // Cargar proveedores activos
                var proveedores = db.tbl_proveedor.Where(p => p.prov_estado == 'A').ToList();
                ddlProveedor.DataSource = proveedores;
                ddlProveedor.DataTextField = "prov_nombre";
                ddlProveedor.DataValueField = "prov_id";
                ddlProveedor.DataBind();
                ddlProveedor.Items.Insert(0, new ListItem("-- Seleccione un Proveedor --", ""));

                if (Request["cod"] != null)
                {
                    int cod = Convert.ToInt32(Request["cod"]);
                    var obj = db.tbl_producto.FirstOrDefault(p => p.pro_id == cod);
                    if (obj != null)
                    {
                        txtNombre.Text = obj.pro_nombre;
                        txtCantidad.Text = obj.pro_cantidad.ToString();
                        txtPrecio.Text = obj.pro_precio.ToString();
                        if (obj.prov_id != null)
                            ddlProveedor.SelectedValue = obj.prov_id.ToString();
                        ddlEstado.SelectedValue = obj.pro_estado.ToString();
                        
                        // Cargar imágenes existentes para previsualización sin JS usando un string de HTML
                    }
                }
            }
        }

        public string ObtenerImagenesHtml()
        {
            string html = "";
            if (Request["cod"] != null)
            {
                int cod = Convert.ToInt32(Request["cod"]);
                var imagenes = db.ExecuteQuery<ImagenResult>("SELECT pimg_path FROM tbl_producto_imagen WHERE pro_id = {0}", cod).ToList();
                foreach (var img in imagenes)
                {
                    html += $"<img src='{ResolveUrl(img.pimg_path)}' alt='Imagen Producto' />";
                }
            }
            return html;
        }

        public class ImagenResult
        {
            public string pimg_path { get; set; }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text) || ddlProveedor.SelectedValue == "")
                {
                    lblMensaje.Text = "Nombre, Precio y Proveedor son obligatorios.";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                bool isNew = Request["cod"] == null;
                // Exigir 3 imágenes solo al crear nuevo, o si sube imágenes nuevas
                if (isNew && (!fileImagenes.HasFiles || fileImagenes.PostedFiles.Count < 3))
                {
                    lblMensaje.Text = "Debe adjuntar al menos 3 imágenes para el nuevo producto.";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                tbl_producto obj = new tbl_producto();
                if (!isNew) {
                    int cod = Convert.ToInt32(Request["cod"]);
                    obj = db.tbl_producto.FirstOrDefault(p => p.pro_id == cod);
                }

                obj.pro_nombre = txtNombre.Text.Trim();
                obj.pro_cantidad = string.IsNullOrEmpty(txtCantidad.Text) ? 0 : Convert.ToInt32(txtCantidad.Text);
                obj.pro_precio = Convert.ToDecimal(txtPrecio.Text);
                

                int selectedProv = Convert.ToInt32(ddlProveedor.SelectedValue);

                var propInfo = obj.GetType().GetProperty("prov_id");
                if (propInfo.PropertyType == typeof(int?))
                    propInfo.SetValue(obj, (int?)selectedProv);
                else
                    propInfo.SetValue(obj, selectedProv);

                obj.pro_estado = Convert.ToChar(ddlEstado.SelectedValue);

                if (isNew) db.tbl_producto.InsertOnSubmit(obj);
                db.SubmitChanges();

                // Procesar imágenes
                if (fileImagenes.HasFiles)
                {
                    string folderPath = Server.MapPath("~/Uploads/Productos/");
                    if (!System.IO.Directory.Exists(folderPath))
                        System.IO.Directory.CreateDirectory(folderPath);

                    bool isFirst = isNew; // Si es nuevo, la primera imagen es la principal
                    foreach (var file in fileImagenes.PostedFiles)
                    {
                        string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                        if ((ext == ".jpg" || ext == ".png" || ext == ".jpeg") && file.ContentLength <= 5 * 1024 * 1024)
                        {
                            string fileName = Guid.NewGuid().ToString() + ext;
                            string savePath = System.IO.Path.Combine(folderPath, fileName);
                            file.SaveAs(savePath);

                            string relativePath = "~/Uploads/Productos/" + fileName;
                            int principal = isFirst ? 1 : 0;
                            
                            // Usamos ExecuteCommand para evitar que el compilador requiera que tbl_producto_imagen exista en DataClasses1.dbml
                            db.ExecuteCommand("INSERT INTO tbl_producto_imagen (pro_id, pimg_path, pimg_es_principal) VALUES ({0}, {1}, {2})", obj.pro_id, relativePath, principal);
                            isFirst = false;
                        }
                    }
                }

                lblMensaje.Text = "¡Producto guardado correctamente!";
                lblMensaje.ForeColor = System.Drawing.Color.Green;

                if (isNew)
                {
                    txtNombre.Text = "";
                    txtCantidad.Text = "";
                    txtPrecio.Text = "";
                    ddlProveedor.SelectedIndex = 0;
                    ddlEstado.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al guardar: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }


        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int cod = Convert.ToInt32((sender as Button).CommandArgument);
            Response.Redirect($"~/Mantenimiento/nuevo_tbl_producto.aspx?cod={cod}");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int cod = Convert.ToInt32((sender as Button).CommandArgument);
                var obj = db.tbl_producto.FirstOrDefault(p => p.pro_id == cod);
                if (obj != null)
                {
                    db.tbl_producto.DeleteOnSubmit(obj);
                    db.SubmitChanges();
                    lblMensaje.Text = "¡Producto eliminado correctamente!";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblMensaje.Text = "Producto no encontrado.";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al eliminar: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            // Ruta corregida apuntando a la lista de productos
            Response.Redirect("~/Mantenimiento/listar_tbl_producto.aspx");
        }
    }
}