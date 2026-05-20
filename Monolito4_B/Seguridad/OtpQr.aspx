<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtpQr.aspx.cs" Inherits="Monolito4_B.Seguridad.OtpQr" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Validación QR — OTP</title>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background: #1a1a2e; color: #eee; margin: 0; padding: 40px 16px; text-align: center; }
        .card { max-width: 420px; margin: 0 auto; background: #16213e; border-radius: 16px; padding: 28px; box-shadow: 0 8px 32px rgba(0,0,0,.35); }
        h1 { font-size: 1.25rem; margin-bottom: 12px; }
        .ok { color: #6bcb77; }
        .err { color: #ff6b6b; }
        a { color: #7fdbda; }
    </style>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div class="card">
            <h1>Monolito 4B — acceso por QR</h1>
            <asp:Label ID="lblMsg" runat="server" />
            <p style="margin-top:20px;"><a href="Login.aspx">Ir al inicio de sesión</a></p>
        </div>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/disable-form-autofill.js") %>" defer></script>
</body>
</html>
