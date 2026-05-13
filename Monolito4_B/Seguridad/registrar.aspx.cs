using System;
using System.Linq;
using capa_negocio; 
using Capa_Datos;
using SimpleCrypto;

namespace Monolito4_B.Seguridad
{
    public partial class registrar : System.Web.UI.Page
    {
        tbl_usuario usuinfo = new tbl_usuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPerfiles();
            }
        }

        private void CargarPerfiles()
        {
            try
            {
                var listaPerfiles = CN_tbl_tipo_usuario.ListarTipoUsuario();

                ddlPerfil.DataSource = listaPerfiles;
                ddlPerfil.DataTextField = "tusu_nombre";
                ddlPerfil.DataValueField = "tusu_id";

                ddlPerfil.DataBind();

                ddlPerfil.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione un Perfil --", ""));
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al cargar perfiles: " + ex.Message + "');</script>");
            }
        }

        protected void btnRegisterASP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlPerfil.SelectedValue))
                {
                    Response.Write("<script>alert('Por favor, seleccione un rol válido.');</script>");
                    return;
                }

                using (DataClasses1DataContext dc = new DataClasses1DataContext())
                {
                    tbl_usuario nuevoUsuario = new tbl_usuario();

                    nuevoUsuario.tusu_id = int.Parse(ddlPerfil.SelectedValue);
                    nuevoUsuario.usu_cedula = regCedula.Text.Trim();
                    nuevoUsuario.usu_nick = regNick.Text.Trim();
                    nuevoUsuario.usu_nombres = regNombres.Text.Trim();
                    nuevoUsuario.usu_apellidos = regApellidos.Text.Trim();
                    nuevoUsuario.usu_celular = regCelular.Text.Trim();

                    if (DateTime.TryParse(regFechaCumple.Text, out DateTime fechaBD))
                    {
                        nuevoUsuario.usu_fecha_cumple = fechaBD;
                    }

                    nuevoUsuario.usu_correo = regCorreo.Text.Trim();
                    nuevoUsuario.usu_direccion = regDireccion.Text.Trim();
                    byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(regPassword.Text);
                    nuevoUsuario.usu_contraseña = new System.Data.Linq.Binary(passwordBytes);
                    nuevoUsuario.usu_estado = 'A';
                    nuevoUsuario.usu_fecha_creacion = DateTime.Now;

                    
                    dc.tbl_usuario.InsertOnSubmit(nuevoUsuario);
                    dc.SubmitChanges();
                }

                
                Response.Redirect("login.aspx", false);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al intentar registrar: " + ex.Message + "');</script>");
            }
        }

        protected void regApellidos_TextChanged(object sender, EventArgs e)
        {
            string[] nom = regNombres.Text.Split(' ');
            string[] ape = regApellidos.Text.Split(' ');

            regCorreo.Text = nom[0].ToLower() + "." + ape[0].ToLower()+"@cordillera.edu.ec";

            regPassword.Text = RandomPassword.Generate(8, PasswordGroup.Numeric, PasswordGroup.Uppercase,
                PasswordGroup.Lowercase, PasswordGroup.Special);

            //generame el nick sacar las iniciales de todos los nombres y apellidos concatenados y convertirlos a minusculas y mayusculas y agregar un numero aleatorio anete 100y 1000 al final del nick un caracter especial y tomar dos digitos aleatorios de la cedula
            Random rnd = new Random();
            char[] ced = regCedula.Text.ToCharArray();
            regNick.Text = nom[0].Substring(0,1).ToUpper()+ nom[1].Substring(0, 1).ToLower() + ape[0].Substring (0, 1).ToUpper()+ ape[1].Substring (0,1).ToLower() + rnd.Next(100,1000).ToString() + RandomPassword.Generate(1, PasswordGroup.Special) + ced[rnd.Next(ced.Length)];


        }
    }
}