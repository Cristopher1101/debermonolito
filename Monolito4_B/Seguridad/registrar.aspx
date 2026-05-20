<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registrar.aspx.cs" Inherits="Monolito4_B.Seguridad.registrar" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Registro — Monolito 4B</title>
    <link href="https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css" rel="stylesheet" />
    <style>
        @import url('https://fonts.googleapis.com/css2?family=DM+Sans:ital,opsz,wght@0,9..40,400;0,9..40,500;0,9..40,600;0,9..40,700;1,9..40,400&display=swap');

        *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

        html { scroll-behavior: smooth; }

        body {
            font-family: 'DM Sans', system-ui, sans-serif;
            min-height: 100vh;
            color: #e8eaef;
            background: #0c1222 url('https://images.unsplash.com/photo-1451187580459-43490279c0fa?q=80&w=2072&auto=format&fit=crop') center/cover no-repeat fixed;
            padding: 24px 16px 48px;
        }

        body::before {
            content: '';
            position: fixed;
            inset: 0;
            background: linear-gradient(160deg, rgba(12,18,34,.88) 0%, rgba(20,32,58,.82) 45%, rgba(8,14,28,.9) 100%);
            pointer-events: none;
            z-index: 0;
        }

        #mainForm {
            position: relative;
            z-index: 1;
            max-width: 720px;
            margin: 0 auto;
            display: flex;
            flex-direction: column;
            gap: 20px;
        }

        .card {
            background: rgba(255,255,255,.08);
            border: 1px solid rgba(255,255,255,.14);
            border-radius: 20px;
            backdrop-filter: blur(18px);
            -webkit-backdrop-filter: blur(18px);
            box-shadow: 0 20px 50px rgba(0,0,0,.35), inset 0 1px 0 rgba(255,255,255,.06);
            padding: 28px 32px;
        }

        .card--compact { padding: 22px 28px; }

        .card-title {
            font-size: 1.05rem;
            font-weight: 600;
            text-align: center;
            margin-bottom: 18px;
            letter-spacing: .02em;
            color: #fff;
        }

        .select-wrap { position: relative; }

        .glass-select {
            width: 100%;
            height: 52px;
            padding: 0 44px 0 16px;
            border-radius: 14px;
            border: 1px solid rgba(255,255,255,.2);
            background: rgba(0,0,0,.2);
            color: #fff;
            font-size: .95rem;
            font-family: inherit;
            appearance: none;
            cursor: pointer;
            transition: border-color .2s, box-shadow .2s;
        }

        .glass-select:focus {
            outline: none;
            border-color: rgba(100,181,246,.7);
            box-shadow: 0 0 0 3px rgba(100,181,246,.2);
        }

        .glass-select option { background: #1a2332; color: #fff; }

        .select-wrap .bx-chevron-down {
            position: absolute;
            right: 14px;
            top: 50%;
            transform: translateY(-50%);
            font-size: 1.35rem;
            color: rgba(255,255,255,.65);
            pointer-events: none;
        }

        .hero-title {
            text-align: center;
            margin-bottom: 8px;
        }

        .hero-title h1 {
            font-size: 1.65rem;
            font-weight: 700;
            color: #fff;
            letter-spacing: -.02em;
        }

        .hero-title p {
            font-size: .9rem;
            color: rgba(255,255,255,.65);
            margin-top: 6px;
        }

        .msg-banner {
            display: block;
            margin: 0 0 22px;
            padding: 14px 16px;
            border-radius: 14px;
            font-size: .88rem;
            line-height: 1.5;
            background: rgba(0,0,0,.35);
            border: 1px solid rgba(255,255,255,.12);
            color: #fff;
        }

        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 18px 20px;
            position: relative;
        }

        @media (max-width: 600px) {
            .form-grid { grid-template-columns: 1fr; }
            .span-2 { grid-column: span 1 !important; }
        }

        .span-2 { grid-column: span 2; }

        .field-label {
            display: block;
            font-size: .78rem;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: .06em;
            color: rgba(255,255,255,.55);
            margin-bottom: 8px;
        }

        .field-input,
        input.field-input {
            width: 100%;
            height: 48px;
            padding: 0 14px 0 14px;
            border-radius: 12px;
            border: 1px solid rgba(255,255,255,.18);
            background: rgba(0,0,0,.22);
            color: #fff;
            font-size: .95rem;
            font-family: inherit;
            transition: border-color .2s, box-shadow .2s;
        }

        .field-input:focus {
            outline: none;
            border-color: rgba(100,181,246,.65);
            box-shadow: 0 0 0 3px rgba(100,181,246,.18);
        }

        .field-input::placeholder { color: rgba(255,255,255,.35); }

        .field-row-icon { position: relative; }

        .field-row-icon .field-input { padding-right: 42px; }

        .field-row-icon .bx {
            position: absolute;
            right: 12px;
            top: 50%;
            transform: translateY(-50%);
            font-size: 1.25rem;
            color: rgba(255,255,255,.45);
            pointer-events: none;
        }

        .field-row-icon .bx.toggle-pass {
            pointer-events: auto;
            cursor: pointer;
            color: rgba(255,255,255,.65);
        }

        .field-row-icon .bx.toggle-pass:hover { color: #fff; }

        /* Foto: bloque propio — destacado al inicio del formulario */
        .photo-card {
            grid-column: span 2;
            margin-top: 0;
            margin-bottom: 4px;
            padding: 22px;
            border-radius: 16px;
            background: rgba(0,0,0,.32);
            border: 1px solid rgba(100,181,246,.22);
            box-shadow: 0 8px 32px rgba(0,0,0,.25);
        }

        @media (max-width: 600px) {
            .photo-card { grid-column: span 1; }
        }

        .photo-layout {
            display: grid;
            grid-template-columns: 140px 1fr;
            gap: 24px;
            align-items: start;
        }

        @media (max-width: 600px) {
            .photo-layout { grid-template-columns: 1fr; justify-items: center; text-align: center; }
        }

        .photo-preview-wrap {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 10px;
        }

        .photo-preview-wrap img {
            width: 128px;
            height: 128px;
            border-radius: 50%;
            object-fit: cover;
            border: 3px solid rgba(100,181,246,.45);
            box-shadow: 0 8px 28px rgba(0,0,0,.4);
            background: rgba(0,0,0,.3);
        }

        .photo-preview-caption {
            font-size: .72rem;
            color: rgba(255,255,255,.5);
            max-width: 140px;
            text-align: center;
            line-height: 1.35;
        }

        .upload-stack { width: 100%; min-width: 0; }

        .upload-block { margin-bottom: 18px; }

        .upload-block:last-of-type { margin-bottom: 14px; }

        .upload-hint {
            font-size: .72rem;
            color: rgba(255,255,255,.45);
            margin-top: 6px;
            line-height: 1.4;
        }

        /* FileUpload nativo: envolver apariencia */
        .file-input-shell {
            position: relative;
            display: block;
            margin-top: 8px;
            border-radius: 12px;
            border: 1px dashed rgba(255,255,255,.28);
            background: rgba(255,255,255,.04);
            padding: 10px 12px;
            transition: border-color .2s, background .2s;
        }

        .file-input-shell:hover {
            border-color: rgba(100,181,246,.5);
            background: rgba(100,181,246,.08);
        }

        .file-input-shell input[type=file] {
            width: 100%;
            font-size: .82rem;
            color: rgba(255,255,255,.85);
            cursor: pointer;
        }

        .file-input-shell input[type=file]::file-selector-button {
            margin-right: 12px;
            padding: 8px 14px;
            border-radius: 10px;
            border: none;
            background: rgba(255,255,255,.18);
            color: #fff;
            font-family: inherit;
            font-size: .8rem;
            font-weight: 600;
            cursor: pointer;
        }

        .file-input-shell input[type=file]::file-selector-button:hover {
            background: rgba(100,181,246,.45);
        }

        .actions {
            display: flex;
            flex-direction: column;
            gap: 12px;
            margin-top: 8px;
        }

        .btn-row {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            justify-content: center;
        }

        input.btn-secondary,
        .btn-secondary {
            padding: 12px 22px;
            border-radius: 12px;
            border: 1px solid rgba(255,255,255,.35);
            background: rgba(255,255,255,.1);
            color: #fff;
            font-family: inherit;
            font-size: .9rem;
            font-weight: 600;
            cursor: pointer;
            transition: background .2s, transform .15s;
        }

        input.btn-secondary:hover,
        .btn-secondary:hover {
            background: rgba(255,255,255,.2);
            transform: translateY(-1px);
        }

        .btn-submit-asp {
            width: 100%;
            padding: 14px 24px;
            border-radius: 14px;
            border: none;
            background: linear-gradient(135deg, #5c9ded 0%, #4a7fd4 50%, #3d6bc4 100%);
            color: #fff;
            font-family: inherit;
            font-size: 1rem;
            font-weight: 700;
            letter-spacing: .03em;
            cursor: pointer;
            box-shadow: 0 10px 30px rgba(74,127,212,.35);
            transition: transform .15s, box-shadow .2s;
        }

        .btn-submit-asp:hover {
            transform: translateY(-2px);
            box-shadow: 0 14px 36px rgba(74,127,212,.45);
        }

        .footer-link {
            text-align: center;
            margin-top: 22px;
            font-size: .9rem;
            color: rgba(255,255,255,.65);
        }

        .footer-link a {
            color: #9ecbff;
            font-weight: 600;
            text-decoration: none;
        }

        .gallery-count {
            font-size: .85rem;
            color: rgba(129,199,132,.95);
            margin-top: 8px;
            padding: 10px 12px;
            border-radius: 10px;
            background: rgba(0,0,0,.25);
            border: 1px solid rgba(129,199,132,.25);
        }

        .photo-card--top {
            margin-top: 0;
            order: -1;
        }

        .gal-strip {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
            margin-top: 10px;
            min-height: 0;
        }

        .gal-strip img {
            width: 56px;
            height: 56px;
            object-fit: cover;
            border-radius: 10px;
            border: 2px solid rgba(100,181,246,.4);
            background: rgba(0,0,0,.35);
        }

        .gal-previa-servidor {
            margin-top: 12px;
        }

        .gal-previa-servidor .gal-strip {
            margin-top: 8px;
            align-items: flex-start;
        }

        .field-input.field-invalid {
            border-color: rgba(255,138,128,.85) !important;
            box-shadow: 0 0 0 3px rgba(255,112,67,.2);
        }
    </style>
</head>
<body>
    <form id="mainForm" runat="server" autocomplete="off">
        <div class="card card--compact">
            <div class="card-title">Tipo de cuenta</div>
            <p style="text-align:center;font-size:.82rem;color:rgba(255,255,255,.7);margin-bottom:14px;line-height:1.45;">El registro público es solo como <strong>usuario</strong>. Los administradores los crea otro administrador desde el panel.</p>
            <div class="select-wrap">
                <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="glass-select" ClientIDMode="Static">
                </asp:DropDownList>
                <i class="bx bx-chevron-down"></i>
            </div>
        </div>

        <div class="card" id="panelRegister">
            <div class="hero-title">
                <h1>Crear cuenta</h1>
                <p>Datos personales: una foto de perfil y, si desea, varias imágenes adicionales en galería.</p>
            </div>

            <asp:Label ID="lblMensaje" runat="server" CssClass="msg-banner" Visible="false" EnableViewState="false" />

            <div class="form-grid">
                <div class="photo-card span-2 photo-card--top">
                    <span class="field-label" style="margin-bottom:14px;">Fotografías</span>
                    <div class="photo-layout">
                        <div class="photo-preview-wrap">
                            <asp:Image ID="imgPreview" runat="server" ClientIDMode="Static"
                                ImageUrl="https://placehold.co/128x128/1a2332/8899aa/png?text=Foto"
                                AlternateText="Vista previa" />
                            <span class="photo-preview-caption">Vista previa de perfil (servidor — pulse «Previsualizar fotos»)</span>
                        </div>
                        <div class="upload-stack">
                            <div class="upload-block">
                                <label class="field-label" for="fuFotoPerfil" style="text-transform:none;letter-spacing:0;color:rgba(255,255,255,.75);">Foto principal (obligatoria)</label>
                                <div class="file-input-shell">
                                    <asp:FileUpload ID="fuFotoPerfil" runat="server" ClientIDMode="Static" accept="image/jpeg,image/png,.jpg,.jpeg,.png" />
                                </div>
                                <p class="upload-hint">Solo JPG o PNG, máx. 2 MB. La validación y la miniatura se hacen en el servidor al pulsar «Previsualizar fotos».</p>
                            </div>
                            <div class="upload-block">
                                <label class="field-label" for="fuGaleria" style="text-transform:none;letter-spacing:0;color:rgba(255,255,255,.75);">Galería (opcional)</label>
                                <div class="file-input-shell">
                                    <asp:FileUpload ID="fuGaleria" runat="server" ClientIDMode="Static" AllowMultiple="true" accept="image/jpeg,image/png,.jpg,.jpeg,.png" />
                                </div>
                                <p class="upload-hint">Hasta 12 archivos. Las miniaturas aparecen aquí abajo solo tras «Previsualizar fotos» (servidor).</p>
                                <asp:Panel ID="pnlGaleriaPrevia" runat="server" Visible="false" CssClass="gal-previa-servidor">
                                    <span class="field-label" style="text-transform:none;letter-spacing:0;color:rgba(129,199,132,.95);margin-top:4px;">Vista previa galería (servidor)</span>
                                    <div class="gal-strip">
                                        <asp:Repeater ID="rptGaleriaPrev" runat="server" OnItemCommand="rptGaleriaPrev_ItemCommand">
                                            <ItemTemplate>
                                                <div style="display:flex;flex-direction:column;align-items:center;gap:4px;">
                                                    <img src="<%# Eval("DataUri") %>" alt="" style="width:56px;height:56px;object-fit:cover;border-radius:10px;border:2px solid rgba(100,181,246,.4);" />
                                                    <asp:LinkButton runat="server" Text="Quitar" CommandName="QuitarGalPrev" CommandArgument='<%# Eval("CmdArg") %>'
                                                        CausesValidation="false" style="color:#ffcdd2;font-size:.72rem;font-weight:600;background:none;border:none;cursor:pointer;text-decoration:underline;"
                                                        OnClientClick="return confirm('¿Quitar esta imagen de la lista?');" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </asp:Panel>
                                <asp:Label ID="lblGaleriaResumen" runat="server" CssClass="gallery-count" Visible="false" EnableViewState="false" />
                            </div>
                            <div class="btn-row">
                                <asp:Button ID="btnPrevisualizar" runat="server" Text="Previsualizar fotos" CssClass="btn-secondary"
                                    OnClick="btnPrevisualizar_Click" CausesValidation="false" UseSubmitBehavior="true" />
                            </div>
                        </div>
                    </div>
                </div>
                <div>
                    <label class="field-label" for="regCedula">Cédula (10 dígitos, solo números)</label>
                    <div class="field-row-icon">
                        <asp:TextBox ID="regCedula" runat="server" ClientIDMode="Static" CssClass="field-input" MaxLength="10" inputmode="numeric" pattern="[0-9]*" placeholder="1723456789" />
                        <i class="bx bx-id-card"></i>
                    </div>
                </div>
                <div>
                    <label class="field-label" for="regNick">Usuario (nick)</label>
                    <div class="field-row-icon">
                        <asp:TextBox ID="regNick" runat="server" ClientIDMode="Static" CssClass="field-input" MaxLength="50" placeholder="Letras y números, mín. 4 (se puede autogenerar)" />
                        <i class="bx bx-user"></i>
                    </div>
                </div>

                <div>
                    <label class="field-label" for="regNombres">Nombres (mínimo dos)</label>
                    <asp:TextBox ID="regNombres" runat="server" ClientIDMode="Static" CssClass="field-input" MaxLength="50" placeholder="Ej.: Juan Carlos (solo letras)" />
                </div>
                <div>
                    <label class="field-label" for="regApellidos">Apellidos (mínimo dos)</label>
                    <asp:TextBox ID="regApellidos" runat="server" ClientIDMode="Static" CssClass="field-input" MaxLength="50" placeholder="Ej.: Pérez López — al terminar autogenera correo y nick" />
                </div>

                <div>
                    <label class="field-label" for="regCelular">Celular (solo números, 9 o 10 dígitos)</label>
                    <div class="field-row-icon">
                        <asp:TextBox ID="regCelular" runat="server" ClientIDMode="Static" CssClass="field-input" MaxLength="10" inputmode="numeric" pattern="[0-9]*" placeholder="0999999999" />
                        <i class="bx bx-phone"></i>
                    </div>
                </div>
                <div>
                    <label class="field-label" for="regFechaCumple">Fecha nacimiento</label>
                    <asp:TextBox ID="regFechaCumple" runat="server" TextMode="Date" ClientIDMode="Static" CssClass="field-input" />
                </div>

                <div class="span-2">
                    <label class="field-label" for="regCorreo">Correo</label>
                    <div class="field-row-icon">
                        <asp:TextBox ID="regCorreo" runat="server" TextMode="Email" ClientIDMode="Static" CssClass="field-input" MaxLength="150" placeholder="correo@ejemplo.com" />
                        <i class="bx bx-envelope"></i>
                    </div>
                </div>

                <div class="span-2">
                    <label class="field-label" for="regDireccion">Dirección</label>
                    <div class="field-row-icon">
                        <asp:TextBox ID="regDireccion" runat="server" ClientIDMode="Static" CssClass="field-input" MaxLength="50" placeholder="Ciudad, calle, número… (5–50 caracteres)" />
                        <i class="bx bx-map"></i>
                    </div>
                </div>

                <div class="span-2">
                    <label class="field-label" for="regPassword">Contraseña</label>
                    <div class="field-row-icon">
                        <asp:TextBox ID="regPassword" runat="server" TextMode="Password" ClientIDMode="Static" CssClass="field-input" MaxLength="80" placeholder="Mínimo 6 caracteres" />
                        <i class="bx bx-hide toggle-pass" data-target="regPassword"></i>
                    </div>
                </div>

            </div>

            <div class="actions">
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrarse" CssClass="btn-submit-asp"
                    OnClick="btnRegistrar_Click" OnClientClick="return regValidarAntesDeRegistrar();" CausesValidation="false" />
            </div>

            <div class="footer-link">
                ¿Ya tienes cuenta? <a href="<%= ResolveUrl("~/Seguridad/Login.aspx") %>">Iniciar sesión</a>
            </div>
        </div>
    </form>

    <script>
        function soloDigitos(el, maxLen) {
            if (!el) return;
            el.addEventListener('input', function () {
                var v = (el.value || '').replace(/\D/g, '');
                if (maxLen) v = v.substring(0, maxLen);
                el.value = v;
            });
            el.addEventListener('paste', function (e) {
                e.preventDefault();
                var t = (e.clipboardData || window.clipboardData).getData('text') || '';
                el.value = t.replace(/\D/g, '').substring(0, maxLen || t.length);
            });
        }

        function soloLetrasNombre(el) {
            if (!el) return;
            el.addEventListener('input', function () {
                var v = (el.value || '').replace(/[^\p{L}\s'.-]/gu, '');
                v = v.replace(/\s{2,}/g, ' ');
                el.value = v;
            });
        }

        function soloNickAlfanumerico(el) {
            if (!el) return;
            el.addEventListener('input', function () {
                el.value = (el.value || '').replace(/[^\p{L}0-9._-]/gu, '');
            });
        }

        function soloDireccionPermitida(el) {
            if (!el) return;
            el.addEventListener('input', function () {
                el.value = (el.value || '').replace(/[^A-Za-z0-9áéíóúÁÉÍÓÚñÑüÜ\s#.,°\-ªº/]/g, '');
            });
        }

        function contarPalabras(t) {
            if (!t || !t.trim()) return 0;
            return t.trim().split(/\s+/).filter(function (x) { return x.length > 0; }).length;
        }

        function regMarcarInvalido(id, invalido) {
            var el = document.getElementById(id);
            if (!el) return;
            if (invalido) el.classList.add('field-invalid');
            else el.classList.remove('field-invalid');
        }

        function regRandomPassword() {
            var upper = 'ABCDEFGHJKLMNPQRSTUVWXYZ';
            var lower = 'abcdefghjkmnpqrstuvwxyz';
            var num = '23456789';
            var spec = '!@#$%';
            var all = upper + lower + num + spec;
            var pwd = '';
            pwd += upper.charAt(Math.floor(Math.random() * upper.length));
            pwd += lower.charAt(Math.floor(Math.random() * lower.length));
            pwd += num.charAt(Math.floor(Math.random() * num.length));
            pwd += spec.charAt(Math.floor(Math.random() * spec.length));
            for (var i = pwd.length; i < 10; i++)
                pwd += all.charAt(Math.floor(Math.random() * all.length));
            var a = pwd.split('');
            for (var j = a.length - 1; j > 0; j--) {
                var k = Math.floor(Math.random() * (j + 1));
                var tmp = a[j]; a[j] = a[k]; a[k] = tmp;
            }
            return a.join('');
        }

        function regAutoGenerarDesdeNombres() {
            var nomEl = document.getElementById('regNombres');
            var apeEl = document.getElementById('regApellidos');
            var cedEl = document.getElementById('regCedula');
            var mailEl = document.getElementById('regCorreo');
            var nickEl = document.getElementById('regNick');
            var passEl = document.getElementById('regPassword');
            if (!nomEl || !apeEl || !mailEl || !nickEl || !passEl) return;
            var nom = nomEl.value.trim().split(/\s+/).filter(Boolean);
            var ape = apeEl.value.trim().split(/\s+/).filter(Boolean);
            if (nom.length === 0 || ape.length === 0) return;
            function toAsciiLocal(s) {
                var t = (s || '').normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase().replace(/[^a-z0-9]/g, '');
                return t || 'usuario';
            }
            mailEl.value = toAsciiLocal(nom[0]) + '.' + toAsciiLocal(ape[0]) + '@cordillera.edu.ec';
            passEl.value = regRandomPassword();
            var iniNom = nom[0].charAt(0).toUpperCase();
            var iniNom2 = nom.length > 1 ? nom[1].charAt(0).toLowerCase() : nom[0].charAt(0).toLowerCase();
            var iniApe = ape[0].charAt(0).toUpperCase();
            var iniApe2 = ape.length > 1 ? ape[1].charAt(0).toLowerCase() : ape[0].charAt(0).toLowerCase();
            var ced = (cedEl && cedEl.value) ? cedEl.value.replace(/\D/g, '') : '';
            var extra = ced.length ? ced.charAt(Math.floor(Math.random() * ced.length)) : '0';
            nickEl.value = iniNom + iniNom2 + iniApe + iniApe2 + (100 + Math.floor(Math.random() * 900)) + extra;
        }

        function regValidarAntesDeRegistrar() {
            ['regCedula', 'regNick', 'regNombres', 'regApellidos', 'regCelular', 'regFechaCumple', 'regCorreo', 'regDireccion', 'regPassword']
                .forEach(function (id) { regMarcarInvalido(id, false); });

            var ced = document.getElementById('regCedula');
            var nick = document.getElementById('regNick');
            var nom = document.getElementById('regNombres');
            var ape = document.getElementById('regApellidos');
            var cel = document.getElementById('regCelular');
            var fn = document.getElementById('regFechaCumple');
            var mail = document.getElementById('regCorreo');
            var dir = document.getElementById('regDireccion');
            var pass = document.getElementById('regPassword');

            if (!ced || ced.value.replace(/\D/g, '').length !== 10) {
                alert('La cédula debe tener exactamente 10 dígitos (solo números).');
                regMarcarInvalido('regCedula', true);
                return false;
            }
            if (!nick || nick.value.length < 4 || !/^[\p{L}0-9._-]{4,50}$/u.test(nick.value)) {
                alert('El usuario (nick): mínimo 4 caracteres; solo letras, números y . _ -');
                regMarcarInvalido('regNick', true);
                return false;
            }
            if (!nom || contarPalabras(nom.value) < 2 || /[0-9]/.test(nom.value)) {
                alert('Nombres: al menos dos palabras, solo letras (sin números).');
                regMarcarInvalido('regNombres', true);
                return false;
            }
            if (!ape || contarPalabras(ape.value) < 2 || /[0-9]/.test(ape.value)) {
                alert('Apellidos: al menos dos palabras, solo letras (sin números).');
                regMarcarInvalido('regApellidos', true);
                return false;
            }
            var cd = (cel && cel.value) ? cel.value.replace(/\D/g, '') : '';
            if (cd.length < 9 || cd.length > 10) {
                alert('El celular debe tener 9 o 10 dígitos (solo números).');
                regMarcarInvalido('regCelular', true);
                return false;
            }
            if (!fn || !fn.value) {
                alert('Seleccione la fecha de nacimiento.');
                regMarcarInvalido('regFechaCumple', true);
                return false;
            }
            var dFn = new Date(fn.value + 'T12:00:00');
            var hoy = new Date();
            hoy.setHours(0, 0, 0, 0);
            if (dFn > hoy) {
                alert('La fecha de nacimiento no puede ser futura.');
                regMarcarInvalido('regFechaCumple', true);
                return false;
            }
            if (!mail || !/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(mail.value.trim())) {
                alert('Ingrese un correo electrónico válido.');
                regMarcarInvalido('regCorreo', true);
                return false;
            }
            var d = (dir && dir.value) ? dir.value.trim() : '';
            if (d.length < 5 || d.length > 50) {
                alert('La dirección debe tener entre 5 y 50 caracteres.');
                regMarcarInvalido('regDireccion', true);
                return false;
            }
            if (!/^[A-Za-z0-9áéíóúÁÉÍÓÚñÑüÜ#.,\s°\-ªº/]+$/.test(d)) {
                alert('La dirección solo puede incluir letras, números, espacio y # . , - / ° ª º');
                regMarcarInvalido('regDireccion', true);
                return false;
            }
            if (!pass || pass.value.length < 6) {
                alert('La contraseña debe tener al menos 6 caracteres.');
                regMarcarInvalido('regPassword', true);
                return false;
            }
            return true;
        }

        document.addEventListener('DOMContentLoaded', function () {
            soloDigitos(document.getElementById('regCedula'), 10);
            soloDigitos(document.getElementById('regCelular'), 10);
            soloLetrasNombre(document.getElementById('regNombres'));
            soloLetrasNombre(document.getElementById('regApellidos'));
            soloNickAlfanumerico(document.getElementById('regNick'));
            soloDireccionPermitida(document.getElementById('regDireccion'));

            var apeBlur = document.getElementById('regApellidos');
            if (apeBlur) {
                apeBlur.addEventListener('blur', function () {
                    if (contarPalabras(document.getElementById('regNombres').value) >= 2 && contarPalabras(apeBlur.value) >= 2)
                        regAutoGenerarDesdeNombres();
                });
            }

            function regExtensionFotoOk(nombre) {
                if (!nombre) return false;
                return /\.(jpe?g|png)$/i.test(nombre);
            }
            function regValidarSoloFotos(input, multiple) {
                if (!input || !input.files || !input.files.length) return;
                var n = multiple ? input.files.length : 1;
                for (var j = 0; j < n; j++) {
                    var f = input.files[j];
                    if (!regExtensionFotoOk(f.name)) {
                        alert('Solo se permiten archivos .jpg, .jpeg o .png. Revise: «' + (f.name || '') + '».');
                        input.value = '';
                        return;
                    }
                }
            }
            var fuPerf = document.getElementById('fuFotoPerfil');
            if (fuPerf) fuPerf.addEventListener('change', function () { regValidarSoloFotos(fuPerf, false); });
            var fuGalEl = document.getElementById('fuGaleria');
            if (fuGalEl) fuGalEl.addEventListener('change', function () { regValidarSoloFotos(fuGalEl, true); });

            document.querySelectorAll('.toggle-pass').forEach(function (icon) {
                icon.addEventListener('click', function () {
                    var id = this.getAttribute('data-target');
                    var input = document.getElementById(id);
                    if (!input) return;
                    if (input.type === 'password') {
                        input.type = 'text';
                        this.classList.remove('bx-hide');
                        this.classList.add('bx-show');
                    } else {
                        input.type = 'password';
                        this.classList.remove('bx-show');
                        this.classList.add('bx-hide');
                    }
                });
            });
        });
    </script>
    <script src="<%= ResolveUrl("~/Scripts/disable-form-autofill.js") %>" defer></script>
</body>
</html>
