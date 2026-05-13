<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Monolito4_B.Seguridad.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Sistema Adopción - Acceso Premium</title>

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
            width: 420px;
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
                font-size: 0.9em;
                font-weight: 300;
                margin-top: 5px;
                color: rgba(255,255,255,0.8);
            }

        /* ===== BOTONES SOCIALES ===== */
        .social-login {
            display: flex;
            flex-direction: column;
            gap: 12px;
            margin-bottom: 25px;
        }

        .btn-social {
            width: 100%;
            height: 46px;
            border-radius: 40px;
            border: 2px solid rgba(255, 255, 255, 0.35);
            background: rgba(255, 255, 255, 0.08);
            color: #fff;
            font-size: 0.95em;
            font-weight: 500;
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 10px;
            cursor: pointer;
            transition: all 0.3s ease;
            text-decoration: none;
            backdrop-filter: blur(5px);
        }

            .btn-social:hover {
                background: rgba(255, 255, 255, 0.2);
                border-color: rgba(255, 255, 255, 0.7);
                box-shadow: 0 0 18px rgba(255, 255, 255, 0.15);
                transform: translateY(-1px);
            }

            .btn-social svg {
                width: 20px;
                height: 20px;
                flex-shrink: 0;
            }

        /* ===== DIVISOR ===== */
        .divider {
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 25px;
            color: rgba(255, 255, 255, 0.5);
            font-size: 0.82em;
            letter-spacing: 0.5px;
        }

            .divider::before,
            .divider::after {
                content: '';
                flex: 1;
                height: 1px;
                background: rgba(255, 255, 255, 0.25);
            }

        /* ===== FORMULARIO ===== */
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

        /* Estilo especial para el input del OTP */
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

        /* ===== BOTÓN PRINCIPAL ===== */
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

            button.btn-submit:hover {
                background: rgba(255,255,255,0.88);
                box-shadow: 0 0 18px rgba(255,255,255,0.25);
            }

        .loader {
            display: none;
            width: 22px;
            height: 22px;
            border: 3px solid #333;
            border-bottom-color: transparent;
            border-radius: 50%;
            animation: rotation 0.8s linear infinite;
        }

        @keyframes rotation {
            100% {
                transform: rotate(360deg);
            }
        }

        .error-shake {
            animation: shake 0.4s;
            border: 2px solid rgba(255, 77, 77, 0.5);
        }

        @keyframes shake {
            0%, 100% {
                transform: translateX(0);
            }

            25%, 75% {
                transform: translateX(-8px);
            }

            50% {
                transform: translateX(8px);
            }
        }

        /* ===== BARRA DE PROGRESO ===== */
        .progress-bar-container {
            width: 100%;
            height: 4px;
            background: rgba(255, 255, 255, 0.2);
            border-radius: 10px;
            margin-top: 15px;
            overflow: hidden;
            display: none;
        }

        .progress-bar-fill {
            height: 100%;
            width: 0%;
            background: #fff;
            border-radius: 10px;
            transition: width 0.08s linear;
        }

        .progress-label {
            text-align: center;
            font-size: 0.78em;
            color: rgba(255, 255, 255, 0.7);
            margin-top: 8px;
            display: none;
            letter-spacing: 0.5px;
        }

        /* ===== PIE ===== */
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
    </style>
