<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistroExito.aspx.cs" Inherits="Monolito4_B.Seguridad.RegistroExito" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Registro correcto — QR de acceso</title>
    <style>
        body { font-family: 'Segoe UI', system-ui, sans-serif; background: #0f172a; color: #e2e8f0; margin: 0; min-height: 100vh; display: flex; align-items: center; justify-content: center; padding: 24px; }
        .card { max-width: 420px; background: #1e293b; border-radius: 20px; padding: 28px; text-align: center; border: 1px solid #334155; box-shadow: 0 20px 50px rgba(0,0,0,.4); }
        h1 { font-size: 1.25rem; margin: 0 0 12px; color: #f8fafc; }
        p { font-size: .9rem; line-height: 1.55; color: #94a3b8; margin: 0 0 18px; }
        .qr { max-width: 260px; margin: 0 auto 20px; border-radius: 12px; background: #fff; padding: 12px; }
        .btn { display: inline-block; padding: 12px 24px; border-radius: 12px; background: #38bdf8; color: #0f172a; font-weight: 700; text-decoration: none; border: none; cursor: pointer; font-size: .95rem; }
        .btn:hover { filter: brightness(1.05); }
    </style>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div class="card">
            <h1>¡Registro completado!</h1>
            <p><asp:Literal ID="litNota" runat="server" /></p>
            <div class="qr">
                <asp:Image ID="imgQr" runat="server" AlternateText="Código QR de acceso" style="width:100%;height:auto;" />
            </div>
            <asp:Button ID="btnIrLogin" runat="server" CssClass="btn" Text="Ir al inicio de sesión" OnClick="btnIrLogin_Click" />
        </div>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/disable-form-autofill.js") %>" defer></script>
</body>
</html>
