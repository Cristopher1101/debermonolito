<%@ Page Title="Desbloqueo" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="Desbloqueo.aspx.cs" Inherits="Monolito4_B.Mantenimiento.Desbloqueo" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="server">
    <style>
        .wrap { background: #fff; border-radius: 14px; padding: 24px; box-shadow: 0 4px 18px rgba(0,0,0,.06); }
        table { width: 100%; border-collapse: collapse; }
        th, td { padding: 12px; border-bottom: 1px solid #eee; text-align: left; }
        th { background: #f5f5f7; font-weight: 600; }
    </style>
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 style="color:#1a1a2e;">Desbloquear usuarios</h1>
    <p style="color:#555;">Solo administradores. Restablece estado activo e intentos a cero.</p>

    <div class="wrap" style="margin-top:20px;">
        <asp:Label ID="lblMsg" runat="server" EnableViewState="false" />
        <asp:GridView ID="gvBloqueados" runat="server" AutoGenerateColumns="False" DataKeyNames="usu_id"
            OnRowCommand="gvBloqueados_RowCommand" CssClass="tbl" GridLines="None">
            <Columns>
                <asp:BoundField DataField="usu_cedula" HeaderText="Cédula" />
                <asp:BoundField DataField="usu_nombres" HeaderText="Nombres" />
                <asp:BoundField DataField="usu_apellidos" HeaderText="Apellidos" />
                <asp:BoundField DataField="usu_intentos" HeaderText="Intentos" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" Text="Desbloquear" CommandName="Desbloquear"
                            CommandArgument='<%# Eval("usu_id") %>' CssClass="btn" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