</head>
<body>

    <form id="loginForm" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <div class="login-wrapper">
            <div class="login-glass" id="glassContainer">

                <!-- ================= PANEL DE LOGIN ================= -->
                <asp:Panel ID="pnlLogin" runat="server">
                    <div class="login-header">
                        <h2>Sistema Adopción</h2>
                        <p>Ingresa tus credenciales</p>
                    </div>

                    <div class="social-login">
                        <a href="#" class="btn-social" id="btnGoogle">
                            <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                <path d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" fill="#4285F4" />
                                <path d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" fill="#34A853" />
                                <path d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z" fill="#FBBC05" />
                                <path d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" fill="#EA4335" />
                            </svg>
                            Continuar con Google
                        </a>

                        <a href="#" class="btn-social" id="btnGithub">
                            <svg viewBox="0 0 24 24" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                <path d="M12 0C5.37 0 0 5.37 0 12c0 5.31 3.435 9.795 8.205 11.385.6.105.825-.255.825-.57 0-.285-.015-1.23-.015-2.235-3.015.555-3.795-.735-4.035-1.41-.135-.345-.72-1.41-1.23-1.695-.42-.225-1.02-.78-.015-.795.945-.015 1.62.87 1.845 1.23 1.08 1.815 2.805 1.305 3.495.99.105-.78.42-1.305.765-1.605-2.67-.3-5.46-1.335-5.46-5.925 0-1.305.465-2.385 1.23-3.225-.12-.3-.54-1.53.12-3.18 0 0 1.005-.315 3.3 1.23.96-.27 1.98-.405 3-.405s2.04.135 3 .405c2.295-1.56 3.3-1.23 3.3-1.23.66 1.65.24 2.88.12 3.18.765.84 1.23 1.905 1.23 3.225 0 4.605-2.805 5.625-5.475 5.925.435.375.81 1.095.81 2.22 0 1.605-.015 2.895-.015 3.3 0 .315.225.69.825.57A12.02 12.02 0 0 0 24 12c0-6.63-5.37-12-12-12z" />
                            </svg>
                            Continuar con GitHub
                        </a>
                    </div>

                    <div class="divider">o ingresa tu cédula</div>

                    <div id="emailForm">
                        <div class="input-box">
                            <asp:TextBox ID="txt_ced" runat="server" required="true" ClientIDMode="Static"></asp:TextBox>
                            <label for="txt_ced">Cédula</label>
                            <i class='bx bx-id-card icon'></i>
                        </div>

                        <div class="input-box">
                            <asp:TextBox ID="txt_pass" runat="server" TextMode="Password" required="true" ClientIDMode="Static"></asp:TextBox>
                            <label for="txt_pass">Contraseña</label>
                            <i class='bx bx-hide icon' id="togglePassword" title="Mostrar/Ocultar"></i>
                        </div>

                        <div class="remember-forgot">
                            <label>
                                <asp:CheckBox ID="chkRemember" runat="server" />
                                Recordarme
                            </label>
                            <a href="#">¿Olvidaste tu contraseña?</a>
                        </div>

                        <button type="submit" runat="server" class="btn-submit" id="btnLogin" onserverclick="btnLogin_Click" clientidmode="Static">
                            <span class="btn-text" id="btnTextLogin">Iniciar Sesión</span>
                            <span class="loader" id="loaderLogin"></span>
                        </button>

                        <div class="progress-bar-container" id="progressContainer">
                            <div class="progress-bar-fill" id="progressFill"></div>
                        </div>
                        <p class="progress-label" id="progressLabel">Verificando credenciales...</p>
                    </div>

                    <div class="footer-link">
                        ¿No tienes cuenta? <a href="registrar.aspx">Regístrate aquí</a>
                    </div>
                </asp:Panel>


                <!-- ================= PANEL DE OTP ================= -->
                <!-- Este panel está oculto por defecto desde el servidor (Visible="false") -->
                <asp:Panel ID="pnlOTP" runat="server" Visible="false">
                    <i class='bx bx-shield-quarter otp-icon-top'></i>

                    <div class="login-header">
                        <h2>Verificación</h2>
                        <p>Hemos enviado un código a tu dispositivo.</p>
                    </div>

                    <div class="input-box otp-input-box">
                        <asp:TextBox ID="txt_otp" runat="server" required="true" MaxLength="6" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                        <label for="txt_otp">Código OTP</label>
                    </div>

                    <button type="submit" runat="server" class="btn-submit" id="btnVerifyOTP" onserverclick="btnVerifyOTP_Click" clientidmode="Static">
                        <span class="btn-text">Validar Acceso</span>
                    </button>

                    <div class="footer-link">
                        ¿No recibiste el código?
                        <asp:LinkButton ID="lnkResend" runat="server" OnClick="lnkResend_Click">Reenviar</asp:LinkButton>
                    </div>
                    <div class="footer-link" style="margin-top: 10px;">
                        <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" Style="color: rgba(255,255,255,0.6); text-decoration: none; font-size: 0.9em;"><i class='bx bx-arrow-back'></i> Volver al Login</asp:LinkButton>
                    </div>
                </asp:Panel>

            </div>
        </div>
    </form>

    <script>
        document.addEventListener("DOMContentLoaded", () => {
            const loginBtn = document.getElementById("btnLogin");
            const togglePassword = document.getElementById("togglePassword");
            const passwordInput = document.getElementById("txt_pass");
            const loginGlass = document.getElementById("glassContainer");

            // --- MOSTRAR/OCULTAR CONTRASEÑA ---
            if (togglePassword && passwordInput) {
                togglePassword.addEventListener("click", () => {
                    const isPass = passwordInput.getAttribute("type") === "password";
                    passwordInput.setAttribute("type", isPass ? "text" : "password");
                    togglePassword.classList.toggle("bx-show");
                    togglePassword.classList.toggle("bx-hide");
                });
            }

            // --- ANIMACIÓN DEL BOTÓN DE LOGIN ---
            if (loginBtn) {
                loginBtn.addEventListener("click", (e) => {
                    const cedEl = document.getElementById("txt_ced");
                    const passEl = document.getElementById("txt_pass");

                    if (!cedEl || !passEl) return;

                    const cedValue = cedEl.value.trim();
                    const passValue = passEl.value.trim();

                    if (!cedValue || !passValue) {
                        loginGlass.classList.add("error-shake");
                        setTimeout(() => loginGlass.classList.remove("error-shake"), 400);
                        return;
                    }

                    e.preventDefault(); // Pausamos el postback para la animación

                    const btnText = document.getElementById("btnTextLogin");
                    const loader = document.getElementById("loaderLogin");
                    const progressContainer = document.getElementById("progressContainer");
                    const progressFill = document.getElementById("progressFill");
                    const progressLabel = document.getElementById("progressLabel");

                    btnText.style.display = "none";
                    loader.style.display = "block";
                    loginBtn.style.cursor = "not-allowed";

                    progressContainer.style.display = "block";
                    progressLabel.style.display = "block";
                    progressFill.style.width = "0%";

                    let progress = 0;
                    let stepIndex = 0;
                    const steps = [
                        { target: 40, label: "Verificando credenciales...", speed: 15 },
                        { target: 80, label: "Generando seguridad...", speed: 20 },
                        { target: 100, label: "Procesando...", speed: 10 }
                    ];

                    function runStep() {
                        if (stepIndex >= steps.length) return;
                        const step = steps[stepIndex];
                        progressLabel.innerText = step.label;

                        const interval = setInterval(() => {
                            if (progress >= step.target) {
                                clearInterval(interval);
                                stepIndex++;
                                if (stepIndex < steps.length) {
                                    setTimeout(runStep, 80);
                                } else {
                                    setTimeout(() => {
                                        // Terminó la animación, disparamos el PostBack real
                                        __doPostBack(loginBtn.name, '');
                                    }, 400);
                                }
                            } else {
                                progress += 1;
                                progressFill.style.width = progress + "%";
                            }
                        }, step.speed);
                    }
                    runStep();
                });
            }
        });
    </script>
</body>
</html>
