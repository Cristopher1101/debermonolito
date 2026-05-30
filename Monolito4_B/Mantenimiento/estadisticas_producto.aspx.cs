using Capa_Datos;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;

namespace Monolito4_B.Mantenimiento
{
    public partial class estadisticas_producto : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        string connStr;

        protected void Page_Load(object sender, EventArgs e)
        {
            connStr = db.Connection.ConnectionString;
            
            if (!IsPostBack)
            {
                int proId = 0;
                if (Request["cod"] != null && int.TryParse(Request["cod"], out proId))
                {
                    CargarCarrusel(proId);
                }
                
                CargarGrafico();
            }
        }

        private void CargarCarrusel(int proId)
        {
            // Cargar imágenes con ADO.NET para no depender del DBML
            string query = "SELECT pimg_path FROM tbl_producto_imagen WHERE pro_id = @id ORDER BY pimg_es_principal DESC, pimg_id ASC";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", proId);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            if (dt.Rows.Count > 0)
            {
                rptImagenes.DataSource = dt;
                rptImagenes.DataBind();
            }
            else
            {
                lblNoImages.Visible = true;
            }
        }

        private void CargarGrafico()
        {
            // Obtener los 10 productos con más stock para la gráfica
            string query = "SELECT TOP 10 pro_nombre, pro_cantidad, pro_precio FROM tbl_producto WHERE pro_estado = 'A' ORDER BY pro_cantidad DESC";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            string labels = "";
            string dataStock = "";
            string dataPrecio = "";

            foreach (DataRow row in dt.Rows)
            {
                labels += $"'{row["pro_nombre"].ToString().Replace("'", "")}',";
                dataStock += $"{row["pro_cantidad"]},";
                dataPrecio += $"{row["pro_precio"].ToString().Replace(",", ".")},";
            }
            
            if (labels.Length > 0)
            {
                labels = labels.TrimEnd(',');
                dataStock = dataStock.TrimEnd(',');
                dataPrecio = dataPrecio.TrimEnd(',');

                string script = $@"
                    <script>
                        var ctx = document.getElementById('myChart').getContext('2d');
                        var myChart = new Chart(ctx, {{
                            type: 'bar',
                            data: {{
                                labels: [{labels}],
                                datasets: [{{
                                    label: 'Stock Disponible',
                                    data: [{dataStock}],
                                    backgroundColor: 'rgba(59, 130, 246, 0.5)',
                                    borderColor: 'rgba(59, 130, 246, 1)',
                                    borderWidth: 1
                                }},
                                {{
                                    type: 'line',
                                    label: 'Precio (USD)',
                                    data: [{dataPrecio}],
                                    backgroundColor: 'rgba(16, 185, 129, 0.5)',
                                    borderColor: 'rgba(16, 185, 129, 1)',
                                    borderWidth: 2,
                                    fill: false
                                }}]
                            }},
                            options: {{
                                responsive: true,
                                scales: {{
                                    y: {{ beginAtZero: true }}
                                }}
                            }}
                        }});
                    </script>
                ";

                litChartScript.Text = script;
            }
        }
    }
}
