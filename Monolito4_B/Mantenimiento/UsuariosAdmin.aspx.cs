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
    public partial class UsuariosAdmin : Page
    {
        private const int MaxArchivosGaleriaCarga = 12;
        private const int MaxTotalGaleria = 24;
        private const string SessUaPerfil = "UaPerfilPrevBin";
        private const string SessUaPerfilMime = "UaPerfilPrevMime";
        private const string SessUaGal = "UaGalStaged";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tusu_id"] == null || Convert.ToInt32(Session["tusu_id"]) != 1)
            {
                Response.Redirect("~/Mantenimiento/Inicio.aspx", false);
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
            {
                CargarTipos();
                CargarGrid();
            }
            else if (pnlEditor.Visible)
            {
                int.TryParse(hdnUsuarioId.Value, out int uidPb);
                BindGaleriaCompleta(uidPb);
            }
        }

        private void CargarTipos()
        {
            ddlTipo.DataSource = CN_tbl_tipo_usuario.ListarTipoUsuario();
            ddlTipo.DataTextField = "tusu_nombre";
            ddlTipo.DataValueField = "tusu_id";
            ddlTipo.DataBind();
        }

        private void CargarGrid()
        {
            var lista = CN_tbl_usuario.ListarTodosParaAdmin();
            gvUsuarios.DataSource = lista;
            gvUsuarios.DataBind();
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
            lblMensaje.CssClass = "ua-msg " + (esError ? "ua-msg-err" : "ua-msg-ok");
        }

        private bool ValidarFormulario(bool esNuevo, int usuId, out string error)
        {
            error = null;
            string ced = Regex.Replace(txtCedula.Text != null ? txtCedula.Text.Trim() : string.Empty, @"\D", "");
            if (string.IsNullOrEmpty(ced) || ced.Length != 10 || !Regex.IsMatch(ced, @"^\d{10}$"))
            {
                error = "La cédula debe tener exactamente 10 dígitos.";
                return false;
            }

            if (esNuevo && CN_tbl_usuario.traterced(ced) != null)
            {
                error = "Ya existe un usuario con esa cédula.";
                return false;
            }
            if (!esNuevo && CN_tbl_usuario.ExisteCedulaOtroUsuario(usuId, ced))
            {
                error = "Otro usuario ya usa esa cédula.";
                return false;
            }

            string nick = txtNick.Text.Trim();
            if (!PerfilValidacion.TryValidarNick(nick, out error))
                return false;
            if (esNuevo && CN_tbl_usuario.ExisteNick(nick))
            {
                error = "El nick ya está en uso.";
                return false;
            }
            if (!esNuevo && CN_tbl_usuario.ExisteNickOtroUsuario(usuId, nick))
            {
                error = "El nick ya lo usa otro usuario.";
                return false;
            }

            if (!PerfilValidacion.TryValidarNombres(txtNombres.Text, out error))
                return false;
            if (!PerfilValidacion.TryValidarApellidos(txtApellidos.Text, out error))
                return false;
            if (!PerfilValidacion.TryValidarCelular(Regex.Replace(txtCelular.Text != null ? txtCelular.Text : string.Empty, @"\D", ""), out error))
                return false;
            if (!PerfilValidacion.TryValidarCorreo(txtCorreo.Text, out error))
                return false;
            if (!PerfilValidacion.TryValidarDireccion(txtDireccion.Text, out error))
                return false;
            if (!PerfilValidacion.TryValidarFechaNacimientoTexto(txtFechaNac.Text, out _, out error))
                return false;

            if (string.IsNullOrEmpty(ddlTipo.SelectedValue))
            {
                error = "Seleccione el tipo de usuario.";
                return false;
            }

            if (esNuevo && string.IsNullOrEmpty(txtPassword.Text))
            {
                error = "La contraseña es obligatoria al crear un usuario.";
                return false;
            }
            if (!PerfilValidacion.TryValidarPasswordOpcional(txtPassword.Text, out error))
                return false;

            if (fuFoto.HasFile)
            {
                byte[] b = LeerStream(fuFoto.PostedFile.InputStream);
                if (!FotoSubidaReglas.ValidarArchivoFoto(fuFoto.FileName, fuFoto.PostedFile.ContentLength, b, out _, out string errFoto))
                {
                    error = errFoto;
                    return false;
                }
            }

            if (!ValidarGaleriaMetadatos(esNuevo ? 0 : usuId, out string errGal))
            {
                error = errGal;
                return false;
            }

            return true;
        }

        private List<HttpPostedFile> EnumerarArchivosGaleria()
        {
            return FileUploadMultiplesHelper.EnumerarArchivos(fuGaleria, Request);
        }

        private bool ValidarGaleriaMetadatos(int usuIdExistente, out string error)
        {
            error = null;
            var files = EnumerarArchivosGaleria();
            int stagedCount = (Session[SessUaGal] as List<byte[]>)?.Count ?? 0;
            int n = files.Count > 0 ? files.Count : stagedCount;

            if (n > MaxArchivosGaleriaCarga)
            {
                error = "Máximo " + MaxArchivosGaleriaCarga + " archivos de galería por guardado.";
                return false;
            }
            int existentes = usuIdExistente > 0 ? CN_tbl_usuario.ContarFotosGaleriaNoPrincipal(usuIdExistente) : 0;
            if (existentes + n > MaxTotalGaleria)
            {
                error = "La galería no puede superar " + MaxTotalGaleria + " imágenes en total (actuales: " + existentes + ").";
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
                Session.Remove(SessUaGal);
                return list;
            }
            if (Session[SessUaGal] is List<byte[]> staged && staged.Count > 0)
            {
                var copy = new List<byte[]>(staged);
                Session.Remove(SessUaGal);
                return copy;
            }
            return new List<byte[]>();
        }

        private void LimpiarStagingFotos()
        {
            Session.Remove(SessUaPerfil);
            Session.Remove(SessUaPerfilMime);
            Session.Remove(SessUaGal);
            if (imgPreviewPerfil != null)
            {
                imgPreviewPerfil.Visible = false;
                imgPreviewPerfil.ImageUrl = string.Empty;
            }
        }

        private bool TryObtenerFotoPerfilBinaria(out System.Data.Linq.Binary bin, out string err)
        {
            bin = null;
            err = null;
            if (fuFoto.HasFile)
            {
                byte[] b = LeerStream(fuFoto.PostedFile.InputStream);
                if (!FotoSubidaReglas.ValidarArchivoFoto(fuFoto.FileName, fuFoto.PostedFile.ContentLength, b, out _, out err))
                    return false;
                bin = new System.Data.Linq.Binary(b);
                return true;
            }
            if (Session[SessUaPerfil] is byte[] pb && pb.Length > 0)
            {
                bin = new System.Data.Linq.Binary((byte[])pb.Clone());
                return true;
            }
            return true;
        }

        protected void btnPrevPerfil_Click(object sender, EventArgs e)
        {
            if (!fuFoto.HasFile)
            {
                MostrarMensaje("Seleccione una imagen JPG o PNG y pulse de nuevo.", true);
                return;
            }
            byte[] b = LeerStream(fuFoto.PostedFile.InputStream);
            if (!FotoSubidaReglas.ValidarArchivoFoto(fuFoto.FileName, fuFoto.PostedFile.ContentLength, b, out string mime, out string err))
            {
                MostrarMensaje(err, true);
                return;
            }
            Session[SessUaPerfil] = b;
            Session[SessUaPerfilMime] = mime;
            imgPreviewPerfil.Visible = true;
            imgPreviewPerfil.ImageUrl = "data:" + mime + ";base64," + Convert.ToBase64String(b);
            MostrarMensaje("Vista previa de perfil (servidor) lista. Se usará al guardar.", false);
        }

        protected void btnValidarGaleria_Click(object sender, EventArgs e)
        {
            Session.Remove(SessUaGal);
            var files = EnumerarArchivosGaleria();
            if (files.Count == 0)
            {
                MostrarMensaje("Seleccione archivos de galería y pulse Previsualizar galería.", true);
                return;
            }
            if (!int.TryParse(hdnUsuarioId.Value, out int uidEd))
                uidEd = 0;
            if (!ValidarGaleriaMetadatos(uidEd, out string errMeta))
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
            Session[SessUaGal] = list;
            BindGaleriaCompleta(uidEd);
            MostrarMensaje(list.Count + " imagen(es) en vista previa en servidor; se guardarán al pulsar Guardar.", false);
        }

        private List<GaleriaVistaLinea> ConstruirListaVistaGaleria(int usuId)
        {
            var rows = new List<GaleriaVistaLinea>();
            if (usuId > 0)
            {
                foreach (var f in CN_tbl_usuario.ListarFotosGaleriaNoPrincipal(usuId))
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
            }
            if (Session[SessUaGal] is List<byte[]> staged)
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

        private void BindGaleriaCompleta(int usuId)
        {
            var rows = ConstruirListaVistaGaleria(usuId);
            pnlGaleriaExistente.Visible = rows.Count > 0;
            rptGaleria.DataSource = rows;
            rptGaleria.DataBind();
        }

        private static byte[] LeerStream(Stream s)
        {
            using (var ms = new MemoryStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            pnlLista.Visible = false;
            pnlEditor.Visible = true;
            hdnUsuarioId.Value = "0";
            litTituloForm.Text = "Nuevo usuario";
            LimpiarFormulario();
            BindGaleriaCompleta(0);
            MostrarMensaje(string.Empty, false);
        }

        private void LimpiarFormulario()
        {
            txtCedula.Text = string.Empty;
            txtNick.Text = string.Empty;
            txtNombres.Text = string.Empty;
            txtApellidos.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtFechaNac.Text = string.Empty;
            txtPassword.Text = string.Empty;
            if (ddlTipo.Items.Count > 0) ddlTipo.SelectedIndex = 0;
            ddlEstado.SelectedValue = "A";
            LimpiarStagingFotos();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlEditor.Visible = false;
            pnlLista.Visible = true;
            LimpiarStagingFotos();
            CargarGrid();
            MostrarMensaje(string.Empty, false);
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrEmpty(e.CommandArgument?.ToString())) return;
            int id = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Eliminar")
            {
                int yo = Convert.ToInt32(Session["usuario"]);
                if (id == yo)
                {
                    MostrarMensaje("No puede eliminar su propio usuario mientras tiene la sesión iniciada.", true);
                    return;
                }
                if (CN_tbl_usuario.EliminarUsuarioPorId(id))
                    MostrarMensaje("Usuario eliminado.", false);
                else
                    MostrarMensaje("No se pudo eliminar el usuario.", true);
                CargarGrid();
                return;
            }

            if (e.CommandName != "Editar") return;

            tbl_usuario u = CN_tbl_usuario.ObtenerPorId(id);
            if (u == null)
            {
                MostrarMensaje("Usuario no encontrado.", true);
                return;
            }

            pnlLista.Visible = false;
            pnlEditor.Visible = true;
            hdnUsuarioId.Value = id.ToString();
            litTituloForm.Text = "Editar usuario #" + id;
            LimpiarStagingFotos();

            txtCedula.Text = u.usu_cedula;
            txtNick.Text = u.usu_nick;
            txtNombres.Text = u.usu_nombres;
            txtApellidos.Text = u.usu_apellidos;
            txtCelular.Text = u.usu_celular;
            txtCorreo.Text = u.usu_correo;
            txtDireccion.Text = u.usu_direccion;
            txtFechaNac.Text = u.usu_fecha_cumple.HasValue ? u.usu_fecha_cumple.Value.ToString("yyyy-MM-dd") : string.Empty;
            txtPassword.Text = string.Empty;

            if (u.tusu_id.HasValue)
            {
                var it = ddlTipo.Items.FindByValue(u.tusu_id.Value.ToString());
                if (it != null) ddlTipo.SelectedValue = u.tusu_id.Value.ToString();
            }

            ddlEstado.SelectedValue = (u.usu_estado == 'T') ? "T" : "A";
            BindGaleriaCompleta(id);
            MostrarMensaje(string.Empty, false);
        }

        protected void rptGaleria_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "QuitarGal") return;
            if (!int.TryParse(hdnUsuarioId.Value, out int editId))
                editId = 0;

            string arg = e.CommandArgument?.ToString() ?? string.Empty;
            if (arg.StartsWith("db:", StringComparison.OrdinalIgnoreCase))
            {
                if (editId <= 0 || !int.TryParse(arg.Substring(3), out int ufotoId))
                    return;
                if (CN_tbl_usuario.EliminarFotoGaleriaNoPrincipal(ufotoId, editId))
                    MostrarMensaje("Imagen eliminada de la galería.", false);
                else
                    MostrarMensaje("No se pudo eliminar la imagen.", true);
            }
            else if (arg.StartsWith("st:", StringComparison.OrdinalIgnoreCase))
            {
                if (!int.TryParse(arg.Substring(3), out int ix))
                    return;
                if (Session[SessUaGal] is List<byte[]> st && ix >= 0 && ix < st.Count)
                {
                    st.RemoveAt(ix);
                    if (st.Count == 0)
                        Session.Remove(SessUaGal);
                    MostrarMensaje("Imagen quitada de la vista previa.", false);
                }
            }
            BindGaleriaCompleta(editId);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int usuId = 0;
            int.TryParse(hdnUsuarioId.Value, out usuId);
            bool esNuevo = usuId <= 0;

            if (!ValidarFormulario(esNuevo, esNuevo ? 0 : usuId, out string err))
            {
                MostrarMensaje(err, true);
                return;
            }

            try
            {
                string nomN = PerfilValidacion.NormalizarNombresParaGuardar(txtNombres.Text);
                string apeN = PerfilValidacion.NormalizarNombresParaGuardar(txtApellidos.Text);
                string cedN = Regex.Replace(txtCedula.Text != null ? txtCedula.Text.Trim() : string.Empty, @"\D", "");
                string celN = Regex.Replace(txtCelular.Text != null ? txtCelular.Text.Trim() : string.Empty, @"\D", "");

                if (esNuevo)
                {
                    using (var dc = new DataClasses1DataContext())
                    {
                        var u = new tbl_usuario
                        {
                            tusu_id = int.Parse(ddlTipo.SelectedValue),
                            usu_cedula = cedN,
                            usu_nick = txtNick.Text.Trim(),
                            usu_nombres = nomN,
                            usu_apellidos = apeN,
                            usu_celular = celN,
                            usu_correo = txtCorreo.Text.Trim(),
                            usu_direccion = txtDireccion.Text.Trim(),
                            usu_estado = ddlEstado.SelectedValue[0],
                            usu_fecha_creacion = DateTime.Now,
                            usu_intentos = 0,
                            usu_contraseña = null
                        };
                        if (DateTime.TryParse(txtFechaNac.Text, out DateTime fn))
                            u.usu_fecha_cumple = fn;

                        if (!TryObtenerFotoPerfilBinaria(out System.Data.Linq.Binary fp, out string errP))
                        {
                            MostrarMensaje(errP, true);
                            return;
                        }
                        if (fp != null)
                            u.usu_foto_perfil = fp;

                        dc.tbl_usuario.InsertOnSubmit(u);
                        dc.SubmitChanges();

                        dc.ExecuteCommand("UPDATE tbl_usuario SET usu_contraseña = dbo.encriptacion({0}) WHERE usu_id = {1}",
                            txtPassword.Text, u.usu_id);

                        if (u.usu_foto_perfil != null)
                        {
                            dc.tbl_usuario_foto.InsertOnSubmit(new tbl_usuario_foto
                            {
                                usu_id = u.usu_id,
                                ufoto_datos = u.usu_foto_perfil,
                                ufoto_es_principal = true
                            });
                            dc.SubmitChanges();
                        }

                        var bytesGaleriaNuevo = ObtenerGaleriaBytesParaGuardar(out string errGalN);
                        if (errGalN != null)
                        {
                            MostrarMensaje(errGalN, true);
                            return;
                        }
                        if (bytesGaleriaNuevo.Count > 0)
                            CN_tbl_usuario.InsertarFotosGaleria(u.usu_id, bytesGaleriaNuevo);
                    }
                    MostrarMensaje("Usuario creado correctamente.", false);
                }
                else
                {
                    using (var dc = new DataClasses1DataContext())
                    {
                        var u = dc.tbl_usuario.FirstOrDefault(x => x.usu_id == usuId);
                        if (u == null)
                        {
                            MostrarMensaje("Usuario no encontrado.", true);
                            return;
                        }

                        u.usu_cedula = cedN;
                        u.usu_nick = txtNick.Text.Trim();
                        u.usu_nombres = nomN;
                        u.usu_apellidos = apeN;
                        u.usu_celular = celN;
                        u.usu_correo = txtCorreo.Text.Trim();
                        u.usu_direccion = txtDireccion.Text.Trim();
                        u.tusu_id = int.Parse(ddlTipo.SelectedValue);
                        u.usu_estado = ddlEstado.SelectedValue[0];
                        if (DateTime.TryParse(txtFechaNac.Text, out DateTime fn2))
                            u.usu_fecha_cumple = fn2;

                        if (!TryObtenerFotoPerfilBinaria(out System.Data.Linq.Binary fp2, out string errP2))
                        {
                            MostrarMensaje(errP2, true);
                            return;
                        }
                        if (fp2 != null)
                        {
                            u.usu_foto_perfil = fp2;
                            var principal = dc.tbl_usuario_foto.FirstOrDefault(f => f.usu_id == usuId && (f.ufoto_es_principal ?? false));
                            if (principal != null)
                            {
                                principal.ufoto_datos = u.usu_foto_perfil;
                            }
                            else
                            {
                                dc.tbl_usuario_foto.InsertOnSubmit(new tbl_usuario_foto
                                {
                                    usu_id = usuId,
                                    ufoto_datos = u.usu_foto_perfil,
                                    ufoto_es_principal = true
                                });
                            }
                        }

                        dc.SubmitChanges();
                    }

                    var bytesGaleriaEdit = ObtenerGaleriaBytesParaGuardar(out string errGalE);
                    if (errGalE != null)
                    {
                        MostrarMensaje(errGalE, true);
                        return;
                    }
                    if (bytesGaleriaEdit.Count > 0)
                        CN_tbl_usuario.InsertarFotosGaleria(usuId, bytesGaleriaEdit);

                    if (!string.IsNullOrEmpty(txtPassword.Text))
                        CN_tbl_usuario.EstablecerContraseñaEncriptadaPorId(usuId, txtPassword.Text);

                    MostrarMensaje("Usuario actualizado.", false);
                }

                pnlEditor.Visible = false;
                pnlLista.Visible = true;
                LimpiarStagingFotos();
                CargarGrid();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message, true);
            }
        }
    }
}
