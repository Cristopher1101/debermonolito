<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registrar.aspx.cs" Inherits="Monolito4_B.Seguridad.registrar" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Nuevo Proyecto - Registro</title>

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
            /* Puedes cambiar esta imagen por una que se adapte a tu nuevo proyecto */
            background: url('https://images.unsplash.com/photo-1451187580459-43490279c0fa?q=80&w=2072&auto=format&fit=crop') no-repeat center;
            background-size: cover;
            overflow-x: hidden;
            padding: 20px 0;
        }

        .main-wrapper {
            position: relative;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 20px;
            width: 100%;
            padding: 20px;
        }

        /* ===== PANELES DE CRISTAL ===== */
        .glass-panel {
            background: rgba(255, 255, 255, 0.1);
            border: 2px solid rgba(255, 255, 255, 0.2);
            backdrop-filter: blur(15px);
            -webkit-backdrop-filter: blur(15px);
            border-radius: 20px;
            color: #fff;
            box-shadow: 0 15px 25px rgba(0, 0, 0, 0.3);
            padding: 40px;
            width: 100%;
            max-width: 650px;
            animation: slideUp 0.8s cubic-bezier(0.25, 1, 0.5, 1) forwards;
        }

        .profile-panel {
            padding: 25px 40px;
            animation-delay: 0.1s;
        }

        @keyframes slideUp {
            from {
                transform: translateY(30px);
                opacity: 0;
            }

            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        /* ===== SELECTOR DESPLEGABLE PRO ===== */
        .profile-header {
            text-align: center;
            margin-bottom: 20px;
            font-size: 1.2em;
            font-weight: 500;
        }

        .glass-select-container {
            position: relative;
            width: 100%;
            height: 50px;
        }

        .glass-select {
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.05);
            border: 2px solid rgba(255, 255, 255, 0.2);
            border-radius: 10px;
            outline: none;
            color: #fff;
            font-size: 1em;
            padding: 0 45px 0 15px;
            transition: all 0.3s ease;
            cursor: pointer;
            /* Ocultar la flecha por defecto del sistema operativo */
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
        }

            .glass-select:hover, .glass-select:focus {
                border-color: #fff;
                background: rgba(255, 255, 255, 0.15);
            }

            /* Estilo de las opciones del select (El fondo debe ser sólido para que se lea) */
            .glass-select option {
                background: #2b2b2b;
                color: #fff;
                padding: 10px;
            }

        .glass-select-container .icon {
            position: absolute;
            right: 15px;
            top: 50%;
            transform: translateY(-50%);
            font-size: 1.5em;
            color: #fff;
            pointer-events: none; /* Para que el clic pase a través del ícono hacia el select */
        }

        /* ===== FORMULARIO ===== */
        .header-text {
            text-align: center;
            margin-bottom: 30px;
        }

            .header-text h2 {
                font-size: 2em;
                font-weight: 600;
            }

            .header-text p {
                font-size: 0.9em;
                font-weight: 300;
                margin-top: 5px;
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

                .input-box input[type="date"] {
                    color: transparent;
                }

                    .input-box input[type="date"]:focus,
                    .input-box input[type="date"]:valid {
                        color: #fff;
                    }

                    .input-box input[type="date"]::-webkit-calendar-picker-indicator {
                        filter: invert(1);
                        cursor: pointer;
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

        .register-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            column-gap: 20px;
        }

        .full-width {
            grid-column: span 2;
        }

        button.btn-submit {
            width: 100%;
            height: 45px;
            background: #fff;
            border: none;
            border-radius: 40px;
            cursor: pointer;
            font-size: 1em;
            color: #333;
            font-weight: 600;
            transition: all 0.3s ease;
            display: flex;
            justify-content: center;
            align-items: center;
            margin-top: 10px;
        }

            button.btn-submit:hover {
                background: rgba(255, 255, 255, 0.9);
                box-shadow: 0 0 15px rgba(255, 255, 255, 0.3);
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
            border: 2px solid rgba(255, 77, 77, 0.6);
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

        @media (max-width: 500px) {
            .register-grid {
                grid-template-columns: 1fr;
            }

            .full-width {
                grid-column: span 1;
            }
        }
    </style>
</head>
<body>

    <form id="mainForm" runat="server">
        <div class="main-wrapper">

            <div class="glass-panel profile-panel">
                <div class="profile-header">Selecciona tu Rol o Perfil</div>

                <div class="glass-select-container">
                    <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="glass-select" ClientIDMode="Static">
                        <asp:ListItem Text="-- Seleccione un Perfil --" Value="" />
                    </asp:DropDownList>
                    <i class='bx bx-chevron-down icon'></i>
                </div>
            </div>

            <div class="glass-panel" id="panelRegister">
                <div class="header-text">
                    <h2>Crear Cuenta</h2>
                    <p>Completa tus datos para ingresar</p>
                </div>

                <div class="register-grid">
                    <div class="input-box">
                        <asp:TextBox ID="regCedula" runat="server" ClientIDMode="Static"></asp:TextBox>
                        <label>Cédula / RUC</label>
                        <i class='bx bx-id-card icon'></i>
                    </div>
                    <div class="input-box">
                        <asp:TextBox ID="regNick" runat="server" ClientIDMode="Static"></asp:TextBox>
                        <label>Usuario (Nick)</label>
                        <i class='bx bx-user icon'></i>
                    </div>

                    <div class="input-box">
                        <asp:TextBox ID="regNombres" runat="server" ClientIDMode="Static"></asp:TextBox>
                        <label>Nombres</label>
                    </div>
                    <div class="input-box">
                        <asp:TextBox ID="regApellidos" runat="server" ClientIDMode="Static" AutoPostBack="True" OnTextChanged="regApellidos_TextChanged"></asp:TextBox>
                        <label>Apellidos</label>
                    </div>

                    <div class="input-box">
                        <asp:TextBox ID="regCelular" runat="server" ClientIDMode="Static"></asp:TextBox>
                        <label>Celular</label>
                        <i class='bx bx-phone icon'></i>
                    </div>
                    <div class="input-box">
                        <asp:TextBox ID="regFechaCumple" runat="server" TextMode="Date" ClientIDMode="Static"></asp:TextBox>
                        <label>Fecha de Nacimiento</label>
                    </div>

                    <div class="input-box full-width">
                        <asp:TextBox ID="regCorreo" runat="server" TextMode="Email" ClientIDMode="Static"></asp:TextBox>
                        <label>Correo Electrónico</label>
                        <i class='bx bx-envelope icon'></i>
                    </div>

                    <div class="input-box full-width">
                        <asp:TextBox ID="regDireccion" runat="server" ClientIDMode="Static"></asp:TextBox>
                        <label>Dirección Completa</label>
                        <i class='bx bx-map icon'></i>
                    </div>

                    <div class="input-box full-width">
                        <asp:TextBox ID="regPassword" runat="server" TextMode="Password" ClientIDMode="Static"></asp:TextBox>
                        <label>Crear Contraseña</label>
                        <i class='bx bx-hide icon toggle-pass' data-target="regPassword"></i>
                    </div>
                </div>

                <button type="button" class="btn-submit" id="btnProcesarRegistro" onclick="procesarAccion(this)">
                    <span class="btn-text">Registrarse</span>
                    <span class="loader"></span>
                </button>

                <asp:Button ID="btnRegisterASP" runat="server" Text="" Style="display: none;" OnClick="btnRegisterASP_Click" ClientIDMode="Static" />

                <div style="text-align: center; margin-top: 20px; font-size: 0.9em;">
                    <p>¿Ya tienes cuenta? <a href="login.aspx" style="color: #fff; font-weight: 600; cursor: pointer; text-decoration: none;">Inicia Sesión</a></p>
                </div>
            </div>

        </div>
    </form>

    <script>
        document.addEventListener('DOMContentLoaded', () => {
            // Mostrar/Ocultar contraseña
            document.querySelectorAll('.toggle-pass').forEach(icon => {
                icon.addEventListener('click', function () {
                    const targetId = this.getAttribute('data-target');
                    const input = document.getElementById(targetId);

                    if (input.type === 'password') {
                        input.type = 'text';
                        this.classList.replace('bx-hide', 'bx-show');
                    } else {
                        input.type = 'password';
                        this.classList.replace('bx-show', 'bx-hide');
                    }
                });
            });
        });

        // Procesamiento con animación UX
        function procesarAccion(botonRef) {
            const btnText = botonRef.querySelector('.btn-text');
            const loader = botonRef.querySelector('.loader');
            const activePanel = botonRef.closest('.glass-panel');
            const profilePanel = document.querySelector('.profile-panel');
            const ddlPerfil = document.getElementById('ddlPerfil');

            // Validación 1: Verificar que se haya seleccionado un perfil
            if (ddlPerfil.value === "") {
                profilePanel.classList.add('error-shake');
                setTimeout(() => profilePanel.classList.remove('error-shake'), 400);
                return;
            }

            // Validación 2: ningún campo vacío en el formulario
            let inputs = activePanel.querySelectorAll('input[type="text"], input[type="password"], input[type="email"], input[type="date"]');
            let formValido = true;

            inputs.forEach(input => {
                if (input.value.trim() === '') {
                    formValido = false;
                }
            });

            if (!formValido) {
                activePanel.classList.add('error-shake');
                setTimeout(() => activePanel.classList.remove('error-shake'), 400);
                return;
            }

            // Animación de carga
            const textoOriginal = btnText.innerText;
            btnText.style.display = 'none';
            loader.style.display = 'block';
            botonRef.style.cursor = 'not-allowed';

            // Simulación visual antes de enviar al servidor
            setTimeout(() => {
                loader.style.display = 'none';
                btnText.style.display = 'block';
                btnText.innerText = "¡Procesando!";
                botonRef.style.background = "#4CAF50";
                botonRef.style.color = "white";

                // Ejecutar el evento real de ASP.NET después de la animación
                setTimeout(() => {
                    document.getElementById('btnRegisterASP').click();

                    // Restaurar botón (por si falla el postback)
                    btnText.innerText = textoOriginal;
                    botonRef.style.background = "#fff";
                    botonRef.style.color = "#333";
                    botonRef.style.cursor = "pointer";
                }, 1000);

            }, 1500);
        }
    </script>
</body>
</html>
