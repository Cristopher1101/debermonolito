using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using capa_negocio;
using Capa_Datos;

namespace Monolito4_B.Mantenimiento
{
    public partial class MiPerfil : Page
    {
        private const int MaxArchivosGaleriaCarga = 12;
        private const int MaxTotalGaleria = 24;
        private const string SessMpPerfil = "MpPerfilPrevBin";
        private const string SessMpPerfilMime = "MpPerfilPrevMime";
        private const string SessMpGal = "MpGalStaged";

        private static readonly string PlaceholderAvatar =
            "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='140' height='140'%3E%3Crect fill='%23ccfbf1' width='140' height='140'/%3E%3Ctext x='50%25' y='54%25' dominant-baseline='middle' text-anchor='middle' font-family='system-ui' font-size='52' fill='%2314b8a6'%3E%E2%80%A2%3C/text%3E%3C/svg%3E";

        private int MiUsuarioId => Convert.ToInt32(Session["usuario"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Seguridad/Login.aspx", false);
                return;
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            BrowserSensitiveCookies.ExpirarLegacyRememberCedula(Response);

            var form = Master?.FindControl("form1") as HtmlForm;
            if (form != null)
                form.Enctype = "multipart/form-data";

            if (!IsPostBack)
                CargarPerfil();
            else
                BindGaleriaCompleta();
        }

        private void MostrarMensaje(string texto, bool esError)
        {
            if (string.IsNullOrEmpty(texto))
            {
                lblMensaje.Visible = false;
                lblMensaje.Text = string.Empty;
                return;
            }
            lblMensaje.Visible = true;
            lblMensaje.Text = Server.HtmlEncode(texto);
            lblMensaje.CssClass = "mp-msg " + (esError ? "mp-msg-err" : "mp-msg-ok");
        }

        private void LimpiarStagingFotos()
        {
            Session.Remove(SessMpPerfil);
            Session.Remove(SessMpPerfilMime);
            Session.Remove(SessMpGal);
            if (imgPreviewPerfil != null)
            {
                imgPreviewPerfil.Visible = false;
                imgPreviewPerfil.ImageUrl = string.Empty;
            }
        }

        private void CargarPerfil()
        {
            var u = CN_tbl_usuario.ObtenerPorId(MiUsuarioId);
            if (u == null)
            {
                Session.Clear();
                Response.Redirect("~/Seguridad/Login.aspx", false);
                return;
            }

            litCedula.Text = Server.HtmlEncode(u.usu_cedula ?? "—");
            litTipo.Text = Server.HtmlEncode(u.tbl_tipo_usuario?.tusu_nombre ?? "—");

            if (u.usu_foto_perfil != null && u.usu_foto_perfil.Length > 0)
                imgAvatar.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(u.usu_foto_perfil.ToArray());
            else
                imgAvatar.ImageUrl = PlaceholderAvatar;

            txtNick.Text = u.usu_nick;
            txtNombres.Text = u.usu_nombres;
            txtApellidos.Text = u.usu_apellidos;
            txtCelular.Text = u.usu_celular;
            txtCorreo.Text = u.usu_correo;
            txtDireccion.Text = u.usu_direccion;
            txtFechaNac.Text = u.usu_fecha_cumple.HasValue ? u.usu_fecha_cumple.Value.ToString("yyyy-MM-dd") : string.Empty;
            txtPassword.Text = string.Empty;

            LimpiarStagingFotos();
            BindGaleriaCompleta();
            MostrarMensaje(string.Empty, false);
        }

        private List<GaleriaVistaLinea> ConstruirListaVistaGaleria()
        {
            var rows = new List<GaleriaVistaLinea>();
            foreach (var f in CN_tbl_usuario.ListarFotosGaleriaNoPrincipal(MiUsuarioId))
            {
                byte[] raw = f.ufoto_datos?.ToArray();
                if (raw == null || raw.Length == 0)
                    continue;
                if (!FotoSubidaReglas.EsJpegOPngPorFirma(raw, out string mime))
                    continue;
                rows.Add(new GaleriaVistaLinea
                {
                    CmdArg = "db:" + f.ufoto_id,
                    DataUri = "data:" + mime + ";base64," + Convert.ToBase64String(raw),
                    Pie = "Guardada · ID " + f.ufoto_id
                });
            }
            if (Session[SessMpGal] is List<byte[]> staged)
            {
                for (int i = 0; i < staged.Count; i++)
                {
                    byte[] raw = staged[i];
                    if (raw == null || raw.Length == 0)
                        continue;
                    if (!FotoSubidaReglas.EsJpegOPngPorFirma(raw, out string mime))
                        continue;
                    rows.Add(new GaleriaVistaLinea
                    {
                        CmdArg = "st:" + i,
                        DataUri = "data:" + mime + ";base64," + Convert.ToBase64String(raw),
                        Pie = "Vista previa (pendiente de guardar) · " + (i + 1) + "/" + staged.Count
                    });
                }
            }
            return rows;
        }

        private void BindGaleriaCompleta()
        {
            var rows = ConstruirListaVistaGaleria();
            pnlGaleria.Visible = rows.Count > 0;
            rptGaleria.DataSource = rows;
            rptGaleria.DataBind();
        }

        private List<HttpPostedFile> EnumerarArchivosGaleria()
        {
            return FileUploadMultiplesHelper.EnumerarArchivos(fuGaleria, Request);
        }

        private bool ValidarGaleriaMetadatos(out string error)
        {
            error = null;
            var files = EnumerarArchivosGaleria();
            int stagedCount = (Session[SessMpGal] as List<byte[]>)?.Count ?? 0;
            int n = files.Count > 0 ? files.Count : stagedCount;

            if (n > MaxArchivosGaleriaCarga)
            {
                error = "Máximo " + MaxArchivosGaleriaCarga + " archivos de galería por guardado.";
                return false;
            }
            int existentes = CN_tbl_usuario.ContarFotosGaleriaNoPrincipal(MiUsuarioId);
            if (existentes + n > MaxTotalGaleria)
            {
                error = "La galería no puede superar " + MaxTotalGaleria + " imágenes (tiene " + existentes + ").";
                return false;
            }
            foreach (var f in files)
            {
                string ext = Path.GetExtension(f.FileName)?.ToLowerInvariant() ?? "";
                if (!FotoSubidaReglas.ExtensionesPermitidas.Contains(ext))
                {
                    error = "Galería: solo .jpg, .jpeg o .png.";
                    return false;
                }
                if (f.ContentLength <= 0 || f.ContentLength > FotoSubidaReglas.TamanoMaximoBytes)
                {
                    error = "Galería: cada archivo entre 1 byte y 2 MB.";
                    return false;
                }
            }
            return true;
        }

        private static byte[] LeerStream(Stream s)
        {
            using (var ms = new MemoryStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private List<byte[]> ObtenerGaleriaBytesParaGuardar(out string errGal)
        {
            errGal = null;
            var files = EnumerarArchivosGaleria();
            if (files.Count > 0)
            {
                var list = new List<byte[]>();
                foreach (var f in files)
                {
                    byte[] b = LeerStream(f.InputStream);
                    if (!FotoSubidaReglas.ValidarArchivoFoto(f.FileName, f.ContentLength, b, out _, out string e))
                    {
                        errGal = e;
                        return null;
                    }
                    list.Add(b);
                }
                Session.Remove(SessMpGal);
                return list;
            }
            if (Session[SessMpGal] is List<byte[]> staged && staged.Count > 0)
            {
                var copy = new List<byte[]>(staged);
                Session.Remove(SessMpGal);
                return copy;
            }
            return new List<byte[]>();
        }

        private bool TryObtenerFotoPerfilBinaria(out System.Data.Linq.Binary bin, out string err)
        {
            bin = null;
            err = null;
            if (fuPerfil.HasFile)
            {
                byte[] b = LeerStream(fuPerfil.PostedFile.InputStream);
                if (!FotoSubidaReglas.ValidarArchivoFoto(fuPerfil.FileName, fuPerfil.PostedFile.ContentLength, b, out _, out err))
                    return false;
                bin = new System.Data.Linq.Binary(b);
                return true;
            }
            if (Session[SessMpPerfil] is byte[] pb && pb.Length > 0)
            {
                bin = new System.Data.Linq.Binary((byte[])pb.Clone());
                return true;
            }
            return true;
        }

        protected void btnPrevPerfil_Click(object sender, EventArgs e)
        {
            if (!fuPerfil.HasFile)
            {
                MostrarMensaje("Seleccione JPG o PNG y pulse de nuevo.", true);
                return;
            }
            byte[] b = LeerStream(fuPerfil.PostedFile.InputStream);
            if (!FotoSubidaReglas.ValidarArchivoFoto(fuPerfil.FileName, fuPerfil.PostedFile.ContentLength, b, out string mime, out string err))
            {
                MostrarMensaje(err, true);
                return;
            }
            Session[SessMpPerfil] = b;
            Session[SessMpPerfilMime] = mime;
            imgPreviewPerfil.Visible = true;
            imgPreviewPerfil.ImageUrl = "data:" + mime + ";base64," + Convert.ToBase64String(b);
            MostrarMensaje("Vista previa lista (servidor). Se aplicará al guardar.", false);
        }

        protected void btnValidarGaleria_Click(object sender, EventArgs e)
        {
            Session.Remove(SessMpGal);
            var files = EnumerarArchivosGaleria();
            if (files.Count == 0)
            {
                MostrarMensaje("Seleccione imágenes de galería.", true);
                return;
            }
            if (!ValidarGaleriaMetadatos(out string errMeta))
            {
                MostrarMensaje(errMeta, true);
                return;
            }
            var list = new List<byte[]>();
            foreach (var f in files)
            {
                byte[] b = LeerStream(f.InputStream);
                if (!FotoSubidaReglas.ValidarArchivoFoto(f.FileName, f.ContentLength, b, out _, out string err))
                {
                    MostrarMensaje(err, true);
                    return;
                }
                list.Add(b);
            }
            Session[SessMpGal] = list;
            BindGaleriaCompleta();
            MostrarMensaje(list.Count + " imagen(es) listas en vista previa; pulse Guardar cambios para subirlas.", false);
        }

        private bool Validar(out string error)
        {
            error = null;
            string nick = txtNick.Text.Trim();
            if (!PerfilValidacion.TryValidarNick(nick, out error))
                return false;
            if (CN_tbl_usuario.ExisteNickOtroUsuario(MiUsuarioId, nick))
            {
                error = "Ese nick ya lo usa otro usuario.";
                return false;
            }

            if (!PerfilValidacion.TryValidarNombres(txtNombres.Text, out error))
                return false;
            if (!PerfilValidacion.TryValidarApellidos(txtApellidos.Text, out error))
                return false;
            if (!PerfilValidacion.TryValidarCelular(txtCelular.Text, out error))
                return false;
            if (!PerfilValidacion.TryValidarCorreo(txtCorreo.Text, out error))
                return false;
            if (!PerfilValidacion.TryValidarDireccion(txtDireccion.Text, out error))
                return false;
            if (!PerfilValidacion.TryValidarFechaNacimientoTexto(txtFechaNac.Text, out _, out error))
                return false;
            if (!PerfilValidacion.TryValidarPasswordOpcional(txtPassword.Text, out error))
                return false;

            if (fuPerfil.HasFile)
            {
                byte[] b = LeerStream(fuPerfil.PostedFile.InputStream);
                if (!FotoSubidaReglas.ValidarArchivoFoto(fuPerfil.FileName, fuPerfil.PostedFile.ContentLength, b, out _, out string errP))
                {
                    error = errP;
                    return false;
                }
            }

            if (!ValidarGaleriaMetadatos(out string errGal))
            {
                error = errGal;
                return false;
            }

            return true;
        }

        protected void rptGaleria_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "QuitarGal") return;
            string arg = e.CommandArgument?.ToString() ?? string.Empty;
            if (arg.StartsWith("db:", StringComparison.OrdinalIgnoreCase))
            {
                if (!int.TryParse(arg.Substring(3), out int ufotoId))
                    return;
                if (CN_tbl_usuario.EliminarFotoGaleriaNoPrincipal(ufotoId, MiUsuarioId))
                    MostrarMensaje("Imagen eliminada de su galería.", false);
                else
                    MostrarMensaje("No se pudo eliminar la imagen.", true);
            }
            else if (arg.StartsWith("st:", StringComparison.OrdinalIgnoreCase))
            {
                if (!int.TryParse(arg.Substring(3), out int ix))
                    return;
                if (Session[SessMpGal] is List<byte[]> st && ix >= 0 && ix < st.Count)
                {
                    st.RemoveAt(ix);
                    if (st.Count == 0)
                        Session.Remove(SessMpGal);
                    MostrarMensaje("Imagen quitada de la vista previa.", false);
                }
            }
            BindGaleriaCompleta();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar(out string err))
            {
                MostrarMensaje(err, true);
                return;
            }

            try
            {
                if (!TryObtenerFotoPerfilBinaria(out System.Data.Linq.Binary fp, out string errP))
                {
                    MostrarMensaje(errP, true);
                    return;
                }

                using (var dc = new DataClasses1DataContext())
                {
                    var u = dc.tbl_usuario.FirstOrDefault(x => x.usu_id == MiUsuarioId);
                    if (u == null)
                    {
                        MostrarMensaje("Sesión inválida.", true);
                        return;
                    }

                    u.usu_nick = txtNick.Text.Trim();
                    u.usu_nombres = PerfilValidacion.NormalizarNombresParaGuardar(txtNombres.Text);
                    u.usu_apellidos = PerfilValidacion.NormalizarNombresParaGuardar(txtApellidos.Text);
                    u.usu_celular = Regex.Replace(txtCelular.Text != null ? txtCelular.Text.Trim() : string.Empty, @"\D", "");
                    u.usu_correo = txtCorreo.Text.Trim();
                    u.usu_direccion = txtDireccion.Text.Trim();
                    if (DateTime.TryParse(txtFechaNac.Text, out DateTime fn))
                        u.usu_fecha_cumple = fn;

                    if (fp != null)
                    {
                        u.usu_foto_perfil = fp;
                        var principal = dc.tbl_usuario_foto.FirstOrDefault(f => f.usu_id == MiUsuarioId && (f.ufoto_es_principal ?? false));
                        if (principal != null)
                            principal.ufoto_datos = u.usu_foto_perfil;
                        else
                        {
                            dc.tbl_usuario_foto.InsertOnSubmit(new tbl_usuario_foto
                            {
                                usu_id = MiUsuarioId,
                                ufoto_datos = u.usu_foto_perfil,
                                ufoto_es_principal = true
                            });
                        }
                    }

                    dc.SubmitChanges();
                }

                var gal = ObtenerGaleriaBytesParaGuardar(out string errGal);
                if (errGal != null)
                {
                    MostrarMensaje(errGal, true);
                    return;
                }
                if (gal.Count > 0)
                    CN_tbl_usuario.InsertarFotosGaleria(MiUsuarioId, gal);

                if (!string.IsNullOrEmpty(txtPassword.Text))
                    CN_tbl_usuario.EstablecerContraseñaEncriptadaPorId(MiUsuarioId, txtPassword.Text);

                string nombreCompleto = PerfilValidacion.NormalizarNombresParaGuardar(txtNombres.Text) + " " +
                    PerfilValidacion.NormalizarNombresParaGuardar(txtApellidos.Text);
                Session["NombreCompleto"] = nombreCompleto;
                if (Session["usu"] != null)
                    Session["usu"] = nombreCompleto;
                if (Session["adm"] != null)
                    Session["adm"] = nombreCompleto;

                LimpiarStagingFotos();
                txtPassword.Text = string.Empty;
                CargarPerfil();
                MostrarMensaje("Cambios guardados correctamente.", false);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message, true);
            }
        }
    }
}
