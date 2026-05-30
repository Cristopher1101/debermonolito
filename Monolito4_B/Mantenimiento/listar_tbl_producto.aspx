<%@ Page Title="Listado de Productos" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="listar_tbl_producto.aspx.cs" Inherits="Monolito4_B.Mantenimiento.listar_tbl_producto" %>

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
        .ua-btn-ghost { background: #fff; color: #3949ab; border: 2px solid #c5cae9; text-decoration:none; }
        .ua-btn-ghost:hover { background: #e8eaf6; text-decoration:none; }
        
        .ua-msg { padding: 14px 18px; border-radius: 14px; margin-bottom: 18px; font-size: .9rem; line-height: 1.45; display:block; }
        
        .ua-card { background: #fff; border-radius: 18px; padding: 24px 26px 28px; box-shadow: 0 8px 32px rgba(0,0,0,.07); border: 1px solid #e8eaf6; margin-bottom: 22px; }
        .ua-card h2 { margin: 0 0 18px; font-size: 1.15rem; color: #1a237e; }
        
        .ua-gv { width: 100%; border-collapse: collapse; font-size: .86rem; }
        .ua-gv th { text-align: left; padding: 12px 10px; background: #e8eaf6; color: #283593; font-weight: 600; border-bottom: 2px solid #c5cae9; }
        .ua-gv td { padding: 11px 10px; border-bottom: 1px solid #eceff1; vertical-align: middle; }
        .ua-gv tr:hover td { background: #f5f7ff; }
        
        .search-box { height: 44px; padding: 0 14px; border-radius: 12px; border: 1px solid #c5cae9; font-size: .92rem; font-family: inherit; background: #fafbff; width: 300px; transition: border-color .2s, box-shadow .2s; }
        .search-box:focus { outline: none; border-color: #5c6bc0; box-shadow: 0 0 0 3px rgba(92,107,192,.2); }
        .product-img { max-width: 60px; max-height: 60px; border-radius: 8px; border: 1px solid #e8eaf6; }
        
        .ua-link { color: #3949ab; font-weight: 600; text-decoration: none; margin-right: 10px; }
        .ua-link:hover { text-decoration: underline; }
        
        .ua-badge { display: inline-block; padding: 4px 10px; border-radius: 999px; font-size: .72rem; font-weight: 600; }
        .ua-badge-a { background: #e8f5e9; color: #2e7d32; }
        
        .grid-pager { margin-top: 15px; }
        .grid-pager table { margin: 0 auto; }
        .grid-pager td { padding: 5px 10px; background: white; cursor: pointer; }
        .grid-pager td span { font-weight: bold; color: #1a237e; background: #e8eaf6; border-radius:5px; padding:5px 10px; }
        .grid-pager td a { color: #3949ab; text-decoration:none; padding:5px 10px; }
        .grid-pager td a:hover { background: #f5f7ff; border-radius:5px; }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cph_contenido" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="ua-wrap">
        <div class="ua-hero">
            <h1>Gestión de Productos</h1>
            <p>Listado completo de productos en inventario. Puedes buscar rápidamente por nombre o proveedor.</p>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMensaje" runat="server" CssClass="ua-msg" EnableViewState="false" Visible="false"></asp:Label>
                
                <div class="ua-toolbar">
                    <asp:TextBox ID="txtBusqueda" runat="server" CssClass="search-box" AutoPostBack="true" OnTextChanged="txtBusqueda_TextChanged" placeholder="Buscar por nombre..."></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="ua-btn ua-btn-primary" OnClick="btnBuscar_Click" />
                    <div style="flex-grow:1;"></div>
                    <a href="nuevo_tbl_producto.aspx" class="ua-btn ua-btn-primary">+ Nuevo Producto</a>
                    <a href="importar_productos.aspx" class="ua-btn ua-btn-ghost">Importar CSV</a>
                </div>
                
                <div class="ua-card">
                    <h2>Listado</h2>
                    <asp:GridView ID="gvProductos" runat="server" CssClass="ua-gv" AutoGenerateColumns="False" 
                        AllowPaging="True" PageSize="5" OnPageIndexChanging="gvProductos_PageIndexChanging" 
                        EmptyDataText="No se encontraron productos." GridLines="None">
                        
                        <Columns>
                            <asp:BoundField DataField="pro_id" HeaderText="ID" />
                            <asp:TemplateField HeaderText="Imagen">
                                <ItemTemplate>
                                    <img src='<%# Eval("ImagenPrincipal") != null && Eval("ImagenPrincipal").ToString() != "" ? ResolveUrl(Eval("ImagenPrincipal").ToString()) : ResolveUrl("~/Images/no-image.png") %>' class="product-img" alt="Producto" onerror="this.src='https://via.placeholder.com/60';" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="pro_nombre" HeaderText="Producto" />
                            <asp:BoundField DataField="ProveedorNombre" HeaderText="Proveedor" NullDisplayText="N/A" />
                            <asp:BoundField DataField="pro_precio" HeaderText="Precio" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="pro_cantidad" HeaderText="Stock" />
                            
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <a href='nuevo_tbl_producto.aspx?cod=<%# Eval("pro_id") %>' class="ua-link">Editar</a>
                                    <a href='estadisticas_producto.aspx?cod=<%# Eval("pro_id") %>' class="ua-link" style="color: #059669;">Ver Detalle</a>
                                    <asp:LinkButton ID="btnBorrar" runat="server" CommandArgument='<%# Eval("pro_id") %>' OnClick="btnBorrar_Click" CssClass="ua-link" ForeColor="#b71c1c" OnClientClick="return confirm('¿Seguro de borrar este producto?');">Borrar</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="grid-pager" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <p style="padding: 20px; color: #757575;">No se encontraron productos con ese criterio.</p>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
