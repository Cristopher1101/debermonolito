<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Monolito4_B.Seguridad.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Monolito 4B — Acceso seguro</title>

    <link href='https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css' rel='stylesheet'>

    <style>
        @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap');

        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Poppins', sans-serif;
        }

        body {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            background: url('https://images.unsplash.com/photo-1550684848-fac1c5b4e853?q=80&w=2070&auto=format&fit=crop') no-repeat center;
            background-size: cover;
        }

        .login-wrapper {
            position: relative;
            width: 460px;
            max-width: 96vw;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .login-glass {
            width: 100%;
            padding: 40px;
            background: rgba(255, 255, 255, 0.1);
            border: 2px solid rgba(255, 255, 255, 0.2);
            backdrop-filter: blur(15px);
            -webkit-backdrop-filter: blur(15px);
            border-radius: 20px;
            color: #fff;
            box-shadow: 0 15px 25px rgba(0, 0, 0, 0.3);
            transform: translateY(30px);
            opacity: 0;
            animation: slideUp 0.8s cubic-bezier(0.25, 1, 0.5, 1) forwards;
            max-height: 92vh;
            overflow-y: auto;
            overflow-x: hidden;
        }

        @keyframes slideUp {
            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        .login-header {
            text-align: center;
            margin-bottom: 30px;
        }

            .login-header h2 {
                font-size: 2em;
                font-weight: 600;
            }

            .login-header p {
                font-size: 0.88em;
                font-weight: 300;
                margin-top: 8px;
                color: rgba(255,255,255,0.88);
                line-height: 1.45;
            }

        .input-box {
            position: relative;
            width: 100%;
            height: 50px;
            margin-bottom: 25px;
        }

            .input-box input {
                width: 100%;
                height: 100%;
                background: transparent;
                border: none;
                outline: none;
                border-bottom: 2px solid rgba(255, 255, 255, 0.5);
                color: #fff;
                font-size: 1em;
                padding: 0 35px 0 5px;
                transition: 0.3s;
            }

                .input-box input:focus,
                .input-box input:valid {
                    border-bottom: 2px solid #fff;
                }

            .input-box label {
                position: absolute;
                top: 50%;
                left: 5px;
                transform: translateY(-50%);
                font-size: 1em;
                color: rgba(255, 255, 255, 0.8);
                font-weight: 400;
                pointer-events: none;
                transition: 0.3s;
            }

            .input-box input:focus ~ label,
            .input-box input:valid ~ label {
                top: -5px;
                font-size: 0.85em;
                color: #fff;
            }

            .input-box .icon {
                position: absolute;
                right: 8px;
                top: 50%;
                transform: translateY(-50%);
                font-size: 1.3em;
                color: #fff;
                cursor: pointer;
                transition: 0.2s;
            }

                .input-box .icon:hover {
                    color: #ccc;
                }

        .otp-input-box input {
            text-align: center;
            font-size: 1.5em;
            letter-spacing: 8px;
            font-weight: 600;
        }

        .remember-forgot {
            display: flex;
            justify-content: space-between;
            align-items: center;
            font-size: 0.85em;
            margin: -10px 0 22px;
        }

            .remember-forgot label {
                display: flex;
                align-items: center;
                cursor: pointer;
                gap: 5px;
            }

                .remember-forgot label input {
                    accent-color: #fff;
                }

            .remember-forgot a {
                color: rgba(255,255,255,0.8);
                text-decoration: none;
                transition: color 0.2s;
            }

                .remember-forgot a:hover {
                    color: #fff;
                    text-decoration: underline;
                }

        button.btn-submit {
            width: 100%;
            height: 45px;
            background: #fff;
            border: none;
            outline: none;
            border-radius: 40px;
            cursor: pointer;
            font-size: 1em;
            color: #333;
            font-weight: 600;
            transition: all 0.3s ease;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        input[type="submit"].btn-submit {
            width: 100%;
            height: 45px;
            background: #fff;
            border: none;
            outline: none;
            border-radius: 40px;
            cursor: pointer;
            font-size: 1em;
            color: #333;
            font-weight: 600;
            transition: all 0.3s ease;
            display: block;
            text-align: center;
            line-height: 45px;
            padding: 0 16px;
        }

            button.btn-submit:hover,
            input[type="submit"].btn-submit:hover {
                background: rgba(255,255,255,0.88);
                box-shadow: 0 0 18px rgba(255,255,255,0.25);
            }

        .error-shake {
            animation: shake 0.4s;
            border: 2px solid rgba(255, 77, 77, 0.5);
        }

        @keyframes shake {
            0%, 100% { transform: translateX(0); }
            25%, 75% { transform: translateX(-8px); }
            50% { transform: translateX(8px); }
        }

        .footer-link {
            text-align: center;
            margin-top: 22px;
            font-size: 0.88em;
            color: rgba(255,255,255,0.75);
        }

            .footer-link a {
                color: #fff;
                font-weight: 600;
                text-decoration: underline;
                cursor: pointer;
            }

        .otp-icon-top {
            font-size: 4em;
            color: #fff;
            text-align: center;
            display: block;
            margin-bottom: 10px;
        }

        .field-hint {
            font-size: 0.78em;
            color: rgba(255,255,255,0.72);
            margin: -18px 0 20px 5px;
            line-height: 1.35;
        }

        button.btn-qr-scan {
            width: 100%;
            max-width: 320px;
            margin: 0 auto 10px;
            display: block;
            padding: 11px 16px;
            border-radius: 12px;
            border: 1px solid rgba(255,255,255,0.45);
            background: rgba(255,255,255,0.14);
            color: #fff;
            font-weight: 600;
            font-size: 0.88rem;
            cursor: pointer;
            transition: background 0.2s, transform 0.15s;
        }
        button.btn-qr-scan:hover { background: rgba(255,255,255,0.22); transform: translateY(-1px); }
        button.btn-qr-scan:disabled { opacity: 0.55; cursor: wait; }
        .qr-reader-wrap {
            display: none;
            margin: 14px auto 8px;
            max-width: 320px;
            border-radius: 14px;
            overflow: hidden;
            border: 2px solid rgba(255,255,255,0.25);
            background: rgba(0,0,0,0.25);
        }
        button.btn-qr-stop {
            width: 100%;
            max-width: 320px;
            margin: 0 auto 8px;
            display: block;
            padding: 8px 12px;
            border-radius: 10px;
            border: 1px solid rgba(255,180,180,0.5);
            background: rgba(255,100,100,0.15);
            color: #fecaca;
            font-size: 0.82rem;
            cursor: pointer;
        }
    </style>
</head>
<body>

    <form id="loginForm" runat="server" autocomplete="off">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <div class="login-wrapper">
            <div class="login-glass" id="glassContainer">

                <asp:Panel ID="pnlLogin" runat="server">
                    <div class="login-header">
                        <h2>Monolito 4B</h2>
                        <p>Ingrese su <strong>cédula</strong> y <strong>contraseña</strong>. Recibirá el mismo <strong>código OTP de 6 dígitos por correo y por WhatsApp</strong> a la vez.</p>
                    </div>

                    <div class="input-box">
                        <asp:TextBox ID="txt_ced" runat="server" required="true" ClientIDMode="Static" MaxLength="10" inputmode="numeric" pattern="[0-9]*" autocomplete="off"></asp:TextBox>
                        <label for="txt_ced">Cédula</label>
                        <i class='bx bx-id-card icon'></i>
                    </div>
                    <p class="field-hint">Solo números, exactamente 10 dígitos.</p>

                    <div class="input-box">
                        <asp:TextBox ID="txt_pass" runat="server" TextMode="Password" required="true" ClientIDMode="Static" autocomplete="off"></asp:TextBox>
                        <label for="txt_pass">Contraseña</label>
                        <i class='bx bx-hide icon' id="togglePassword" title="Mostrar/Ocultar"></i>
                    </div>

                    <div class="remember-forgot">
                        <label>
                            <asp:CheckBox ID="chkRemember" runat="server" />
                            Recordarme
                        </label>
                        <a href="Recuperar.aspx">¿Olvidaste tu contraseña?</a>
                    </div>
                    <p class="field-hint" style="margin:-8px 0 18px 5px;">Si marca <strong>Recordarme</strong>, tras validar el OTP la sesión del servidor durará más. No se guarda la cédula ni la contraseña en el navegador.</p>

                    <asp:Button ID="btnLogin" runat="server" CssClass="btn-submit" Text="Iniciar sesión"
                        OnClick="btnLogin_Click" UseSubmitBehavior="true" ClientIDMode="Static"
                        OnClientClick="return validarLoginCliente();" />

                    <div class="footer-link">
                        ¿No tienes cuenta? <a href="<%= ResolveUrl("~/Seguridad/registrar.aspx") %>">Regístrate aquí</a>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlOTP" runat="server" Visible="false">
                    <i class='bx bx-shield-quarter otp-icon-top'></i>

                    <div class="login-header">
                        <h2>Verificación OTP</h2>
                        <p><asp:Literal ID="litSubtituloOtp" runat="server" Mode="PassThrough" Text="Ingrese el código de 6 dígitos enviado a su correo y a su WhatsApp."></asp:Literal></p>
                    </div>

                    <div class="input-box otp-input-box">
                        <asp:TextBox ID="txt_otp" runat="server" required="true" MaxLength="6" autocomplete="off" ClientIDMode="Static" inputmode="numeric" pattern="[0-9]*"></asp:TextBox>
                        <label for="txt_otp">Código OTP</label>
                    </div>
                    <p class="field-hint">Solo números, exactamente 6 dígitos (el mismo código en correo y WhatsApp).</p>

                    <p class="field-hint" style="margin-top:4px;">Si abrió el correo en <strong>otro dispositivo</strong>, puede <strong>escanear el QR</strong> con la cámara de este equipo para entrar sin teclear el OTP.</p>
                    <button type="button" id="btnScanQrOtp" class="btn-qr-scan"><i class='bx bx-qr-scan' style="vertical-align:middle;margin-right:6px;"></i>Escanear QR del correo (cámara)</button>
                    <div id="wrapQrReaderOtp" class="qr-reader-wrap"><div id="qrReaderOtpHost"></div></div>
                    <button type="button" id="btnStopQrOtp" class="btn-qr-stop" style="display:none;">Detener cámara</button>

                    <asp:Button ID="btnVerifyOTP" runat="server" CssClass="btn-submit" Text="Confirmar e ingresar"
                        OnClick="btnVerifyOTP_Click" UseSubmitBehavior="true" ClientIDMode="Static"
                        OnClientClick="return validarOtpCliente();" />

                    <div class="footer-link">
                        ¿No llegó el mensaje?
                        <asp:LinkButton ID="lnkResend" runat="server" OnClick="lnkResend_Click">Reenviar código (correo y WhatsApp)</asp:LinkButton>
                    </div>
                    <div class="footer-link" style="margin-top: 10px;">
                        <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" Style="color: rgba(255,255,255,0.6); text-decoration: none; font-size: 0.9em;"><i class='bx bx-arrow-back'></i> Volver al login</asp:LinkButton>
                    </div>
                </asp:Panel>

            </div>
        </div>
    </form>

    <script>
        function soloDigitos(el, maxLen) {
            if (!el) return;
            el.addEventListener("input", function () {
                var v = (el.value || "").replace(/\D/g, "");
                if (maxLen) v = v.substring(0, maxLen);
                el.value = v;
            });
            el.addEventListener("paste", function (e) {
                e.preventDefault();
                var t = (e.clipboardData || window.clipboardData).getData("text") || "";
                el.value = t.replace(/\D/g, "").substring(0, maxLen || t.length);
            });
        }

        function validarLoginCliente() {
            var glass = document.getElementById("glassContainer");
            var cedEl = document.getElementById("txt_ced");
            var passEl = document.getElementById("txt_pass");
            if (!cedEl || !passEl) return true;
            var ced = (cedEl.value || "").replace(/\D/g, "");
            var pass = (passEl.value || "").trim();
            if (ced.length !== 10 || !pass) {
                if (glass) {
                    glass.classList.add("error-shake");
                    setTimeout(function () { glass.classList.remove("error-shake"); }, 400);
                }
                if (ced.length !== 10) alert("La cédula debe tener exactamente 10 dígitos (solo números).");
                else alert("Ingrese su contraseña.");
                return false;
            }
            return true;
        }

        function validarOtpCliente() {
            var glass = document.getElementById("glassContainer");
            var otpEl = document.getElementById("txt_otp");
            if (!otpEl) return true;
            var otp = (otpEl.value || "").replace(/\D/g, "");
            if (otp.length !== 6) {
                if (glass) {
                    glass.classList.add("error-shake");
                    setTimeout(function () { glass.classList.remove("error-shake"); }, 400);
                }
                alert("El OTP debe ser exactamente 6 dígitos (solo números).");
                return false;
            }
            return true;
        }

        (function () {
            var html5QrOtp = null;
            var otpQrLibLoading = false;
            var lastQrText = "";

            function loadHtml5QrcodeLib(cb) {
                if (window.Html5Qrcode) { cb(); return; }
                if (otpQrLibLoading) {
                    var t0 = Date.now();
                    var iv = setInterval(function () {
                        if (window.Html5Qrcode) { clearInterval(iv); cb(); }
                        else if (Date.now() - t0 > 15000) {
                            clearInterval(iv);
                            var bx = document.getElementById("btnScanQrOtp");
                            if (bx) bx.disabled = false;
                            alert("No se pudo cargar el lector QR.");
                        }
                    }, 80);
                    return;
                }
                otpQrLibLoading = true;
                var s = document.createElement("script");
                s.src = "https://unpkg.com/html5-qrcode@2.3.8/html5-qrcode.min.js";
                s.async = true;
                s.onload = function () { otpQrLibLoading = false; cb(); };
                s.onerror = function () {
                    otpQrLibLoading = false;
                    var bs = document.getElementById("btnScanQrOtp");
                    if (bs) bs.disabled = false;
                    alert("No se pudo descargar el lector QR. Compruebe la conexión o use el código de 6 dígitos.");
                };
                document.head.appendChild(s);
            }

            function urlOtpQrSegura(texto) {
                var raw = (texto || "").trim();
                if (!raw) return null;
                var u;
                try {
                    u = /^https?:\/\//i.test(raw) ? new URL(raw) : new URL(raw, window.location.origin);
                } catch (e) { return null; }
                if (u.origin !== window.location.origin) return null;
                var p = (u.pathname || "").replace(/\\/g, "/").toLowerCase();
                if (p.indexOf("otpqr.aspx") === -1) return null;
                var t = u.searchParams.get("t");
                if (!t || !String(t).trim()) return null;
                return u.href;
            }

            function detenerCamaraQr() {
                var wrap = document.getElementById("wrapQrReaderOtp");
                var btnStop = document.getElementById("btnStopQrOtp");
                var btnScan = document.getElementById("btnScanQrOtp");
                var host = document.getElementById("qrReaderOtpHost");
                if (html5QrOtp) {
                    html5QrOtp.stop().then(function () {
                        try { html5QrOtp.clear(); } catch (e1) { }
                        html5QrOtp = null;
                    }).catch(function () {
                        html5QrOtp = null;
                    });
                }
                if (host) host.innerHTML = "";
                if (wrap) wrap.style.display = "none";
                if (btnStop) btnStop.style.display = "none";
                if (btnScan) btnScan.disabled = false;
            }

            function iniciarCamaraQr() {
                var btnScan = document.getElementById("btnScanQrOtp");
                var btnStop = document.getElementById("btnStopQrOtp");
                var wrap = document.getElementById("wrapQrReaderOtp");
                var host = document.getElementById("qrReaderOtpHost");
                if (!btnScan || !wrap || !host) return;

                btnScan.disabled = true;
                lastQrText = "";
                host.innerHTML = "";
                wrap.style.display = "block";
                if (btnStop) btnStop.style.display = "block";

                loadHtml5QrcodeLib(function () {
                    if (!window.Html5Qrcode) {
                        btnScan.disabled = false;
                        return;
                    }
                    html5QrOtp = new Html5Qrcode("qrReaderOtpHost");
                    var cfg = { fps: 8, qrbox: { width: 220, height: 220 } };
                    var onOk = function (decodedText) {
                        var href = urlOtpQrSegura(decodedText);
                        if (!href) {
                            if (decodedText !== lastQrText) {
                                lastQrText = decodedText;
                                alert("Este QR no es el de acceso de Monolito 4B o no pertenece a este sitio.");
                            }
                            return;
                        }
                        lastQrText = decodedText;
                        html5QrOtp.stop().then(function () {
                            window.location.assign(href);
                        }).catch(function () {
                            window.location.assign(href);
                        });
                    };
                    var onErr = function () { };

                    html5QrOtp.start({ facingMode: "environment" }, cfg, onOk, onErr).catch(function () {
                        html5QrOtp.start({ facingMode: "user" }, cfg, onOk, onErr).catch(function () {
                            alert("No se pudo abrir la cámara. Permita el acceso a la cámara o escriba el OTP de 6 dígitos.");
                            detenerCamaraQr();
                        });
                    });
                });
            }

            document.addEventListener("DOMContentLoaded", function () {
                soloDigitos(document.getElementById("txt_ced"), 10);
                soloDigitos(document.getElementById("txt_otp"), 6);

                var togglePassword = document.getElementById("togglePassword");
                var passwordInput = document.getElementById("txt_pass");

                if (togglePassword && passwordInput) {
                    togglePassword.addEventListener("click", function () {
                        var isPass = passwordInput.getAttribute("type") === "password";
                        passwordInput.setAttribute("type", isPass ? "text" : "password");
                        togglePassword.classList.toggle("bx-show");
                        togglePassword.classList.toggle("bx-hide");
                    });
                }

                var btnScan = document.getElementById("btnScanQrOtp");
                var btnStop = document.getElementById("btnStopQrOtp");
                if (btnScan) btnScan.addEventListener("click", iniciarCamaraQr);
                if (btnStop) btnStop.addEventListener("click", detenerCamaraQr);
            });
        })();

        // Bloquear el botón de retroceso del navegador
        window.history.pushState(null, "", window.location.href);
        window.onpopstate = function () {
            window.history.pushState(null, "", window.location.href);
        };

    </script>
    <script src="<%= ResolveUrl("~/Scripts/disable-form-autofill.js") %>" defer></script>
</body>
</html>
