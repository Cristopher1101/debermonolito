using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using capa_negocio;
using Capa_Datos;

namespace Monolito4_B.Seguridad
{
    public partial class registrar : Page
    {
        /// <summary>Único tipo permitido en registro web (no administrador; admin = 1 en sesión).</summary>
        private const int IdTipoUsuarioRegistroPublico = 2;

        private const int TamMaxFotoBytes = FotoSubidaReglas.TamanoMaximoBytes;
        private const int MaxArchivosGaleria = 12;
        private static readonly string[] ExtensionesFoto = FotoSubidaReglas.ExtensionesPermitidas;
        private const string SessionGaleria = "RegGaleriaFotos";

        private static readonly Regex RxNickRegistro = new Regex(@"^[\p{L}0-9._-]{4,50}$", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex RxNombreSoloLetras = new Regex(@"^[\p{L}'\-]+(?:\s+[\p{L}'\-]+)+$", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex RxDireccionPermitida = new Regex(@"^[\p{L}\p{N}#.,\s°\-ªº/]+$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        [Serializable]
        private class FotoGaleriaSesion
        {
            public byte[] Datos { get; set; }
            public string Nombre { get; set; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Form != null)
                Form.Enctype = "multipart/form-data";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarPerfiles();

            RestaurarVistaFotoDesdeSesion();
            RestaurarResumenGaleria();
        }

        private void RestaurarVistaFotoDesdeSesion()
        {
            if (Session["FotoTemporal"] is byte[] b && b.Length > 0)
            {
                string mime = Session["FotoTemporalMime"] as string ?? "image/jpeg";
                imgPreview.ImageUrl = "data:" + mime + ";base64," + Convert.ToBase64String(b);
            }
        }

        private void RestaurarResumenGaleria()
        {
            var lista = Session[SessionGaleria] as List<FotoGaleriaSesion>;
            if (lista == null || lista.Count == 0)
            {
                lblGaleriaResumen.Visible = false;
            }
            else
            {
                lblGaleriaResumen.Visible = true;
                lblGaleriaResumen.Text = lista.Count + " imagen(es) en galería lista para guardar (máx. " + MaxArchivosGaleria + ").";
            }
            BindGaleriaPrevia();
        }

        private void BindGaleriaPrevia()
        {
            if (pnlGaleriaPrevia == null || rptGaleriaPrev == null)
                return;

            var lista = Session[SessionGaleria] as List<FotoGaleriaSesion>;
            if (lista == null || lista.Count == 0)
            {
                pnlGaleriaPrevia.Visible = false;
                rptGaleriaPrev.DataSource = null;
                rptGaleriaPrev.DataBind();
                return;
            }

            var rows = new List<GaleriaVistaLinea>();
            for (int i = 0; i < lista.Count; i++)
            {
                FotoGaleriaSesion item = lista[i];
                if (item?.Datos == null || item.Datos.Length == 0)
                    continue;
                if (!FotoSubidaReglas.EsJpegOPngPorFirma(item.Datos, out string mime))
                    continue;
                string nombre = item.Nombre ?? ("Imagen " + (i + 1));
                rows.Add(new GaleriaVistaLinea
                {
                    DataUri = "data:" + mime + ";base64," + Convert.ToBase64String(item.Datos),
                    Pie = HttpUtility.HtmlAttributeEncode(nombre),
                    CmdArg = i.ToString()
                });
            }

            pnlGaleriaPrevia.Visible = rows.Count > 0;
            rptGaleriaPrev.DataSource = rows;
            rptGaleriaPrev.DataBind();
        }

        protected void rptGaleriaPrev_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "QuitarGalPrev")
                return;
            if (!int.TryParse(e.CommandArgument?.ToString(), out int ix))
                return;
            if (Session[SessionGaleria] is List<FotoGaleriaSesion> lst && ix >= 0 && ix < lst.Count)
            {
                lst.RemoveAt(ix);
                if (lst.Count == 0)
                    Session.Remove(SessionGaleria);
            }
            RestaurarResumenGaleria();
        }

        private static string MimePorExtension(string ext)
        {
            switch (ext?.ToLowerInvariant())
            {
                case ".png": return "image/png";
                default: return "image/jpeg";
            }
        }

        private static byte[] LeerArchivoCompleto(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private bool EsExtensionFotoValida(string nombreArchivo)
        {
            string ext = Path.GetExtension(nombreArchivo)?.ToLowerInvariant() ?? "";
            return ExtensionesFoto.Contains(ext);
        }

        private List<HttpPostedFile> EnumerarArchivosGaleria()
        {
            return FileUploadMultiplesHelper.EnumerarArchivos(fuGaleria, Request);
        }

        private bool ValidarYGuardarGaleriaEnSesion(out string error)
        {
            error = null;
            var lista = new List<FotoGaleriaSesion>();
            int n = 0;
            foreach (HttpPostedFile f in EnumerarArchivosGaleria())
            {
                n++;
                if (n > MaxArchivosGaleria)
                {
                    error = "Máximo " + MaxArchivosGaleria + " imágenes en galería.";
                    return false;
                }
                if (f.ContentLength > TamMaxFotoBytes)
                {
                    error = "«" + Path.GetFileName(f.FileName) + "» supera 2 MB (vista previa servidor).";
                    return false;
                }
                if (!EsExtensionFotoValida(f.FileName))
                {
                    error = "«" + Path.GetFileName(f.FileName) + "»: solo JPG/PNG en servidor.";
                    return false;
                }
                byte[] bytes = LeerArchivoCompleto(f.InputStream);
                if (!FotoSubidaReglas.ValidarArchivoFoto(f.FileName, f.ContentLength, bytes, out string mime, out string errFirma))
                {
                    error = "«" + Path.GetFileName(f.FileName) + "»: " + errFirma + " (vista previa servidor).";
                    return false;
                }
                lista.Add(new FotoGaleriaSesion { Datos = bytes, Nombre = Path.GetFileName(f.FileName) });
            }
            Session[SessionGaleria] = lista;
            return true;
        }

        protected void btnPrevisualizar_Click(object sender, EventArgs e)
        {
            lblMensaje.Visible = true;

            if (!fuFotoPerfil.HasFile)
            {
                lblMensaje.ForeColor = System.Drawing.Color.FromArgb(255, 183, 77);
                lblMensaje.Text = "Elija la foto de perfil y pulse «Previsualizar fotos» para verla en el servidor.";
                return;
            }

            if (!ProcesarArchivoPrincipal(fuFotoPerfil, true, out string err))
            {
                lblMensaje.ForeColor = System.Drawing.Color.FromArgb(255, 112, 67);
                lblMensaje.Text = err;
                return;
            }

            if (EnumerarArchivosGaleria().Any())
            {
                if (!ValidarYGuardarGaleriaEnSesion(out string errGal))
                {
                    lblMensaje.ForeColor = System.Drawing.Color.FromArgb(255, 183, 77);
                    lblMensaje.Text = "Vista previa de perfil lista. Galería: " + errGal + " Pulse «Previsualizar» de nuevo tras corregir los archivos.";
                    RestaurarResumenGaleria();
                    return;
                }
            }
            else
            {
                Session.Remove(SessionGaleria);
            }

            lblMensaje.ForeColor = System.Drawing.Color.FromArgb(129, 199, 132);
            lblMensaje.Text = "Vista previa de perfil actualizada." + (Session[SessionGaleria] is List<FotoGaleriaSesion> g && g.Count > 0 ? " Galería: " + g.Count + " imagen(es)." : "");
            RestaurarResumenGaleria();
        }

        private bool ProcesarArchivoPrincipal(FileUpload upload, bool actualizarMiniatura, out string error)
        {
            error = null;
            if (!upload.HasFile)
            {
                error = "No hay archivo de foto principal.";
                return false;
            }

            int len = upload.PostedFile.ContentLength;
            if (len <= 0 || len > TamMaxFotoBytes)
            {
                error = "Vista previa: la imagen debe tener como máximo 2 MB.";
                return false;
            }

            if (!EsExtensionFotoValida(upload.FileName))
            {
                error = "Vista previa: el servidor solo muestra JPG o PNG. Compruebe el archivo.";
                return false;
            }

            try
            {
                byte[] pic = LeerArchivoCompleto(upload.PostedFile.InputStream);
                if (!FotoSubidaReglas.ValidarArchivoFoto(upload.FileName, len, pic, out string mimeFirma, out string errFirma))
                {
                    error = "Vista previa: no se pudo leer la imagen como JPG/PNG válido (" + errFirma + ").";
                    return false;
                }

                Session["FotoTemporal"] = pic;
                Session["FotoTemporalMime"] = mimeFirma;

                if (actualizarMiniatura)
                {
                    string mime = (string)Session["FotoTemporalMime"];
                    imgPreview.ImageUrl = "data:" + mime + ";base64," + Convert.ToBase64String(pic);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = "Error al leer la imagen: " + ex.Message;
                return false;
            }
        }

        private void AsegurarFotosDesdeUploadParaGuardar()
        {
            if (Session["FotoTemporal"] == null && fuFotoPerfil.HasFile)
                ProcesarArchivoPrincipal(fuFotoPerfil, true, out _);

            if (Session[SessionGaleria] == null && EnumerarArchivosGaleria().Any())
                ValidarYGuardarGaleriaEnSesion(out _);
        }

        private void CargarPerfiles()
        {
            try
            {
                var listaPerfiles = CN_tbl_tipo_usuario.ListarTipoUsuario()
                    .Where(t => t.tusu_id == IdTipoUsuarioRegistroPublico)
                    .ToList();

                ddlPerfil.Items.Clear();
                if (listaPerfiles.Count == 0)
                {
                    lblMensaje.Visible = true;
                    lblMensaje.ForeColor = System.Drawing.Color.FromArgb(255, 112, 67);
                    lblMensaje.Text = "No está configurado el tipo de usuario estándar (ID " + IdTipoUsuarioRegistroPublico + ") en la base de datos. Contacte al administrador.";
                    return;
                }

                ddlPerfil.DataSource = listaPerfiles;
                ddlPerfil.DataTextField = "tusu_nombre";
                ddlPerfil.DataValueField = "tusu_id";
                ddlPerfil.DataBind();
                ddlPerfil.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMensaje.Visible = true;
                lblMensaje.ForeColor = System.Drawing.Color.FromArgb(255, 112, 67);
                lblMensaje.Text = "Error al cargar perfiles: " + ex.Message;
            }
        }

        private bool ExisteCedula(string cedula)
        {
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario.Any(u => u.usu_cedula == cedula);
            }
        }

        private static int ContarPalabras(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return 0;
            return texto.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        private bool ValidarFormulario(out string mensaje)
        {
            var sb = new System.Text.StringBuilder();

            if (string.IsNullOrWhiteSpace(ddlPerfil.SelectedValue))
                sb.AppendLine("• No se pudo determinar el tipo de usuario. Recargue la página.");
            else if (!int.TryParse(ddlPerfil.SelectedValue, out int tidSel) || tidSel != IdTipoUsuarioRegistroPublico)
                sb.AppendLine("• El registro público solo permite el perfil usuario (ID " + IdTipoUsuarioRegistroPublico + ").");

            string ced = regCedula.Text.Trim();
            if (string.IsNullOrWhiteSpace(ced) || ced.Length != 10 || !Regex.IsMatch(ced, @"^\d{10}$"))
                sb.AppendLine("• La cédula debe tener exactamente 10 dígitos (solo números).");

            if (string.IsNullOrWhiteSpace(regNick.Text.Trim()) || regNick.Text.Trim().Length < 4)
                sb.AppendLine("• El nick debe tener al menos 4 caracteres.");
            else if (!RxNickRegistro.IsMatch(regNick.Text.Trim()))
                sb.AppendLine("• El nick solo puede usar letras, números y los símbolos . _ - (mínimo 4 caracteres).");
            else if (CN_tbl_usuario.ExisteNick(regNick.Text.Trim()))
                sb.AppendLine("• El nick ya está registrado; elija otro.");

            string nombresTrim = regNombres.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombresTrim))
                sb.AppendLine("• Ingrese los nombres.");
            else if (ContarPalabras(nombresTrim) < 2)
                sb.AppendLine("• Debe ingresar al menos dos nombres separados por un espacio (ej.: Juan Carlos).");
            else if (!RxNombreSoloLetras.IsMatch(nombresTrim))
                sb.AppendLine("• Los nombres solo pueden contener letras, espacios, apóstrofe o guion (sin números).");

            string apellidosTrim = regApellidos.Text.Trim();
            if (string.IsNullOrWhiteSpace(apellidosTrim))
                sb.AppendLine("• Ingrese los apellidos.");
            else if (ContarPalabras(apellidosTrim) < 2)
                sb.AppendLine("• Debe ingresar al menos dos apellidos separados por un espacio (ej.: Pérez López).");
            else if (!RxNombreSoloLetras.IsMatch(apellidosTrim))
                sb.AppendLine("• Los apellidos solo pueden contener letras, espacios, apóstrofe o guion (sin números).");

            string cel = regCelular.Text.Trim();
            if (string.IsNullOrWhiteSpace(cel) || !Regex.IsMatch(cel, @"^\d{9,10}$"))
                sb.AppendLine("• El celular debe tener 9 o 10 dígitos (solo números).");

            if (string.IsNullOrWhiteSpace(regFechaCumple.Text) || !DateTime.TryParse(regFechaCumple.Text, out DateTime fnac))
                sb.AppendLine("• Seleccione una fecha de nacimiento válida.");
            else if (fnac > DateTime.Today)
                sb.AppendLine("• La fecha de nacimiento no puede ser futura.");

            string mail = regCorreo.Text.Trim();
            if (string.IsNullOrWhiteSpace(mail) || !Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                sb.AppendLine("• Ingrese un correo electrónico válido.");

            string dir = regDireccion.Text.Trim();
            if (string.IsNullOrWhiteSpace(dir) || dir.Length < 5)
                sb.AppendLine("• La dirección debe tener al menos 5 caracteres.");
            else if (dir.Length > 50)
                sb.AppendLine("• La dirección no puede superar 50 caracteres (límite de base de datos).");
            else if (!RxDireccionPermitida.IsMatch(dir))
                sb.AppendLine("• La dirección solo puede incluir letras, números, espacios y los caracteres # . , - / °");

            if (string.IsNullOrWhiteSpace(regPassword.Text) || regPassword.Text.Length < 6)
                sb.AppendLine("• La contraseña debe tener al menos 6 caracteres.");

            AsegurarFotosDesdeUploadParaGuardar();
            if (Session["FotoTemporal"] == null)
                sb.AppendLine("• Debe adjuntar una foto de perfil (una sola imagen).");

            var galeria = Session[SessionGaleria] as List<FotoGaleriaSesion>;
            if (galeria != null && galeria.Count > MaxArchivosGaleria)
                sb.AppendLine("• La galería no puede superar " + MaxArchivosGaleria + " imágenes.");

            mensaje = sb.ToString().Trim();
            return string.IsNullOrEmpty(mensaje);
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            lblMensaje.Visible = true;

            if (!ValidarFormulario(out string errores))
            {
                lblMensaje.ForeColor = System.Drawing.Color.FromArgb(255, 183, 77);
                lblMensaje.Text = errores.Replace(Environment.NewLine, " · ");
                return;
            }

            string ced = regCedula.Text.Trim();
            if (ExisteCedula(ced))
            {
                lblMensaje.ForeColor = System.Drawing.Color.FromArgb(255, 112, 67);
                lblMensaje.Text = "Ya existe un usuario registrado con esa cédula.";
                return;
            }

            try
            {
                int idRegistrado = 0;
                using (DataClasses1DataContext dc = new DataClasses1DataContext())
                {
                    var nuevoUsuario = new tbl_usuario
                    {
                        tusu_id = IdTipoUsuarioRegistroPublico,
                        usu_cedula = ced,
                        usu_nick = regNick.Text.Trim(),
                        usu_nombres = regNombres.Text.Trim(),
                        usu_apellidos = regApellidos.Text.Trim(),
                        usu_celular = regCelular.Text.Trim(),
                        usu_correo = regCorreo.Text.Trim(),
                        usu_direccion = regDireccion.Text.Trim(),
                        usu_estado = 'A',
                        usu_fecha_creacion = DateTime.Now,
                        usu_intentos = 0,
                        usu_contraseña = null
                    };

                    if (DateTime.TryParse(regFechaCumple.Text, out DateTime fechaBD))
                        nuevoUsuario.usu_fecha_cumple = fechaBD;

                    byte[] fotoBytes = (byte[])Session["FotoTemporal"];
                    nuevoUsuario.usu_foto_perfil = new System.Data.Linq.Binary(fotoBytes);

                    dc.tbl_usuario.InsertOnSubmit(nuevoUsuario);
                    dc.SubmitChanges();

                    dc.ExecuteCommand("UPDATE tbl_usuario SET usu_contraseña = dbo.encriptacion({0}) WHERE usu_id = {1}",
                        regPassword.Text, nuevoUsuario.usu_id);

                    var principal = new tbl_usuario_foto
                    {
                        usu_id = nuevoUsuario.usu_id,
                        ufoto_datos = nuevoUsuario.usu_foto_perfil,
                        ufoto_es_principal = true
                    };
                    dc.tbl_usuario_foto.InsertOnSubmit(principal);

                    if (Session[SessionGaleria] is List<FotoGaleriaSesion> extras)
                    {
                        foreach (FotoGaleriaSesion item in extras)
                        {
                            if (item?.Datos == null || item.Datos.Length == 0) continue;
                            dc.tbl_usuario_foto.InsertOnSubmit(new tbl_usuario_foto
                            {
                                usu_id = nuevoUsuario.usu_id,
                                ufoto_datos = new System.Data.Linq.Binary(item.Datos),
                                ufoto_es_principal = false
                            });
                        }
                    }

                    dc.SubmitChanges();
                    idRegistrado = nuevoUsuario.usu_id;
                }

                Session.Remove("FotoTemporal");
                Session.Remove("FotoTemporalMime");
                Session.Remove(SessionGaleria);

                Response.Redirect("~/Seguridad/Login.aspx?reg=1", false);
            }
            catch (Exception ex)
            {
                lblMensaje.ForeColor = System.Drawing.Color.FromArgb(255, 112, 67);
                lblMensaje.Text = "Error al registrar: " + ex.Message;
            }
        }
    }
}
