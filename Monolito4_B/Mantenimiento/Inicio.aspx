<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Mantenimiento/Principal.master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="Monolito4_B.Mantenimiento.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .in-wrap {
            max-width: 1100px;
            margin: 0 auto;
        }

        .in-hero {
            background: linear-gradient(120deg, #1d4ed8 0%, #6366f1 50%, #8b5cf6 100%);
            color: #fff;
            border-radius: 24px;
            padding: 32px 36px 36px;
            margin-bottom: 28px;
            box-shadow: 0 20px 50px rgba(79, 70, 229, .35);
            position: relative;
            overflow: hidden;
        }

            .in-hero::after {
                content: "";
                position: absolute;
                right: -30px;
                bottom: -40px;
                width: 220px;
                height: 220px;
                border-radius: 50%;
                background: rgba(255,255,255,.1);
            }

            .in-hero h1 {
                font-size: 1.85rem;
                font-weight: 800;
                margin: 0 0 10px;
                letter-spacing: -.03em;
            }

            .in-hero p {
                margin: 0;
                opacity: .92;
                font-size: .98rem;
                max-width: 560px;
                line-height: 1.55;
            }

        .in-hero-row {
            display: flex;
            align-items: flex-start;
            gap: 22px;
            flex-wrap: wrap;
            position: relative;
            z-index: 1;
        }

        .in-hero-text {
            flex: 1;
            min-width: 220px;
        }

        .in-dash-avatar {
            width: 88px;
            height: 88px;
            border-radius: 50%;
            object-fit: cover;
            border: 3px solid rgba(255,255,255,.4);
            box-shadow: 0 10px 28px rgba(0,0,0,.2);
            flex-shrink: 0;
            background: rgba(255,255,255,.12);
        }

        .in-dash {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 18px;
            margin-bottom: 28px;
        }

        .in-stat {
            background: #fff;
            border-radius: 18px;
            padding: 22px 22px 20px;
            border: 1px solid #e2e8f0;
            box-shadow: 0 10px 30px rgba(15, 23, 42, .06);
            display: flex;
            flex-direction: column;
            gap: 6px;
            position: relative;
            overflow: hidden;
        }

            .in-stat::before {
                content: "";
                position: absolute;
                left: 0;
                top: 0;
                bottom: 0;
                width: 4px;
                border-radius: 4px 0 0 4px;
            }

            .in-stat:nth-child(1)::before {
                background: linear-gradient(180deg, #22c55e, #16a34a);
            }

            .in-stat:nth-child(2)::before {
                background: linear-gradient(180deg, #f97316, #ea580c);
            }

            .in-stat:nth-child(3)::before {
                background: linear-gradient(180deg, #6366f1, #4f46e5);
            }

            .in-stat .lbl {
                font-size: .78rem;
                font-weight: 600;
                text-transform: uppercase;
                letter-spacing: .06em;
                color: #64748b;
            }

            .in-stat .num {
                font-size: 2.15rem;
                font-weight: 800;
                color: #0f172a;
                letter-spacing: -.03em;
            }

        .in-panel {
            background: #fff;
            border-radius: 20px;
            padding: 26px 28px 28px;
            border: 1px solid #e2e8f0;
            box-shadow: 0 10px 36px rgba(15, 23, 42, .06);
        }

            .in-panel h2 {
                margin: 0 0 14px;
                font-size: 1.15rem;
                color: #1e293b;
                font-weight: 700;
            }

            .in-panel p {
                margin: 0;
                color: #475569;
                line-height: 1.65;
                font-size: .95rem;
            }

            .in-panel a {
                color: #4f46e5;
                font-weight: 600;
                text-decoration: none;
                border-bottom: 2px solid rgba(79, 70, 229, .25);
            }

                .in-panel a:hover {
                    border-bottom-color: #4f46e5;
                }

        .in-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            margin-top: 18px;
        }

        .in-chip {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding: 10px 16px;
            border-radius: 999px;
            font-size: .85rem;
            font-weight: 600;
            background: #eef2ff;
            color: #4338ca;
            border: 1px solid #c7d2fe;
            text-decoration: none;
            transition: transform .15s, box-shadow .2s;
        }

            .in-chip:hover {
                transform: translateY(-1px);
                box-shadow: 0 6px 16px rgba(67, 56, 202, .2);
            }

            .in-chip.teal {
                background: #ccfbf1;
                color: #0f766e;
                border-color: #99f6e4;
            }

        .in-err {
            color: #b91c1c;
            margin-top: 16px;
            font-size: .9rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="in-wrap">
        <div class="in-hero">
            <div class="in-hero-row">
                <asp:Image ID="imgDashAvatar" runat="server" CssClass="in-dash-avatar" AlternateText="Su foto de perfil" />
                <div class="in-hero-text">
                    <h1>Panel principal</h1>
                    <p>Resumen rápido de la base de usuarios y accesos a las herramientas del sistema.</p>
                </div>
            </div>
        </div>

        <div class="in-dash">
            <div class="in-stat">
                <span class="lbl">Activos</span>
                <asp:Label ID="lblActivos" runat="server" CssClass="num" Text="0" />
            </div>
            <div class="in-stat">
                <span class="lbl">Bloqueados</span>
                <asp:Label ID="lblBloq" runat="server" CssClass="num" Text="0" />
            </div>
            <div class="in-stat">
                <span class="lbl">Total</span>
                <asp:Label ID="lblTotal" runat="server" CssClass="num" Text="0" />
            </div>
        </div>

        <asp:Panel ID="pnlAdminInfo" runat="server" CssClass="in-panel" Visible="false">
            <h2>Administración y Gestión</h2>
            <p>Gestione cuentas, desbloquee intentos fallidos y administre el <strong>catálogo de productos</strong>.</p>
            <div class="in-actions">
                <a class="in-chip" href="UsuariosAdmin.aspx"><i class='bx bx-group'></i>Usuarios (CRUD)</a>
                <a class="in-chip" href="Desbloqueo.aspx"><i class='bx bx-lock-open-alt'></i>Desbloqueo</a>

                <!-- Nuevos botones de productos agregados aquí -->
                <a class="in-chip teal" href="listar_tbl_producto.aspx"><i class='bx bx-box'></i>Listar Productos</a>
                <a class="in-chip teal" href="nuevo_tbl_producto.aspx"><i class='bx bx-plus-circle'></i>Nuevo Producto</a>

                <a class="in-chip teal" href="MiPerfil.aspx"><i class='bx bx-user-circle'></i>Mi perfil</a>
                <a class="in-chip" href="Juego.aspx"><i class='bx bx-grid-alt'></i>3 en raya</a>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlUsuarioInfo" runat="server" CssClass="in-panel" Visible="false">
            <h2>Tu espacio</h2>
            <p>Actualice sus datos en <strong>Mi perfil</strong>, consulte los <strong>productos disponibles</strong> y juegue al <strong>3 en raya</strong>.</p>
            <div class="in-actions">
                <!-- Botón para ver el listado agregado aquí -->
                <a class="in-chip teal" href="listar_tbl_producto.aspx"><i class='bx bx-box'></i>Ver Productos</a>

                <a class="in-chip teal" href="MiPerfil.aspx"><i class='bx bx-user-circle'></i>Mi perfil</a>
                <a class="in-chip" href="Juego.aspx"><i class='bx bx-grid-alt'></i>3 en raya</a>
            </div>
        </asp:Panel>

        <asp:Label ID="lblErrorDash" runat="server" CssClass="in-err" Visible="false" />
    </div>
</asp:Content>
