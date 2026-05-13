using Capa_Datos;
using capa_negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;
using System.Web.UI.WebControls;
using SimpleCrypto;
using System.Diagnostics;
namespace Monolito4_B.Seguridad
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataClasses1DataContext dc = new DataClasses1DataContext();
                dc.sp_reiniciarIntentosDiarios();

                pnlLogin.Visible = true;
                pnlOTP.Visible = false;
            }

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_ced.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Por favor, ingrese su cédula.');", true);
            }
            else if (string.IsNullOrEmpty(txt_pass.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Por favor, ingrese su contraseña.');", true);



            }
            else
            {
                bool existece = CN_tbl_usuario.autentixced(txt_ced.Text);
                {
                    tbl_usuario usinfo = new tbl_usuario();
                    usinfo = CN_tbl_usuario.traterced(txt_ced.Text);
                    int? intentos = usinfo.usu_intentos;
                    int? perf = usinfo.tusu_id;
                    if (existece)
                    {
                        bool existecc = CN_tbl_usuario.autentixpass(txt_ced.Text, txt_pass.Text);
                        {
                            if (existecc)
                            {

                                string CodOPT = RandomPassword.Generate(8, PasswordGroup.Uppercase, PasswordGroup.Numeric, PasswordGroup.Special);
                                usinfo.usu_codigo_OTP = CodOPT;
                                CN_tbl_usuario.modificarusu(usinfo);

                                if (perf == 1)
                                {
                                    Session["adm"] = usinfo.usu_nombres + " " + usinfo.usu_apellidos;
                                    Response.Redirect("~/Mantenimiento/Admin.aspx");

                                }

                                if (perf == 2)
                                {
                                    Session["usu" + ""] = usinfo.usu_nombres + " " + usinfo.usu_apellidos;
                                    Response.Redirect("");

                                }

                                usinfo.usu_intentos = 0;

                                Session["CedulaOTP"] = txt_ced.Text;
                                pnlLogin.Visible = false;
                                pnlOTP.Visible = true;

                            }
                            else
                            {
                                usinfo.usu_fecha_ultimo_intento = DateTime.Now;
                                usinfo.usu_intentos = intentos + 1;
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Credenciales incorrectas intento: {intentos}');", true);

                                CN_tbl_usuario.modificarintentos(txt_ced.Text, txt_pass.Text);
                            }


                        }






                    }

                }
            }
        }


        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            if (Session["CedulaOTP"] != null)
            {
                string cedula = Session["CedulaOTP"].ToString();
                tbl_usuario usinfo = CN_tbl_usuario.traterced(cedula);

                if (usinfo != null && usinfo.usu_codigo_OTP == txt_otp.Text.Trim())
                {
                    usinfo.usu_codigo_OTP = null;
                    usinfo.usu_intentos = 0;
                    CN_tbl_usuario.modificarusu(usinfo);

                    Session.Remove("CedulaOTP");
                    Session["usuario"] = usinfo.usu_id;

                    Response.Redirect("Inicio.aspx");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Código incorrecto.');", true);
                }
            }
        }

        protected void lnkResend_Click(object sender, EventArgs e)
        {
            if (Session["CedulaOTP"] != null)
            {
                string cedula = Session["CedulaOTP"].ToString();
                tbl_usuario usinfo = CN_tbl_usuario.traterced(cedula);

                string nuevoCodOPT = RandomPassword.Generate(8, PasswordGroup.Uppercase, PasswordGroup.Numeric, PasswordGroup.Special);
                usinfo.usu_codigo_OTP = nuevoCodOPT;
                CN_tbl_usuario.modificarusu(usinfo);

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Nuevo código generado y enviado.');", true);
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Session.Remove("CedulaOTP");
            pnlLogin.Visible = true;
            pnlOTP.Visible = false;
        }
    }
}