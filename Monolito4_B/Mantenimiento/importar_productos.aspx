<%@ Page Title="Importar Productos" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="importar_productos.aspx.cs" Inherits="Monolito4_B.Mantenimiento.importar_productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ua-wrap { max-width: 1180px; margin: 0 auto; }
        .ua-hero { background: linear-gradient(135deg, #1a237e 0%, #3949ab 45%, #5c6bc0 100%); color: #fff; border-radius: 20px; padding: 28px 32px 32px; margin-bottom: 26px; box-shadow: 0 16px 40px rgba(26,35,126,.35); }
        .ua-hero h1 { font-size: 1.65rem; font-weight: 700; margin: 0 0 8px; letter-spacing: -.02em; }
        .ua-hero p { margin: 0; opacity: .92; font-size: 1rem; line-height: 1.6; max-width: 38rem; }
        
        .ua-card { background: #fff; border-radius: 18px; padding: 24px 26px 28px; box-shadow: 0 8px 32px rgba(0,0,0,.07); border: 1px solid #e8eaf6; margin-bottom: 22px; }
        .ua-card h2 { margin: 0 0 18px; font-size: 1.15rem; color: #1a237e; }
        
        .ua-gv { width: 100%; border-collapse: collapse; font-size: .86rem; }
        .ua-gv th { text-align: left; padding: 12px 10px; background: #e8eaf6; color: #283593; font-weight: 600; border-bottom: 2px solid #c5cae9; }
        .ua-gv td { padding: 11px 10px; border-bottom: 1px solid #eceff1; vertical-align: middle; }
        .ua-gv tr:hover td { background: #f5f7ff; }
        
        .ua-form-actions { display: flex; flex-wrap: wrap; gap: 12px; margin-top: 22px; }
        .ua-btn { display: inline-flex; align-items: center; gap: 8px; padding: 11px 20px; border-radius: 12px; border: none; font-weight: 600; font-size: .9rem; cursor: pointer; transition: transform .15s, box-shadow .2s; text-decoration:none; }
        .ua-btn-primary { background: linear-gradient(135deg, #3949ab, #5c6bc0); color: #fff; box-shadow: 0 6px 20px rgba(57,73,171,.4); }
        .ua-btn-primary:hover { transform: translateY(-1px); box-shadow: 0 10px 28px rgba(57,73,171,.45); color:white; }
        .ua-btn-success { background: linear-gradient(135deg, #2e7d32, #43a047); color: #fff; box-shadow: 0 6px 20px rgba(46,125,50,.4); }
        .ua-btn-success:hover { transform: translateY(-1px); box-shadow: 0 10px 28px rgba(46,125,50,.45); color:white; }
        .ua-btn-ghost { background: #fff; color: #3949ab; border: 2px solid #c5cae9; }
        .ua-btn-ghost:hover { background: #e8eaf6; }
        
        .ua-msg { padding: 14px 18px; border-radius: 14px; margin-bottom: 18px; font-size: .9rem; line-height: 1.45; display:block; }
        .upload-container { padding: 30px; border: 2px dashed #c5cae9; border-radius: 14px; text-align: center; background: #fafbff; }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cph_contenido" runat="server">
    
    <div class="ua-wrap">
        <div class="ua-hero">
            <h1>Importar Productos Masivamente</h1>
            <p>Sube un archivo de Excel (.xlsx) con las columnas: Nombre, Precio, Cantidad, Prov_ID.</p>
        </div>
        
        <asp:Label ID="lblMensaje" runat="server" CssClass="ua-msg" EnableViewState="false"></asp:Label>
        
        <div class="ua-card">
            <h2>Gestión de Archivos (Excel)</h2>
            <div class="upload-container">
                <p style="margin-bottom: 15px; color:#5c6bc0; font-size:0.9rem;">Para importar, selecciona un archivo Excel (.xlsx). Para exportar con el diseño completo de Excel, haz clic en "Exportar a Excel". (Máx 5MB)</p>
                <asp:FileUpload ID="fileUploadExcel" runat="server" accept=".xlsx,.xls" onchange="validarExcel(this);" />
                <div class="ua-form-actions" style="justify-content:center;">
                    <asp:Button ID="btnPrevisualizar" runat="server" Text="Previsualizar Datos a Importar" CssClass="ua-btn ua-btn-primary" OnClick="btnPrevisualizar_Click" />
                    <asp:Button ID="btnExportar" runat="server" Text="Exportar a Excel" CssClass="ua-btn ua-btn-ghost" OnClick="btnExportar_Click" />
                </div>
            </div>
            
            <script>
                function validarExcel(input) {
                    if (input.files && input.files.length > 0) {
                        var file = input.files[0];
                        if (file.size > 5 * 1024 * 1024) {
                            alert("El archivo '" + file.name + "' pesa más de 5 MB. Por favor, sube un archivo más ligero.");
                            input.value = ""; // Limpiar selección
                            return;
                        }
                        var ext = file.name.split('.').pop().toLowerCase();
                        if (ext !== "xlsx" && ext !== "xls") {
                            alert("Solo se permiten archivos de Excel (.xlsx o .xls).");
                            input.value = "";
                            return;
                        }
                    }
                }
            </script>
        </div>

        <asp:Panel ID="pnlPreview" runat="server" Visible="false" CssClass="ua-card">
            <h2>Vista Previa de Datos</h2>
            <asp:GridView ID="gvPreview" runat="server" AutoGenerateColumns="true" CssClass="ua-gv" GridLines="None"></asp:GridView>

            <div class="ua-form-actions">
                <asp:Button ID="btnProcesar" runat="server" Text="Confirmar e Importar (Upsert)" CssClass="ua-btn ua-btn-success" OnClick="btnProcesar_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
