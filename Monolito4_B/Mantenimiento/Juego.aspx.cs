using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Monolito4_B.Mantenimiento
{
    public partial class Juego : Page
    {
        private const string VsBoard = "TttBoard";
        private const string VsTurn = "TttTurn";
        private const string VsWinner = "TttWinner";

        private Button[] _celdas;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _celdas = new[] { btnC0, btnC1, btnC2, btnC3, btnC4, btnC5, btnC6, btnC7, btnC8 };
        }

        private string Tablero
        {
            get => ViewState[VsBoard] as string ?? ".........";
            set => ViewState[VsBoard] = value;
        }

        private char TurnoActual
        {
            get
            {
                object o = ViewState[VsTurn];
                if (o is char c)
                    return c;
                if (o is string s && s.Length > 0)
                    return s[0];
                return 'X';
            }
            set => ViewState[VsTurn] = value;
        }

        /// <summary>Nulo = partida en curso; 'X' u 'O' ganador; 'E' empate.</summary>
        private char? GanadorOEmpate
        {
            get
            {
                if (!(ViewState[VsWinner] is string s) || s.Length == 0)
                    return null;
                return s[0];
            }
            set => ViewState[VsWinner] = value.HasValue ? value.Value.ToString() : string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Seguridad/Login.aspx", false);
                return;
            }

            if (!IsPostBack)
                ReiniciarPartida();
            else
                RefrescarBotones();
        }

        private void ReiniciarPartida()
        {
            Tablero = ".........";
            TurnoActual = 'X';
            GanadorOEmpate = null;
            RefrescarBotones();
        }

        private void RefrescarBotones()
        {
            string b = Tablero;
            char? fin = GanadorOEmpate;
            for (int i = 0; i < 9; i++)
            {
                Button btn = _celdas[i];
                char c = i < b.Length ? b[i] : '.';
                btn.Text = c == '.' ? "\u00A0" : c.ToString();
                btn.Enabled = fin == null && c == '.';
            }

            if (fin == null)
            {
                lblTurno.Text = "Turno: " + TurnoActual;
                pnlFin.Visible = false;
            }
            else if (fin == 'E')
            {
                lblTurno.Text = "Partida terminada.";
                pnlFin.Visible = true;
                litFin.Text = "Empate.";
            }
            else
            {
                lblTurno.Text = "Partida terminada.";
                pnlFin.Visible = true;
                litFin.Text = "Ganador: " + fin;
            }
        }

        private static char? EvaluarGanador(string board)
        {
            if (board == null || board.Length != 9) return null;
            int[][] lineas =
            {
                new[] { 0, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 },
                new[] { 0, 3, 6 }, new[] { 1, 4, 7 }, new[] { 2, 5, 8 },
                new[] { 0, 4, 8 }, new[] { 2, 4, 6 }
            };
            foreach (int[] ln in lineas)
            {
                char a = board[ln[0]], c2 = board[ln[1]], c3 = board[ln[2]];
                if (a != '.' && a == c2 && a == c3)
                    return a;
            }
            if (board.IndexOf('.') < 0)
                return 'E';
            return null;
        }

        protected void Cell_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName != "Cell" || GanadorOEmpate != null)
                return;
            if (!int.TryParse(e.CommandArgument?.ToString(), out int idx) || idx < 0 || idx > 8)
                return;

            var sb = new StringBuilder(Tablero);
            if (sb[idx] != '.')
                return;

            sb[idx] = TurnoActual;
            Tablero = sb.ToString();

            char? win = EvaluarGanador(Tablero);
            if (win != null)
                GanadorOEmpate = win;
            else
                TurnoActual = TurnoActual == 'X' ? 'O' : 'X';

            RefrescarBotones();
        }

        protected void btnReiniciar_Click(object sender, EventArgs e)
        {
            ReiniciarPartida();
        }
    }
}
