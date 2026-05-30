<%@ Page Title="Estadísticas de Producto" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="estadisticas_producto.aspx.cs" Inherits="Monolito4_B.Mantenimiento.estadisticas_producto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Bootstrap y Chart.js para un diseño moderno (wow) -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <style>
        .carousel-item img { height: 400px; object-fit: cover; border-radius: 10px; }
        .stats-container { background: #fff; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); padding: 20px; }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cph_contenido" runat="server">
    <div class="ua-wrap" style="max-width: 1180px; margin: 0 auto;">
        
        <div style="background: linear-gradient(135deg, #1a237e 0%, #3949ab 45%, #5c6bc0 100%); color: #fff; border-radius: 20px; padding: 28px 32px 32px; margin-bottom: 26px; box-shadow: 0 16px 40px rgba(26,35,126,.35);">
            <h1 style="font-size: 1.65rem; font-weight: 700; margin: 0 0 8px;">Detalle y Estadísticas del Producto</h1>
            <p style="margin: 0; opacity: .92; font-size: 1rem;">Visualiza la galería del producto y su rendimiento en el inventario.</p>
        </div>

        <div class="row">
            <!-- Galería de Imágenes (Carrusel) -->
            <div class="col-md-6 mb-4">
                <div class="stats-container">
                    <h4 class="text-center mb-3">Galería de Imágenes</h4>
                    
                    <div id="productCarousel" class="carousel slide" data-bs-ride="carousel">
                        <div class="carousel-inner">
                            <asp:Repeater ID="rptImagenes" runat="server">
                                <ItemTemplate>
                                    <div class='<%# Container.ItemIndex == 0 ? "carousel-item active" : "carousel-item" %>'>
                                        <img src='<%# ResolveUrl(Eval("pimg_path").ToString()) %>' class="d-block w-100" alt="Producto">
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <button class="carousel-control-prev" type="button" data-bs-target="#productCarousel" data-bs-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span class="visually-hidden">Anterior</span>
                        </button>
                        <button class="carousel-control-next" type="button" data-bs-target="#productCarousel" data-bs-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                            <span class="visually-hidden">Siguiente</span>
                        </button>
                    </div>
                    <asp:Label ID="lblNoImages" runat="server" Visible="false" CssClass="text-danger">No hay imágenes para este producto.</asp:Label>
                </div>
            </div>

            <!-- Gráfico de Estadísticas -->
            <div class="col-md-6 mb-4">
                <div class="stats-container">
                    <h4 class="text-center mb-3">Relación Precio vs Stock Global</h4>
                    <canvas id="myChart"></canvas>
                </div>
            </div>
        </div>
        
        <div class="text-center mt-4">
            <a href="listar_tbl_producto.aspx" class="btn btn-secondary">Volver al Listado</a>
        </div>
    </div>

    <!-- Generación del script de Chart.js con datos desde el servidor -->
    <asp:Literal ID="litChartScript" runat="server"></asp:Literal>

</asp:Content>
