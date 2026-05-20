using System;
using System.Web.UI;
using capa_negocio;

namespace Monolito4_B.Seguridad
{
    public partial class RegistroExito : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string url = Session["RegistroQr_Url"] as string;
            if (string.IsNullOrWhiteSpace(url))
            {
                Response.Redirect("~/Seguridad/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            try
            {
                byte[] png = QrPngService.GenerarPng(url, 6);
                imgQr.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(png);
                litNota.Text = "Guarde una captura o escanee con la cámara desde el inicio de sesión. El enlace caduca en 15 minutos. También puede usar cédula, contraseña y el código por correo.";
            }
            catch
            {
                imgQr.Visible = false;
                litNota.Text = "No se pudo generar el QR. Inicie sesión con cédula y contraseña.";
            }
        }

        protected void btnIrLogin_Click(object sender, EventArgs e)
        {
            Session.Remove("RegistroQr_Url");
            Response.Redirect("~/Seguridad/Login.aspx", false);
        }
    }
}
