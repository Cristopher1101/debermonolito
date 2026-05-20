<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recuperar.aspx.cs" Inherits="Monolito4_B.Seguridad.Recuperar" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <title>Recuperar Contraseña</title>
    <link href='https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css' rel='stylesheet'>
    <style>
        /* Agrega aquí exactamente los mismos estilos CSS que usamos en Login.aspx */
        @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap');
        * { margin: 0; padding: 0; box-sizing: border-box; font-family: 'Poppins', sans-serif; }
        body { display: flex; justify-content: center; align-items: center; min-height: 100vh; background: url('https://images.unsplash.com/photo-1550684848-fac1c5b4e853?q=80&w=2070&auto=format&fit=crop') no-repeat center; background-size: cover; }
        .login-glass { width: 420px; padding: 40px; background: rgba(255, 255, 255, 0.1); border: 2px solid rgba(255, 255, 255, 0.2); backdrop-filter: blur(15px); border-radius: 20px; color: #fff; box-shadow: 0 15px 25px rgba(0, 0, 0, 0.3); }
        .login-header { text-align: center; margin-bottom: 30px; }
        .login-header h2 { font-size: 2em; font-weight: 600; }
        .input-box { position: relative; width: 100%; height: 50px; margin-bottom: 25px; }
        .input-box input { width: 100%; height: 100%; background: transparent; border: none; outline: none; border-bottom: 2px solid rgba(255, 255, 255, 0.5); color: #fff; font-size: 1em; padding: 0 35px 0 5px; }
        .input-box label { position: absolute; top: 50%; left: 5px; transform: translateY(-50%); font-size: 1em; color: rgba(255, 255, 255, 0.8); pointer-events: none; transition: 0.3s; }
        .input-box input:focus ~ label, .input-box input:valid ~ label { top: -5px; font-size: 0.85em; color: #fff; }
        .btn-submit { width: 100%; height: 45px; background: #fff; border: none; border-radius: 40px; cursor: pointer; font-size: 1em; color: #333; font-weight: 600; margin-top: 10px; }
    </style>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div class="login-glass">
            <div class="login-header">
                <h2>Recuperar clave</h2>
                <p>Le enviaremos una clave temporal a su <strong>correo</strong> registrado en el sistema.</p>
            </div>

            <div class="input-box">
                <asp:TextBox ID="txtCedulaRecuperar" runat="server" required="true"></asp:TextBox>
                <label>Ingresa tu Cédula</label>
            </div>

            <asp:Button ID="btnRecuperar" runat="server" Text="Enviar clave por correo" CssClass="btn-submit" OnClick="btnRecuperar_Click" />
            
            <div style="text-align: center; margin-top: 20px;">
                <a href="Login.aspx" style="color: #fff; text-decoration: none;">Volver al Login</a>
            </div>
        </div>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/disable-form-autofill.js") %>" defer></script>
</body>
</html>
