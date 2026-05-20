<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="UsuariosAdmin.aspx.cs" Inherits="Monolito4_B.Mantenimiento.UsuariosAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ua-wrap {
            max-width: 1180px;
            margin: 0 auto;
        }

        .ua-hero {
            background: linear-gradient(135deg, #1a237e 0%, #3949ab 45%, #5c6bc0 100%);
            color: #fff;
            border-radius: 20px;
            padding: 28px 32px 32px;
            margin-bottom: 26px;
            box-shadow: 0 16px 40px rgba(26,35,126,.35);
        }

            .ua-hero h1 {
                font-size: 1.65rem;
                font-weight: 700;
                margin: 0 0 8px;
                letter-spacing: -.02em;
            }

            .ua-hero p {
                margin: 0;
                opacity: .92;
                font-size: 1rem;
                line-height: 1.6;
                max-width: 38rem;
            }

        .ua-toolbar {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            align-items: center;
            margin-bottom: 18px;
        }

        .ua-btn {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding: 11px 20px;
            border-radius: 12px;
            border: none;
            font-weight: 600;
            font-size: .9rem;
            cursor: pointer;
            transition: transform .15s, box-shadow .2s;
        }

        .ua-btn-primary {
            background: linear-gradient(135deg, #3949ab, #5c6bc0);
            color: #fff;
            box-shadow: 0 6px 20px rgba(57,73,171,.4);
        }

            .ua-btn-primary:hover {
                transform: translateY(-1px);
                box-shadow: 0 10px 28px rgba(57,73,171,.45);
            }

        .ua-btn-ghost {
            background: #fff;
            color: #3949ab;
            border: 2px solid #c5cae9;
        }

            .ua-btn-ghost:hover {
                background: #e8eaf6;
            }

        .ua-msg {
            padding: 14px 18px;
            border-radius: 14px;
            margin-bottom: 18px;
            font-size: .9rem;
            line-height: 1.45;
        }

        .ua-msg-ok {
            background: #e8f5e9;
            color: #1b5e20;
            border: 1px solid #a5d6a7;
        }

        .ua-msg-err {
            background: #ffebee;
            color: #b71c1c;
            border: 1px solid #ffcdd2;
        }

        .ua-card {
            background: #fff;
            border-radius: 18px;
            padding: 24px 26px 28px;
            box-shadow: 0 8px 32px rgba(0,0,0,.07);
            border: 1px solid #e8eaf6;
            margin-bottom: 22px;
        }

            .ua-card h2 {
                margin: 0 0 18px;
                font-size: 1.15rem;
                color: #1a237e;
            }

        .ua-grid-form {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 16px 20px;
        }

        @media (max-width: 720px) {
            .ua-grid-form {
                grid-template-columns: 1fr;
            }

            .ua-span2 {
                grid-column: span 1 !important;
            }
        }

        .ua-span2 {
            grid-column: span 2;
        }

        .ua-field label {
            display: block;
            font-size: .72rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: .06em;
            color: #7986cb;
            margin-bottom: 6px;
        }

        .ua-field input[type=text], .ua-field input[type=password], .ua-field input[type=email], .ua-field input[type=date], .ua-field select {
            width: 100%;
            height: 46px;
            padding: 0 14px;
            border-radius: 12px;
            border: 1px solid #c5cae9;
            font-size: .92rem;
            font-family: inherit;
            background: #fafbff;
            transition: border-color .2s, box-shadow .2s;
        }

            .ua-field input:focus, .ua-field select:focus {
                outline: none;
                border-color: #5c6bc0;
                box-shadow: 0 0 0 3px rgba(92,107,192,.2);
            }

        .ua-field .hint {
            font-size: .75rem;
            color: #9e9e9e;
            margin-top: 4px;
        }

        .ua-gv {
            width: 100%;
            border-collapse: collapse;
            font-size: .86rem;
        }

            .ua-gv th {
                text-align: left;
                padding: 12px 10px;
                background: #e8eaf6;
                color: #283593;
                font-weight: 600;
                border-bottom: 2px solid #c5cae9;
            }

            .ua-gv td {
                padding: 11px 10px;
                border-bottom: 1px solid #eceff1;
                vertical-align: middle;
            }

            .ua-gv tr:hover td {
                background: #f5f7ff;
            }

        .ua-badge {
            display: inline-block;
            padding: 4px 10px;
            border-radius: 999px;
            font-size: .72rem;
            font-weight: 600;
        }

        .ua-badge-a {
            background: #e8f5e9;
            color: #2e7d32;
        }

        .ua-badge-t {
            background: #fff3e0;
            color: #e65100;
        }

        .ua-link {
            color: #3949ab;
            font-weight: 600;
            text-decoration: none;
            margin-right: 10px;
        }

            .ua-link:hover {
                text-decoration: underline;
            }

        .ua-form-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            margin-top: 22px;
        }

        .ua-galeria-list {
            display: flex;
            flex-direction: column;
            gap: 8px;
            margin-top: 8px;
        }

        .ua-galeria-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(148px, 1fr));
            gap: 14px;
            margin-top: 8px;
        }

        .ua-galeria-card {
            padding: 10px;
            background: #f5f7ff;
            border-radius: 12px;
            border: 1px solid #e8eaf6;
        }

        .ua-galeria-thumb {
            width: 100%;
            height: 120px;
            object-fit: cover;
            border-radius: 10px;
            display: block;
            border: 1px solid #e8eaf6;
        }

        .ua-galeria-row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 12px;
            padding: 10px 14px;
            background: #f5f7ff;
            border-radius: 12px;
            border: 1px solid #e8eaf6;
        }

        .ua-galeria-meta {
            font-size: .82rem;
            color: #5c6bc0;
            line-height: 1.35;
        }

        .ua-preview-img {
            max-width: 220px;
            border-radius: 14px;
            margin-top: 12px;
            border: 1px solid #e8eaf6;
            display: block;
        }

        .ua-media-block {
            margin-top: 22px;
            padding: 20px 18px 18px;
            border-radius: 16px;
            border: 1px solid #e8eaf6;
            background: linear-gradient(180deg, #fafbff 0%, #fff 100%);
        }

            .ua-media-block .ua-subh {
                margin: 0 0 16px;
                font-size: .95rem;
                font-weight: 700;
                color: #3949ab;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ua-wrap">
        <div class="ua-hero">
            <h1>Gestión de usuarios</h1>
            <p>Alta, edición y baja de cuentas. Las contraseñas se guardan cifradas. No puede eliminar su propia sesión.</p>
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="ua-msg" Visible="false" EnableViewState="false" />

        <asp:Panel ID="pnlLista" runat="server">
            <div class="ua-toolbar">
                <asp:Button ID="btnNuevo" runat="server" CssClass="ua-btn ua-btn-primary" Text="+ Nuevo usuario" OnClick="btnNuevo_Click" />
            </div>
            <div class="ua-card">
                <h2>Listado</h2>
                <asp:GridView ID="gvUsuarios" runat="server" CssClass="ua-gv" AutoGenerateColumns="false"
                    DataKeyNames="usu_id" OnRowCommand="gvUsuarios_RowCommand" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="usu_id" HeaderText="ID" ItemStyle-Width="48px" />
                        <asp:BoundField DataField="usu_cedula" HeaderText="Cédula" />
                        <asp:BoundField DataField="usu_nick" HeaderText="Nick" />
                        <asp:TemplateField HeaderText="Nombre">
                            <ItemTemplate>
                                <%# Eval("usu_nombres") %> <%# Eval("usu_apellidos") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="usu_correo" HeaderText="Correo" />
                        <asp:TemplateField HeaderText="Perfil" ItemStyle-Width="120px">
                            <ItemTemplate>
                                <%# Eval("tbl_tipo_usuario") != null ? Eval("tbl_tipo_usuario.tusu_nombre") : "—" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <span class='<%# (Eval("usu_estado") != null && Eval("usu_estado").ToString() == "A") ? "ua-badge ua-badge-a" : "ua-badge ua-badge-t" %>'>
                                    <%# (Eval("usu_estado") != null && Eval("usu_estado").ToString() == "A") ? "Activo" : "Bloq." %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="160px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CssClass="ua-link" CommandName="Editar" CommandArgument='<%# Eval("usu_id") %>' Text="Editar" CausesValidation="false" />
                                <asp:LinkButton runat="server" CssClass="ua-link" CommandName="Eliminar" CommandArgument='<%# Eval("usu_id") %>'
                                    Text="Eliminar" CausesValidation="false"
                                    OnClientClick="return confirm('¿Eliminar este usuario? Esta acción no se puede deshacer.');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <p style="padding: 20px; color: #757575;">No hay usuarios registrados.</p>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlEditor" runat="server" Visible="false" CssClass="ua-card">
            <h2>
                <asp:Literal ID="litTituloForm" runat="server" /></h2>
            <asp:HiddenField ID="hdnUsuarioId" runat="server" Value="0" ClientIDMode="Static" />

            <div class="ua-grid-form">
                <div class="ua-field">
                    <label for="<%= txtCedula.ClientID %>">Cédula (10 dígitos)</label>
                    <asp:TextBox ID="txtCedula" runat="server" MaxLength="10" ClientIDMode="Static" autocomplete="off" inputmode="numeric" pattern="[0-9]*" />
                    <span class="hint">Solo números.</span>
                </div>
                <div class="ua-field">
                    <label for="<%= txtNick.ClientID %>">Nick</label>
                    <asp:TextBox ID="txtNick" runat="server" MaxLength="50" autocomplete="off" />
                </div>
                <div class="ua-field ua-span2">
                    <label for="<%= txtNombres.ClientID %>">Nombres (mínimo 2 palabras, solo letras)</label>
                    <asp:TextBox ID="txtNombres" runat="server" MaxLength="50" autocomplete="off" />
                </div>
                <div class="ua-field ua-span2">
                    <label for="<%= txtApellidos.ClientID %>">Apellidos (mínimo 2 palabras, solo letras)</label>
                    <asp:TextBox ID="txtApellidos" runat="server" MaxLength="50" autocomplete="off" />
                </div>
                <div class="ua-field">
                    <label for="<%= txtCelular.ClientID %>">Celular</label>
                    <asp:TextBox ID="txtCelular" runat="server" MaxLength="10" ClientIDMode="Static" inputmode="numeric" pattern="[0-9]*" placeholder="0999999999" autocomplete="off" />
                </div>
                <div class="ua-field">
                    <label for="<%= txtFechaNac.ClientID %>">Fecha nacimiento</label>
                    <asp:TextBox ID="txtFechaNac" runat="server" TextMode="Date" />
                </div>
                <div class="ua-field ua-span2">
                    <label for="<%= txtCorreo.ClientID %>">Correo</label>
                    <asp:TextBox ID="txtCorreo" runat="server" TextMode="Email" MaxLength="150" placeholder="correo@dominio.com" autocomplete="off" />
                </div>
                <div class="ua-field ua-span2">
                    <label for="<%= txtDireccion.ClientID %>">Dirección (máx. 50 caracteres)</label>
                    <asp:TextBox ID="txtDireccion" runat="server" MaxLength="50" autocomplete="off" />
                </div>
                <div class="ua-field">
                    <label for="<%= ddlTipo.ClientID %>">Tipo de usuario</label>
                    <asp:DropDownList ID="ddlTipo" runat="server" />
                </div>
                <div class="ua-field">
                    <label for="<%= ddlEstado.ClientID %>">Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server">
                        <asp:ListItem Text="Activo" Value="A" />
                        <asp:ListItem Text="Bloqueado / temporal" Value="T" />
                    </asp:DropDownList>
                </div>
                <div class="ua-field ua-span2">
                    <label for="<%= txtPassword.ClientID %>">Contraseña</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" autocomplete="new-password" />
                    <span class="hint">Obligatoria al crear. Al editar, dejar vacío para no cambiar.</span>
                </div>
            </div>

            <div class="ua-media-block">
                <p class="ua-subh">Fotografías</p>
                <div class="ua-grid-form">
                    <div class="ua-field ua-span2">
                        <label for="<%= fuFoto.ClientID %>">Foto de perfil</label>
                        <asp:FileUpload ID="fuFoto" runat="server" accept="image/jpeg,image/png,.jpg,.jpeg,.png" />
                        <span class="hint">Opcional al crear o editar. JPG o PNG · máx. 2 MB. Pulse «Vista previa foto» en el servidor.</span>
                        <div style="margin-top: 10px; display: flex; flex-wrap: wrap; gap: 10px; align-items: flex-start;">
                            <asp:Button ID="btnPrevPerfil" runat="server" CssClass="ua-btn ua-btn-ghost" Text="Vista previa foto" CausesValidation="false" OnClick="btnPrevPerfil_Click" />
                        </div>
                        <asp:Image ID="imgPreviewPerfil" runat="server" Visible="false" CssClass="ua-preview-img" AlternateText="Vista previa" />
                    </div>
                    <div class="ua-field ua-span2">
                        <label for="<%= fuGaleria.ClientID %>">Galería (varias imágenes)</label>
                        <asp:FileUpload ID="fuGaleria" runat="server" AllowMultiple="true" accept="image/jpeg,image/png,.jpg,.jpeg,.png" />
                        <span class="hint">Hasta 12 archivos por envío · 2 MB c/u · máx. 24 en galería. Pulse «Previsualizar galería» y luego Guardar.</span>
                        <div style="margin-top: 10px;">
                            <asp:Button ID="btnValidarGaleria" runat="server" CssClass="ua-btn ua-btn-ghost" Text="Previsualizar galería" CausesValidation="false" OnClick="btnValidarGaleria_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <asp:Panel ID="pnlGaleriaExistente" runat="server" Visible="false" CssClass="ua-field ua-span2">
                <label>Galería · guardadas y vista previa</label>
                <asp:Repeater ID="rptGaleria" runat="server" EnableViewState="false" OnItemCommand="rptGaleria_ItemCommand">
                    <HeaderTemplate>
                        <div class="ua-galeria-grid">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ua-galeria-card">
                            <img src="<%# Eval("DataUri") %>" alt="" class="ua-galeria-thumb" />
                            <div class="ua-galeria-row" style="margin-top: 8px; border: none; padding: 8px 0; background: transparent; flex-direction: column; align-items: stretch; gap: 6px;">
                                <span class="ua-galeria-meta"><%# Eval("Pie") %></span>
                                <asp:LinkButton runat="server" CssClass="ua-link" Text="Quitar" CommandName="QuitarGal"
                                    CommandArgument='<%# Eval("CmdArg") %>' CausesValidation="false"
                                    OnClientClick="return confirm('¿Quitar esta imagen de la lista?');" />
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate></div></FooterTemplate>
                </asp:Repeater>
            </asp:Panel>

            <div class="ua-form-actions">
                <asp:Button ID="btnGuardar" runat="server" CssClass="ua-btn ua-btn-primary" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return uaValidarAntesDeGuardar();" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="ua-btn ua-btn-ghost" Text="Volver al listado" CausesValidation="false" OnClick="btnCancelar_Click" />
            </div>
        </asp:Panel>
    </div>

    <script>
        function uaContarPalabras(t) {
            if (!t || !t.trim()) return 0;
            return t.trim().split(/\s+/).filter(function (x) { return x.length > 0; }).length;
        }
        function uaEdadDesdeYmd(fnStr) {
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
        function uaNombreSoloLetrasOk(s) {
            var t = (s || '').trim().replace(/\s+/g, ' ');
            if (!t || /\d/.test(t)) return false;
            return /^[\p{L}\s'-]+$/u.test(t);
        }
        function uaFiltrarSoloLetrasNombre(el) {
            if (!el) return;
            el.addEventListener('input', function () {
                var v = el.value.replace(/[^\p{L}\s'-]/gu, '');
                el.value = v.replace(/\s+/g, ' ');
            });
        }
        function uaValidarAntesDeGuardar() {
            var esNuevo = (document.getElementById('hdnUsuarioId').value || '0') === '0';
            var ced = (document.getElementById('txtCedula').value || '').replace(/\D/g, '');
            if (ced.length !== 10) { alert('La cédula debe tener exactamente 10 dígitos.'); return false; }
            var nick = document.getElementById('<%= txtNick.ClientID %>');
            if (!nick || nick.value.trim().length < 4) { alert('El nick debe tener al menos 4 caracteres.'); return false; }
            if (!/^[\p{L}0-9._-]{4,50}$/u.test(nick.value.trim())) { alert('El nick solo puede usar letras, números y los símbolos . _ - (como en el registro).'); return false; }
            if (uaContarPalabras(document.getElementById('<%= txtNombres.ClientID %>').value) < 2) { alert('Ingrese al menos dos nombres.'); return false; }
            if (uaContarPalabras(document.getElementById('<%= txtApellidos.ClientID %>').value) < 2) { alert('Ingrese al menos dos apellidos.'); return false; }
            if (!uaNombreSoloLetrasOk(document.getElementById('<%= txtNombres.ClientID %>').value)) { alert('Los nombres solo pueden contener letras (espacios entre palabras; se permiten \' y - entre letras).'); return false; }
            if (!uaNombreSoloLetrasOk(document.getElementById('<%= txtApellidos.ClientID %>').value)) { alert('Los apellidos solo pueden contener letras (espacios entre palabras; se permiten \' y - entre letras).'); return false; }
            var cel = (document.getElementById('txtCelular').value || '').replace(/\D/g, '');
            if (cel.length < 9 || cel.length > 10) { alert('El celular debe tener 9 o 10 dígitos.'); return false; }
            var mail = document.getElementById('<%= txtCorreo.ClientID %>').value.trim();
            if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(mail)) { alert('Correo electrónico no válido.'); return false; }
            var dir = document.getElementById('<%= txtDireccion.ClientID %>').value.trim();
            if (dir.length < 5 || dir.length > 50) { alert('La dirección debe tener entre 5 y 50 caracteres.'); return false; }
            if (!document.getElementById('<%= txtFechaNac.ClientID %>').value) { alert('Seleccione la fecha de nacimiento.'); return false; }
            var edadUa = uaEdadDesdeYmd(document.getElementById('<%= txtFechaNac.ClientID %>').value);
            if (isNaN(edadUa) || edadUa < 5 || edadUa > 120) { alert('La fecha de nacimiento debe corresponder a una edad entre 5 y 120 años.'); return false; }
            var pass = document.getElementById('<%= txtPassword.ClientID %>').value;
            if (esNuevo && !pass) { alert('La contraseña es obligatoria al crear un usuario.'); return false; }
            if (pass && pass.length < 6) { alert('La contraseña debe tener al menos 6 caracteres.'); return false; }
            var ddlTipo = document.getElementById('<%= ddlTipo.ClientID %>');
            if (!ddlTipo || !ddlTipo.value) { alert('Seleccione el tipo de usuario.'); return false; }
            return true;
        }
        document.addEventListener('DOMContentLoaded', function () {
            uaFiltrarSoloLetrasNombre(document.getElementById('<%= txtNombres.ClientID %>'));
            uaFiltrarSoloLetrasNombre(document.getElementById('<%= txtApellidos.ClientID %>'));
            var c = document.getElementById('txtCedula');
            var cel = document.getElementById('txtCelular');
            function soloDigitos(el, maxLen) {
                if (!el) return;
                el.addEventListener('input', function () {
                    var v = (el.value || '').replace(/\D/g, '');
                    if (maxLen) v = v.substring(0, maxLen);
                    el.value = v;
                });
            }
            soloDigitos(c, 10);
            soloDigitos(cel, 10);
        });
    </script>
</asp:Content>
