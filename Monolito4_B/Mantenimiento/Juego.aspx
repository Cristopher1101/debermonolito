<%@ Page Title="3 en raya" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="Juego.aspx.cs" Inherits="Monolito4_B.Mantenimiento.Juego" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ttt-wrap { max-width: 520px; margin: 0 auto; }
        .ttt-card {
            background: #fff;
            border-radius: 20px;
            padding: 28px 26px 30px;
            box-shadow: 0 12px 40px rgba(15, 23, 42, .1);
            border: 1px solid #e2e8f0;
        }
        .ttt-card h1 { margin: 0 0 8px; font-size: 1.5rem; color: #0f172a; }
        .ttt-card .sub { color: #64748b; font-size: .92rem; margin-bottom: 20px; line-height: 1.5; }
        .ttt-estado { font-size: 1rem; font-weight: 600; color: #334155; margin-bottom: 16px; }
        .ttt-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 10px; max-width: 320px; margin: 0 auto 20px; }
        .ttt-cell {
            aspect-ratio: 1;
            min-height: 72px;
            font-size: 2rem;
            font-weight: 800;
            border-radius: 14px;
            border: 2px solid #cbd5e1;
            background: #f8fafc;
            color: #0f172a;
            cursor: pointer;
            transition: background .15s, transform .1s;
        }
        .ttt-cell:disabled { cursor: default; opacity: .75; }
        .ttt-cell:not(:disabled):hover { background: #e0f2fe; transform: scale(1.02); }
        .ttt-rein { width: 100%; max-width: 320px; margin: 0 auto; display: block; padding: 12px; border-radius: 12px; border: none; background: linear-gradient(135deg, #6366f1, #4f46e5); color: #fff; font-weight: 700; cursor: pointer; font-size: .95rem; }
        .ttt-rein:hover { filter: brightness(1.05); }
        .ttt-fin { margin-top: 18px; padding: 14px; border-radius: 12px; background: #ecfdf5; border: 1px solid #a7f3d0; color: #047857; font-weight: 600; text-align: center; }
    </style>
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ttt-wrap">
        <div class="ttt-card">
            <h1>3 en raya</h1>
            <p class="sub">Todo el tablero y la lógica van en el servidor (C#). Dos jugadores en el mismo dispositivo: alternan <strong>X</strong> y <strong>O</strong>. Pulse una casilla vacía.</p>
            <asp:Label ID="lblTurno" runat="server" CssClass="ttt-estado" />
            <div class="ttt-grid">
                <asp:Button ID="btnC0" runat="server" CssClass="ttt-cell" CommandName="Cell" CommandArgument="0" OnCommand="Cell_Command" />
                <asp:Button ID="btnC1" runat="server" CssClass="ttt-cell" CommandName="Cell" CommandArgument="1" OnCommand="Cell_Command" />
                <asp:Button ID="btnC2" runat="server" CssClass="ttt-cell" CommandName="Cell" CommandArgument="2" OnCommand="Cell_Command" />
                <asp:Button ID="btnC3" runat="server" CssClass="ttt-cell" CommandName="Cell" CommandArgument="3" OnCommand="Cell_Command" />
                <asp:Button ID="btnC4" runat="server" CssClass="ttt-cell" CommandName="Cell" CommandArgument="4" OnCommand="Cell_Command" />
                <asp:Button ID="btnC5" runat="server" CssClass="ttt-cell" CommandName="Cell" CommandArgument="5" OnCommand="Cell_Command" />
                <asp:Button ID="btnC6" runat="server" CssClass="ttt-cell" CommandName="Cell" CommandArgument="6" OnCommand="Cell_Command" />
                <asp:Button ID="btnC7" runat="server" CssClass="ttt-cell" CommandName="Cell" CommandArgument="7" OnCommand="Cell_Command" />
                <asp:Button ID="btnC8" runat="server" CssClass="ttt-cell" CommandName="Cell" CommandArgument="8" OnCommand="Cell_Command" />
            </div>
            <asp:Button ID="btnReiniciar" runat="server" Text="Nueva partida" CssClass="ttt-rein" OnClick="btnReiniciar_Click" CausesValidation="false" />
            <asp:Panel ID="pnlFin" runat="server" Visible="false" CssClass="ttt-fin">
                <asp:Literal ID="litFin" runat="server" />
            </asp:Panel>
        </div>
    </div>
</asp:Content>
