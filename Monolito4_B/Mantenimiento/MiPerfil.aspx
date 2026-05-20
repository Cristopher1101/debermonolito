<%@ Page Title="Mi perfil" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="Monolito4_B.Mantenimiento.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .mp-wrap { max-width: 1040px; margin: 0 auto; }
        .mp-hero {
            background: linear-gradient(125deg, #0d9488 0%, #14b8a6 40%, #2dd4bf 100%);
            color: #fff;
            border-radius: 24px;
            padding: 32px 36px 36px;
            margin-bottom: 28px;
            box-shadow: 0 20px 50px rgba(13,148,136,.35);
            position: relative;
            overflow: hidden;
        }
        .mp-hero::after {
            content: ""; position: absolute; right: -40px; top: -40px;
            width: 180px; height: 180px; border-radius: 50%;
            background: rgba(255,255,255,.12);
        }
        .mp-hero h1 { font-size: 1.75rem; font-weight: 700; margin: 0 0 10px; letter-spacing: -.03em; position: relative; z-index: 1; }
        .mp-hero p { margin: 0; opacity: .95; font-size: 1rem; max-width: 38rem; line-height: 1.6; position: relative; z-index: 1; }
        .mp-layout {
            display: grid;
            grid-template-columns: 280px 1fr;
            gap: 28px;
            align-items: start;
        }
        @media (max-width: 900px) { .mp-layout { grid-template-columns: 1fr; } }
        .mp-card {
            background: #fff;
            border-radius: 20px;
            padding: 26px 24px 28px;
            box-shadow: 0 10px 40px rgba(15,23,42,.08);
            border: 1px solid #e2e8f0;
        }
        .mp-card h2 { margin: 0 0 18px; font-size: 1.1rem; color: #0f766e; font-weight: 700; }
        .mp-avatar-wrap { text-align: center; margin-bottom: 18px; }
        .mp-avatar {
            width: 140px; height: 140px; border-radius: 50%; object-fit: cover;
            border: 4px solid #ccfbf1; box-shadow: 0 8px 24px rgba(13,148,136,.25);
            background: #f0fdfa;
        }
        .mp-meta { font-size: .82rem; color: #64748b; line-height: 1.6; }
        .mp-meta strong { color: #334155; }
        .mp-msg { padding: 14px 18px; border-radius: 14px; margin-bottom: 20px; font-size: .9rem; }
        .mp-msg-ok { background: #ecfdf5; color: #047857; border: 1px solid #a7f3d0; }
        .mp-msg-err { background: #fef2f2; color: #b91c1c; border: 1px solid #fecaca; }
        .mp-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 16px 20px;
        }
        @media (max-width: 640px) { .mp-grid { grid-template-columns: 1fr; } .mp-span2 { grid-column: span 1 !important; } }
        .mp-span2 { grid-column: span 2; }
        .mp-field label {
            display: block; font-size: .72rem; font-weight: 700; text-transform: uppercase;
            letter-spacing: .06em; color: #94a3b8; margin-bottom: 6px;
        }
        .mp-field input[type=text], .mp-field input[type=password], .mp-field input[type=email], .mp-field input[type=date] {
            width: 100%; height: 46px; padding: 0 14px; border-radius: 12px;
            border: 1px solid #e2e8f0; font-size: .92rem; font-family: inherit;
            background: #f8fafc; transition: border-color .2s, box-shadow .2s;
        }
        .mp-field input:focus { outline: none; border-color: #14b8a6; box-shadow: 0 0 0 3px rgba(20,184,166,.2); }
        .mp-field .hint { font-size: .75rem; color: #94a3b8; margin-top: 4px; display: block; }
        .mp-actions { display: flex; flex-wrap: wrap; gap: 12px; margin-top: 24px; }
        .mp-btn {
            display: inline-flex; align-items: center; justify-content: center; gap: 8px;
            padding: 12px 22px; border-radius: 12px; border: none;
            font-weight: 600; font-size: .9rem; cursor: pointer;
            transition: transform .15s, box-shadow .2s;
        }
        .mp-btn-primary { background: linear-gradient(135deg, #0d9488, #14b8a6); color: #fff; box-shadow: 0 6px 20px rgba(13,148,136,.35); }
        .mp-btn-primary:hover { transform: translateY(-1px); box-shadow: 0 10px 28px rgba(13,148,136,.4); }
        .mp-galeria-list { display: flex; flex-direction: column; gap: 8px; margin-top: 10px; }
        .mp-galeria-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(148px, 1fr));
            gap: 14px;
            margin-top: 10px;
        }
        .mp-galeria-card {
            padding: 10px;
            background: #f8fafc;
            border-radius: 12px;
            border: 1px solid #e2e8f0;
        }
        .mp-galeria-thumb {
            width: 100%;
            height: 120px;
            object-fit: cover;
            border-radius: 10px;
            display: block;
            border: 1px solid #e2e8f0;
        }
        .mp-galeria-row {
            display: flex; align-items: center; justify-content: space-between; gap: 12px;
            padding: 10px 14px; background: #f8fafc; border-radius: 12px; border: 1px solid #e2e8f0;
        }
        .mp-galeria-meta { font-size: .82rem; color: #0f766e; font-weight: 500; line-height: 1.35; }
        .mp-link { color: #0d9488; font-weight: 600; text-decoration: none; background: none; border: none; cursor: pointer; font-size: inherit; font-family: inherit; }
        .mp-link:hover { text-decoration: underline; }
        .mp-preview { max-width: 220px; border-radius: 12px; margin-top: 10px; border: 1px solid #e2e8f0; display: block; }
        .mp-media-block {
            margin-top: 22px;
            padding: 20px 18px 18px;
            border-radius: 16px;
            border: 1px solid #e2e8f0;
            background: linear-gradient(180deg, #f8fafc 0%, #fff 100%);
        }
        .mp-media-block .mp-subh { margin: 0 0 16px; font-size: .95rem; font-weight: 700; color: #0f766e; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mp-wrap">
        <div class="mp-hero">
            <h1>Mi perfil</h1>
            <p>Actualice sus datos de contacto, contraseña y fotos. La cédula y el tipo de cuenta no se modifican aquí por seguridad.</p>
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="mp-msg" Visible="false" EnableViewState="false" />

        <div class="mp-layout">
            <div class="mp-card">
                <h2>Foto actual</h2>
                <div class="mp-avatar-wrap">
                    <asp:Image ID="imgAvatar" runat="server" CssClass="mp-avatar" AlternateText="Foto de perfil" />
                </div>
                <p class="mp-meta"><strong>Cédula:</strong><br /><asp:Literal ID="litCedula" runat="server" /></p>
                <p class="mp-meta" style="margin-top:12px;"><strong>Perfil:</strong><br /><asp:Literal ID="litTipo" runat="server" /></p>
            </div>

            <div class="mp-card">
                <h2>Datos y multimedia</h2>
                <div class="mp-grid">
                    <div class="mp-field">
                        <label for="<%= txtNick.ClientID %>">Nick</label>
                        <asp:TextBox ID="txtNick" runat="server" MaxLength="50" autocomplete="off" />
                    </div>
                    <div class="mp-field">
                        <label for="<%= txtCelular.ClientID %>">Celular</label>
                        <asp:TextBox ID="txtCelular" runat="server" MaxLength="10" ClientIDMode="Static" inputmode="numeric" pattern="[0-9]*" placeholder="0999999999" autocomplete="off" />
                    </div>
                    <div class="mp-field mp-span2">
                        <label for="<%= txtNombres.ClientID %>">Nombres (mínimo 2 palabras, solo letras)</label>
                        <asp:TextBox ID="txtNombres" runat="server" MaxLength="50" autocomplete="off" />
                    </div>
                    <div class="mp-field mp-span2">
                        <label for="<%= txtApellidos.ClientID %>">Apellidos (mínimo 2 palabras, solo letras)</label>
                        <asp:TextBox ID="txtApellidos" runat="server" MaxLength="50" autocomplete="off" />
                    </div>
                    <div class="mp-field">
                        <label for="<%= txtFechaNac.ClientID %>">Fecha de nacimiento</label>
                        <asp:TextBox ID="txtFechaNac" runat="server" TextMode="Date" />
                    </div>
                    <div class="mp-field mp-span2">
                        <label for="<%= txtCorreo.ClientID %>">Correo</label>
                        <asp:TextBox ID="txtCorreo" runat="server" TextMode="Email" MaxLength="150" placeholder="correo@dominio.com" autocomplete="off" />
                    </div>
                    <div class="mp-field mp-span2">
                        <label for="<%= txtDireccion.ClientID %>">Dirección (máx. 50 caracteres)</label>
                        <asp:TextBox ID="txtDireccion" runat="server" MaxLength="50" autocomplete="off" />
                    </div>
                    <div class="mp-field mp-span2">
                        <label for="<%= txtPassword.ClientID %>">Nueva contraseña</label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" autocomplete="new-password" />
                        <span class="hint">Deje en blanco si no desea cambiarla (mínimo 6 caracteres si escribe una).</span>
                    </div>
                </div>

                <div class="mp-media-block">
                    <p class="mp-subh">Fotografías</p>
                    <div class="mp-grid">
                    <div class="mp-field mp-span2">
                        <label for="<%= fuPerfil.ClientID %>">Cambiar foto de perfil</label>
                        <asp:FileUpload ID="fuPerfil" runat="server" accept="image/jpeg,image/png,.jpg,.jpeg,.png" />
                        <span class="hint">Solo JPG o PNG · máx. 2 MB. Pulse «Vista previa foto» para verla desde el servidor.</span>
                        <div style="margin-top:10px;">
                            <asp:Button ID="btnPrevPerfil" runat="server" CssClass="mp-btn mp-btn-primary" style="padding:10px 18px;font-size:.85rem;" Text="Vista previa foto" CausesValidation="false" OnClick="btnPrevPerfil_Click" />
                        </div>
                        <asp:Image ID="imgPreviewPerfil" runat="server" Visible="false" CssClass="mp-preview" AlternateText="Vista previa" />
                    </div>
                    <div class="mp-field mp-span2">
                        <label for="<%= fuGaleria.ClientID %>">Añadir imágenes a la galería</label>
                        <asp:FileUpload ID="fuGaleria" runat="server" AllowMultiple="true" accept="image/jpeg,image/png,.jpg,.jpeg,.png" />
                        <span class="hint">Hasta 12 archivos por envío · 2 MB c/u · máx. 24 en galería. Pulse «Previsualizar galería» y luego Guardar cambios.</span>
                        <div style="margin-top:10px;">
                            <asp:Button ID="btnValidarGaleria" runat="server" CssClass="mp-btn mp-btn-primary" style="padding:10px 18px;font-size:.85rem;background:linear-gradient(135deg,#0f766e,#14b8a6);" Text="Previsualizar galería" CausesValidation="false" OnClick="btnValidarGaleria_Click" />
                        </div>
                    </div>
                    </div>
                </div>

                <asp:Panel ID="pnlGaleria" runat="server" Visible="false" CssClass="mp-field mp-span2" style="margin-top:8px;">
                    <label>Galería · guardadas y vista previa</label>
                    <asp:Repeater ID="rptGaleria" runat="server" EnableViewState="false" OnItemCommand="rptGaleria_ItemCommand">
                        <HeaderTemplate><div class="mp-galeria-grid"></HeaderTemplate>
                        <ItemTemplate>
                            <div class="mp-galeria-card">
                                <img src="<%# Eval("DataUri") %>" alt="" class="mp-galeria-thumb" />
                                <div class="mp-galeria-row" style="margin-top:8px;border:none;padding:8px 0;background:transparent;flex-direction:column;align-items:stretch;gap:6px;">
                                    <span class="mp-galeria-meta"><%# Eval("Pie") %></span>
                                    <asp:LinkButton runat="server" CssClass="mp-link" Text="Quitar" CommandName="QuitarGal"
                                        CommandArgument='<%# Eval("CmdArg") %>' CausesValidation="false"
                                        OnClientClick="return confirm('¿Quitar esta imagen de la lista?');" />
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate></div></FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>

                <div class="mp-actions">
                    <asp:Button ID="btnGuardar" runat="server" CssClass="mp-btn mp-btn-primary" Text="Guardar cambios" OnClick="btnGuardar_Click" OnClientClick="return mpValidarAntesDeGuardar();" />
                </div>
            </div>
        </div>
    </div>

    <script>
        function contarPalabras(t) {
            if (!t || !t.trim()) return 0;
            return t.trim().split(/\s+/).filter(function (x) { return x.length > 0; }).length;
        }
        function perfilEdadDesdeYmd(fnStr) {
            if (!fnStr || !/^\d{4}-\d{2}-\d{2}$/.test(fnStr)) return NaN;
            var p = fnStr.split('-');
            var d = new Date(parseInt(p[0], 10), parseInt(p[1], 10) - 1, parseInt(p[2], 10));
            if (isNaN(d.getTime())) return NaN;
            var now = new Date();
            var age = now.getFullYear() - d.getFullYear();
            var m = now.getMonth() - d.getMonth();
            if (m < 0 || (m === 0 && now.getDate() < d.getDate())) age--;
            return age;
        }
        function perfilNombreSoloLetrasOk(s) {
            var t = (s || '').trim().replace(/\s+/g, ' ');
            if (!t || /\d/.test(t)) return false;
            return /^[\p{L}\s'-]+$/u.test(t);
        }
        function perfilFiltrarSoloLetrasNombre(el) {
            if (!el) return;
            el.addEventListener('input', function () {
                var v = el.value.replace(/[^\p{L}\s'-]/gu, '');
                el.value = v.replace(/\s+/g, ' ');
            });
        }
        function mpValidarAntesDeGuardar() {
            var nick = document.getElementById('<%= txtNick.ClientID %>');
            var nom = document.getElementById('<%= txtNombres.ClientID %>');
            var ape = document.getElementById('<%= txtApellidos.ClientID %>');
            var cel = document.getElementById('txtCelular');
            var mail = document.getElementById('<%= txtCorreo.ClientID %>');
            var dir = document.getElementById('<%= txtDireccion.ClientID %>');
            var fn = document.getElementById('<%= txtFechaNac.ClientID %>');
            var pass = document.getElementById('<%= txtPassword.ClientID %>');
            if (!nick || nick.value.trim().length < 4) { alert('El nick debe tener al menos 4 caracteres.'); return false; }
            if (!/^[\p{L}0-9._-]{4,50}$/u.test(nick.value.trim())) { alert('El nick solo puede usar letras, números y los símbolos . _ - (como en el registro).'); return false; }
            if (contarPalabras(nom.value) < 2) { alert('Ingrese al menos dos nombres separados por espacio.'); return false; }
            if (contarPalabras(ape.value) < 2) { alert('Ingrese al menos dos apellidos separados por espacio.'); return false; }
            if (!perfilNombreSoloLetrasOk(nom.value)) { alert('Los nombres solo pueden contener letras (espacios entre palabras; se permiten \' y - entre letras).'); return false; }
            if (!perfilNombreSoloLetrasOk(ape.value)) { alert('Los apellidos solo pueden contener letras (espacios entre palabras; se permiten \' y - entre letras).'); return false; }
            var cd = (cel.value || '').replace(/\D/g, '');
            if (cd.length < 9 || cd.length > 10) { alert('El celular debe tener 9 o 10 dígitos.'); return false; }
            if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test((mail.value || '').trim())) { alert('Correo no válido.'); return false; }
            var d = (dir.value || '').trim();
            if (d.length < 5 || d.length > 50) { alert('La dirección debe tener entre 5 y 50 caracteres.'); return false; }
            if (!fn.value) { alert('Seleccione la fecha de nacimiento.'); return false; }
            var edad = perfilEdadDesdeYmd(fn.value);
            if (isNaN(edad) || edad < 5 || edad > 120) { alert('La fecha de nacimiento debe corresponder a una edad entre 5 y 120 años.'); return false; }
            if (pass.value.length > 0 && pass.value.length < 6) { alert('La contraseña debe tener al menos 6 caracteres.'); return false; }
            return true;
        }
        document.addEventListener('DOMContentLoaded', function () {
            perfilFiltrarSoloLetrasNombre(document.getElementById('<%= txtNombres.ClientID %>'));
            perfilFiltrarSoloLetrasNombre(document.getElementById('<%= txtApellidos.ClientID %>'));
            var cel = document.getElementById('txtCelular');
            if (!cel) return;
            cel.addEventListener('input', function () {
                var v = (cel.value || '').replace(/\D/g, '').substring(0, 10);
                cel.value = v;
            });
        });
    </script>
</asp:Content>
