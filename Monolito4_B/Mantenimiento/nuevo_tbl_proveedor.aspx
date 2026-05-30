<%@ Page Title="Nuevo Proveedor" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="nuevo_tbl_proveedor.aspx.cs" Inherits="Monolito4_B.Mantenimiento.nuevo_tbl_proveedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ua-wrap { max-width: 1180px; margin: 0 auto; }
        .ua-hero { background: linear-gradient(135deg, #1a237e 0%, #3949ab 45%, #5c6bc0 100%); color: #fff; border-radius: 20px; padding: 28px 32px 32px; margin-bottom: 26px; box-shadow: 0 16px 40px rgba(26,35,126,.35); }
        .ua-hero h1 { font-size: 1.65rem; font-weight: 700; margin: 0 0 8px; letter-spacing: -.02em; }
        .ua-hero p { margin: 0; opacity: .92; font-size: 1rem; line-height: 1.6; max-width: 38rem; }
        
        .ua-card { background: #fff; border-radius: 18px; padding: 24px 26px 28px; box-shadow: 0 8px 32px rgba(0,0,0,.07); border: 1px solid #e8eaf6; margin-bottom: 22px; }
        .ua-card h2 { margin: 0 0 18px; font-size: 1.15rem; color: #1a237e; }
        
        .ua-grid-form { display: grid; grid-template-columns: repeat(2, 1fr); gap: 16px 20px; }
        @media (max-width: 720px) { .ua-grid-form { grid-template-columns: 1fr; } .ua-span2 { grid-column: span 1 !important; } }
        .ua-span2 { grid-column: span 2; }
        
        .ua-field label { display: block; font-size: .72rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: #7986cb; margin-bottom: 6px; }
        .ua-field input[type=text], .ua-field select { width: 100%; height: 46px; padding: 0 14px; border-radius: 12px; border: 1px solid #c5cae9; font-size: .92rem; font-family: inherit; background: #fafbff; transition: border-color .2s, box-shadow .2s; }
        .ua-field input:focus, .ua-field select:focus { outline: none; border-color: #5c6bc0; box-shadow: 0 0 0 3px rgba(92,107,192,.2); }
        
        .ua-form-actions { display: flex; flex-wrap: wrap; gap: 12px; margin-top: 22px; }
        .ua-btn { display: inline-flex; align-items: center; gap: 8px; padding: 11px 20px; border-radius: 12px; border: none; font-weight: 600; font-size: .9rem; cursor: pointer; transition: transform .15s, box-shadow .2s; text-decoration:none; }
        .ua-btn-primary { background: linear-gradient(135deg, #3949ab, #5c6bc0); color: #fff; box-shadow: 0 6px 20px rgba(57,73,171,.4); }
        .ua-btn-primary:hover { transform: translateY(-1px); box-shadow: 0 10px 28px rgba(57,73,171,.45); color:white; }
        .ua-btn-ghost { background: #fff; color: #3949ab; border: 2px solid #c5cae9; }
        .ua-btn-ghost:hover { background: #e8eaf6; }
        
        .ua-msg { padding: 14px 18px; border-radius: 14px; margin-bottom: 18px; font-size: .9rem; line-height: 1.45; display:block; }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cph_contenido" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="ua-wrap">
        <div class="ua-hero">
            <h1>Agregar/Editar Proveedor</h1>
            <p>Registra un nuevo contacto de proveedor.</p>
        </div>
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                
                <asp:Label ID="lblMensaje" runat="server" CssClass="ua-msg" EnableViewState="false"></asp:Label>
                
                <div class="ua-card">
                    <h2>Datos del Proveedor</h2>
                    <div class="ua-grid-form">
                        <div class="ua-field">
                            <label>Nombre:</label>
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100"></asp:TextBox>
                        </div>
                        <div class="ua-field">
                            <label>Teléfono:</label>
                            <asp:TextBox ID="txtTelefono" runat="server" MaxLength="15"></asp:TextBox>
                        </div>
                        <div class="ua-field ua-span2">
                            <label>Dirección:</label>
                            <asp:TextBox ID="txtDireccion" runat="server" MaxLength="150"></asp:TextBox>
                        </div>
                        <div class="ua-field">
                            <label>Estado:</label>
                            <asp:DropDownList ID="ddlEstado" runat="server">
                                <asp:ListItem Value="A" Text="Activo"></asp:ListItem>
                                <asp:ListItem Value="I" Text="Inactivo"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="ua-form-actions">
                        <asp:Button ID="btnGuardar" runat="server" CssClass="ua-btn ua-btn-primary" Text="Guardar Proveedor" OnClick="btnGuardar_Click" />
                        <asp:Button ID="btnRegresar" runat="server" CssClass="ua-btn ua-btn-ghost" Text="Volver a la Lista" OnClick="btnRegresar_Click" CausesValidation="false" />
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
