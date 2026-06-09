using Capa_Datos;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace Monolito4_B.Mantenimiento
{
    public partial class importar_productos : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Requerido por EPPlus 4 (licencia no comercial es por default en la 4.x pero para evitar warnings)
            // No aplica en 4.5.3, solo en 5+ (LicenseContext)
        }

        protected void btnPrevisualizar_Click(object sender, EventArgs e)
        {
            if (fileUploadExcel.HasFile)
            {
                try
                {
                    string ext = Path.GetExtension(fileUploadExcel.FileName).ToLower();
                    if (ext != ".xlsx" && ext != ".xls")
                    {
                        lblMensaje.Text = "Por favor sube un archivo con extensión .xlsx o .xls (Excel).";
                        lblMensaje.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    if (fileUploadExcel.PostedFile.ContentLength > 5 * 1024 * 1024)
                    {
                        lblMensaje.Text = "El archivo Excel supera el límite de 5 MB.";
                        lblMensaje.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    DataTable dt = ParseExcel(fileUploadExcel.FileContent);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        lblMensaje.Text = "El archivo Excel está vacío o no tiene el formato correcto.";
                        lblMensaje.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    Session["ExcelData"] = dt;
                    
                    gvPreview.DataSource = dt;
                    gvPreview.DataBind();
                    pnlPreview.Visible = true;
                    lblMensaje.Text = $"Se encontraron {dt.Rows.Count} registros listos para importar.";
                    lblMensaje.ForeColor = System.Drawing.Color.Blue;
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = "Error leyendo el archivo Excel: " + ex.Message;
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblMensaje.Text = "Debes seleccionar un archivo primero.";
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            DataTable dt = Session["ExcelData"] as DataTable;
            if (dt == null) return;

            int insertados = 0;
            int actualizados = 0;

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    int proId = 0;
                    double tempId = 0;
                    if (double.TryParse(row["ID"]?.ToString(), out tempId)) proId = (int)tempId;
                    
                    string nombre = row["Nombre"]?.ToString().Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(nombre)) continue; // Nombre es obligatorio

                    decimal precio = 0;
                    decimal.TryParse(row["Precio"]?.ToString(), out precio);
                    int cantidad = 0;
                    int.TryParse(row["Cantidad"]?.ToString(), out cantidad);
                    int provId = 0;
                    int.TryParse(row["Prov_ID"]?.ToString(), out provId);
                    string urlImg = row["Imagenes_URLs"]?.ToString();

                    // Buscar por ID primero
                    tbl_producto prod = null;
                    if (proId > 0)
                    {
                        prod = db.tbl_producto.FirstOrDefault(p => p.pro_id == proId);
                    }
                    
                    // Si no se encontró por ID (o no tiene ID), buscar por nombre de forma súper robusta
                    if (prod == null)
                    {
                        string nombreLower = nombre.ToLower();
                        // Hacemos ToList() para asegurar que la comparación ignore espacios en blanco de forma segura en memoria
                        prod = db.tbl_producto.ToList().FirstOrDefault(p => p.pro_nombre != null && p.pro_nombre.Trim().ToLower() == nombreLower);
                    }

                    int productoId = 0;

                    if (prod != null)
                    {
                        // Update
                        prod.pro_nombre = nombre; // Actualiza el nombre por si lo cambiaron en Excel
                        prod.pro_precio = precio;
                        prod.pro_cantidad = cantidad;
                        
                        var propInfo = prod.GetType().GetProperty("prov_id");
                        if (propInfo.PropertyType == typeof(int?))
                            propInfo.SetValue(prod, provId > 0 ? (int?)provId : null);
                        else
                            propInfo.SetValue(prod, provId);
                        
                        db.SubmitChanges();
                        productoId = prod.pro_id;
                        actualizados++;
                    }
                    else
                    {
                        // Insert
                        tbl_producto nuevo = new tbl_producto();
                        nuevo.pro_nombre = nombre;
                        nuevo.pro_precio = precio;
                        nuevo.pro_cantidad = cantidad;
                        nuevo.pro_estado = 'A';
                        
                        var propInfo = nuevo.GetType().GetProperty("prov_id");
                        if (propInfo.PropertyType == typeof(int?))
                            propInfo.SetValue(nuevo, provId > 0 ? (int?)provId : null);
                        else
                            propInfo.SetValue(nuevo, provId);

                        db.tbl_producto.InsertOnSubmit(nuevo);
                        db.SubmitChanges(); // Necesario para obtener el pro_id generado
                        productoId = nuevo.pro_id;
                        insertados++;
                    }

                    // Actualizar imágenes (Borrar anteriores y re-insertar)
                    if (productoId > 0)
                    {
                        db.ExecuteCommand("DELETE FROM tbl_producto_imagen WHERE pro_id = {0}", productoId);
                        
                        if (!string.IsNullOrWhiteSpace(urlImg))
                        {
                            string[] paths = urlImg.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            bool esPrimero = true;
                            foreach (var p in paths)
                            {
                                string cleanPath = p.Trim();
                                if (!string.IsNullOrEmpty(cleanPath))
                                {
                                    int esPrinc = esPrimero ? 1 : 0;
                                    db.ExecuteCommand("INSERT INTO tbl_producto_imagen (pro_id, pimg_path, pimg_es_principal) VALUES ({0}, {1}, {2})", productoId, cleanPath, esPrinc);
                                    esPrimero = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) 
                { 
                    System.Diagnostics.Debug.WriteLine("Error procesando fila: " + ex.Message);
                }
            }

            db.SubmitChanges();
            Session.Remove("ExcelData");
            pnlPreview.Visible = false;

            lblMensaje.Text = $"¡Proceso finalizado! Se insertaron {insertados} y se actualizaron {actualizados} productos.";
            lblMensaje.ForeColor = System.Drawing.Color.Green;
        }

        private DataTable ParseExcel(Stream fileStream)
        {
            DataTable dt = new DataTable();
            
            using (ExcelPackage package = new ExcelPackage(fileStream))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();
                if (ws == null) return null;

                // Definimos exactamente las columnas requeridas
                dt.Columns.Add("ID");
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Precio");
                dt.Columns.Add("Cantidad");
                dt.Columns.Add("Prov_ID");
                dt.Columns.Add("Imagenes_URLs");

                int rowCount = ws.Dimension?.End.Row ?? 0;
                
                for (int row = 2; row <= rowCount; row++)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = ws.Cells[row, 1].Value?.ToString();
                    dr["Nombre"] = ws.Cells[row, 2].Value?.ToString();
                    dr["Precio"] = ws.Cells[row, 3].Value?.ToString();
                    dr["Cantidad"] = ws.Cells[row, 4].Value?.ToString();
                    dr["Prov_ID"] = ws.Cells[row, 5].Value?.ToString();
                    dr["Imagenes_URLs"] = ws.Cells[row, 6].Value?.ToString();
                    
                    if (!string.IsNullOrWhiteSpace(dr["Nombre"]?.ToString()))
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            
            return dt;
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                // Ordenar por ID ascendente para mostrar los más antiguos primero
                var productos = db.tbl_producto.OrderBy(p => p.pro_id).ToList();

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Productos");

                    // Crear cabeceras
                    ws.Cells["A1"].Value = "ID";
                    ws.Cells["B1"].Value = "Nombre";
                    ws.Cells["C1"].Value = "Precio";
                    ws.Cells["D1"].Value = "Cantidad";
                    ws.Cells["E1"].Value = "Prov_ID";
                    ws.Cells["F1"].Value = "Imagenes_URLs";

                    // Estilizar las cabeceras
                    using (ExcelRange rng = ws.Cells["A1:F1"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Font.Size = 12;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(57, 73, 171));
                        rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rng.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    }

                    int row = 2;
                    foreach (var p in productos)
                    {
                        ws.Cells[$"A{row}"].Value = p.pro_id;
                        ws.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        
                        ws.Cells[$"B{row}"].Value = p.pro_nombre;
                        
                        ws.Cells[$"C{row}"].Value = p.pro_precio;
                        ws.Cells[$"C{row}"].Style.Numberformat.Format = "$#,##0.00"; 
                        
                        ws.Cells[$"D{row}"].Value = p.pro_cantidad;
                        
                        var propInfo = p.GetType().GetProperty("prov_id");
                        object prov = propInfo.GetValue(p);
                        ws.Cells[$"E{row}"].Value = prov != null ? Convert.ToInt32(prov) : 0;
                        
                        // Extraer todas las imágenes del producto
                        string urlsCombinadas = "";
                        try {
                            var imgList = db.ExecuteQuery<string>("SELECT pimg_path FROM tbl_producto_imagen WHERE pro_id = {0} ORDER BY pimg_es_principal DESC, pimg_id ASC", p.pro_id).ToList();
                            if(imgList.Count > 0) {
                                urlsCombinadas = string.Join(" | ", imgList);
                            }
                        } catch { }
                        
                        ws.Cells[$"F{row}"].Value = urlsCombinadas;
                        
                        // Bordes sutiles
                        using (ExcelRange rowRng = ws.Cells[$"A{row}:F{row}"])
                        {
                            rowRng.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                            rowRng.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.LightGray);
                        }

                        row++;
                    }

                    ws.Cells.AutoFitColumns();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ProductosExportados.xlsx");
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al exportar a Excel: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
