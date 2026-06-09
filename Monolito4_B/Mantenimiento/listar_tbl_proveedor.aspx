<%@ Page Title="Listar Proveedores" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="listar_tbl_proveedor.aspx.cs" Inherits="Monolito4_B.Mantenimiento.listar_tbl_proveedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ua-wrap { max-width: 1180px; margin: 0 auto; }
        .ua-hero { background: linear-gradient(135deg, #1a237e 0%, #3949ab 45%, #5c6bc0 100%); color: #fff; border-radius: 20px; padding: 28px 32px 32px; margin-bottom: 26px; box-shadow: 0 16px 40px rgba(26,35,126,.35); }
        .ua-hero h1 { font-size: 1.65rem; font-weight: 700; margin: 0 0 8px; letter-spacing: -.02em; }
        .ua-hero p { margin: 0; opacity: .92; font-size: 1rem; line-height: 1.6; max-width: 38rem; }
        
        .ua-toolbar { display: flex; flex-wrap: wrap; gap: 12px; align-items: center; margin-bottom: 18px; }
        .ua-btn { display: inline-flex; align-items: center; gap: 8px; padding: 11px 20px; border-radius: 12px; border: none; font-weight: 600; font-size: .9rem; cursor: pointer; transition: transform .15s, box-shadow .2s; text-decoration: none; }
        .ua-btn-primary { background: linear-gradient(135deg, #3949ab, #5c6bc0); color: #fff; box-shadow: 0 6px 20px rgba(57,73,171,.4); }
        .ua-btn-primary:hover { transform: translateY(-1px); box-shadow: 0 10px 28px rgba(57,73,171,.45); color: #fff; text-decoration:none; }
        
        .ua-card { background: #fff; border-radius: 18px; padding: 24px 26px 28px; box-shadow: 0 8px 32px rgba(0,0,0,.07); border: 1px solid #e8eaf6; margin-bottom: 22px; }
        .ua-card h2 { margin: 0 0 18px; font-size: 1.15rem; color: #1a237e; }
        
        .ua-gv { width: 100%; border-collapse: collapse; font-size: .86rem; }
        .ua-gv th { text-align: left; padding: 12px 10px; background: #e8eaf6; color: #283593; font-weight: 600; border-bottom: 2px solid #c5cae9; }
        .ua-gv td { padding: 11px 10px; border-bottom: 1px solid #eceff1; vertical-align: middle; }
        .ua-gv tr:hover td { background: #f5f7ff; }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cph_contenido" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="ua-wrap">
        <div class="ua-hero">
            <h1>Gestión de Proveedores</h1>
            <p>Lista de proveedores autorizados. Puedes gestionar su estado y detalles de contacto.</p>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMensaje" runat="server" CssClass="ua-msg" EnableViewState="false" Visible="false"></asp:Label>
                <div class="ua-toolbar">
                    <a href="nuevo_tbl_proveedor.aspx" class="ua-btn ua-btn-primary">+ Nuevo Proveedor</a>
                </div>
                
                <div class="ua-card">
                    <h2>Listado</h2>
                    <asp:GridView ID="gvProveedores" runat="server" CssClass="ua-gv" AutoGenerateColumns="False" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="prov_id" HeaderText="ID" />
                            <asp:BoundField DataField="prov_nombre" HeaderText="Nombre del Proveedor" />
                            <asp:BoundField DataField="prov_telefono" HeaderText="Teléfono" />
                            <asp:BoundField DataField="prov_direccion" HeaderText="Dirección" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='<%# (Eval("prov_estado") != null && Eval("prov_estado").ToString() == "A") ? "ua-badge ua-badge-a" : "ua-badge ua-badge-t" %>'>
                                        <%# (Eval("prov_estado") != null && Eval("prov_estado").ToString() == "A") ? "Activo" : "Inactivo" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <a href='nuevo_tbl_proveedor.aspx?cod=<%# Eval("prov_id") %>' class="ua-link">Editar</a>
                                    
                                    <asp:LinkButton ID="btnLogico" runat="server" CommandArgument='<%# Eval("prov_id") %>' OnClick="btnLogico_Click" CssClass="ua-link" ForeColor="#e65100" OnClientClick="return confirm('¿Cambiar el estado de este proveedor?');">
                                        <%# (Eval("prov_estado") != null && Eval("prov_estado").ToString() == "A") ? "Inactivar (Lógico)" : "Activar" %>
                                    </asp:LinkButton>
                                    
                                    <asp:LinkButton ID="btnFisico" runat="server" CommandArgument='<%# Eval("prov_id") %>' OnClick="btnFisico_Click" CssClass="ua-link" ForeColor="#b71c1c" OnClientClick="return confirm('¡PELIGRO! ¿Seguro de ELIMINAR FÍSICAMENTE este proveedor? Esto no se puede deshacer.');">Borrar Físico</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>