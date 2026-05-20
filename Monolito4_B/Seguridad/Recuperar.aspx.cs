using System;
using System.Web;
using capa_negocio;
using Capa_Datos;
using SimpleCrypto;

namespace Monolito4_B.Seguridad
{
    public partial class Recuperar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            string cedula = txtCedulaRecuperar.Text.Trim();

            if (string.IsNullOrEmpty(cedula))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Ingresa una cédula válida.');", true);
                return;
            }

            tbl_usuario usinfo = CN_tbl_usuario.traterced(cedula);

            if (usinfo == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Usuario no encontrado.');", true);
                return;
            }

            string claveTemporal = RandomPassword.Generate(8, PasswordGroup.Uppercase, PasswordGroup.Lowercase, PasswordGroup.Numeric, PasswordGroup.Special);

            bool actualizado = CN_tbl_usuario.ActualizarPasswordTemporal(cedula, claveTemporal);

            if (!actualizado)
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Error al actualizar la contraseña en la base de datos.');", true);
                return;
            }

            string mensaje = $"Monolito4B — Hola {usinfo.usu_nombres}, su clave temporal es: {claveTemporal}. Cámbiela al ingresar.";

            var mail = new Mail();
            if (!string.IsNullOrWhiteSpace(usinfo.usu_correo) && mail.enviar_correo(usinfo.usu_correo, mensaje))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Se envió la clave temporal a su correo electrónico.'); window.location='Login.aspx';", true);
                return;
            }

            ClientScript.RegisterStartupScript(GetType(), "alert",
                "alert('No hay correo registrado o no se pudo enviar el mensaje (revise Mail.cs / Web.config). La clave sí quedó actualizada en base de datos.'); window.location='Login.aspx';", true);
        }
    }
}
