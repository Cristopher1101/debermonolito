using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace capa_negocio
{
    /// <summary>Reglas compartidas para Mi perfil (usuario) y gestión admin.</summary>
    public static class PerfilValidacion
    {
        public const int NickMinLen = 4;
        public const int NickMaxLen = 50;
        public const int NombreMaxLen = 50;
        public const int CorreoMaxLen = 150;
        public const int DireccionMinLen = 5;
        public const int DireccionMaxLen = 50;
        public const int PasswordMinLen = 6;
        public const int EdadMinima = 5;
        public const int EdadMaxima = 120;

        private static readonly Regex RxNick = new Regex(@"^[\p{L}0-9._-]{4,50}$", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex RxCelular = new Regex(@"^\d{9,10}$", RegexOptions.Compiled);
        private static readonly Regex RxNombreSoloLetras = new Regex(@"^[\p{L}\s'-]+$", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex RxContieneDigito = new Regex(@"\d", RegexOptions.Compiled);

        public static int ContarPalabras(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return 0;
            return texto.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>Recorta y colapsa espacios intermedios (nombres / apellidos).</summary>
        public static string NormalizarNombresParaGuardar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;
            return Regex.Replace(texto.Trim(), @"\s+", " ");
        }

        private static bool EsNombreSoloLetrasYSeparadores(string normalizado, string campoAmigable, out string error)
        {
            error = null;
            if (RxContieneDigito.IsMatch(normalizado))
            {
                error = $"{campoAmigable} no pueden incluir números; use solo letras.";
                return false;
            }
            if (!RxNombreSoloLetras.IsMatch(normalizado))
            {
                error = $"{campoAmigable} solo pueden contener letras y espacios (también ' o - entre letras, sin otros símbolos).";
                return false;
            }
            return true;
        }

        public static bool TryValidarNick(string nick, out string error)
        {
            error = null;
            nick = (nick ?? string.Empty).Trim();
            if (nick.Length < NickMinLen || nick.Length > NickMaxLen)
            {
                error = $"El nick debe tener entre {NickMinLen} y {NickMaxLen} caracteres.";
                return false;
            }
            if (!RxNick.IsMatch(nick))
            {
                error = "El nick solo puede usar letras, números y los símbolos . _ - (entre 4 y 50 caracteres).";
                return false;
            }
            return true;
        }

        public static bool TryValidarNombres(string nombres, out string error)
        {
            error = null;
            nombres = NormalizarNombresParaGuardar(nombres ?? string.Empty);
            if (string.IsNullOrEmpty(nombres) || nombres.Length > NombreMaxLen)
            {
                error = $"Los nombres son obligatorios y como máximo {NombreMaxLen} caracteres.";
                return false;
            }
            if (ContarPalabras(nombres) < 2)
            {
                error = "Ingrese al menos dos nombres separados por espacio.";
                return false;
            }
            if (!EsNombreSoloLetrasYSeparadores(nombres, "Los nombres", out error))
                return false;
            return true;
        }

        public static bool TryValidarApellidos(string apellidos, out string error)
        {
            error = null;
            apellidos = NormalizarNombresParaGuardar(apellidos ?? string.Empty);
            if (string.IsNullOrEmpty(apellidos) || apellidos.Length > NombreMaxLen)
            {
                error = $"Los apellidos son obligatorios y como máximo {NombreMaxLen} caracteres.";
                return false;
            }
            if (ContarPalabras(apellidos) < 2)
            {
                error = "Ingrese al menos dos apellidos separados por espacio.";
                return false;
            }
            if (!EsNombreSoloLetrasYSeparadores(apellidos, "Los apellidos", out error))
                return false;
            return true;
        }

        public static bool TryValidarCelular(string celular, out string error)
        {
            error = null;
            celular = (celular ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(celular) || !RxCelular.IsMatch(celular))
            {
                error = "El celular debe tener 9 o 10 dígitos (solo números).";
                return false;
            }
            return true;
        }

        public static bool TryValidarCorreo(string correo, out string error)
        {
            error = null;
            correo = (correo ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(correo) || correo.Length > CorreoMaxLen)
            {
                error = $"El correo es obligatorio y como máximo {CorreoMaxLen} caracteres.";
                return false;
            }
            try
            {
                new MailAddress(correo);
            }
            catch (FormatException)
            {
                error = "Correo electrónico no válido.";
                return false;
            }
            return true;
        }

        public static bool TryValidarDireccion(string direccion, out string error)
        {
            error = null;
            direccion = (direccion ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(direccion) || direccion.Length < DireccionMinLen || direccion.Length > DireccionMaxLen)
            {
                error = $"La dirección debe tener entre {DireccionMinLen} y {DireccionMaxLen} caracteres.";
                return false;
            }
            return true;
        }

        public static bool TryValidarFechaNacimiento(DateTime fecha, out string error)
        {
            error = null;
            if (fecha > DateTime.Today)
            {
                error = "La fecha de nacimiento no puede ser futura.";
                return false;
            }
            int edad = EdadAl(fecha, DateTime.Today);
            if (edad < EdadMinima)
            {
                error = $"La persona debe tener al menos {EdadMinima} años.";
                return false;
            }
            if (edad > EdadMaxima)
            {
                error = "La fecha de nacimiento no es plausible.";
                return false;
            }
            return true;
        }

        public static bool TryValidarFechaNacimientoTexto(string textoFecha, out DateTime fecha, out string error)
        {
            fecha = default;
            error = null;
            if (string.IsNullOrWhiteSpace(textoFecha) || !DateTime.TryParse(textoFecha, out fecha))
            {
                error = "Seleccione una fecha de nacimiento válida.";
                return false;
            }
            return TryValidarFechaNacimiento(fecha.Date, out error);
        }

        public static int EdadAl(DateTime fechaNac, DateTime referencia)
        {
            int edad = referencia.Year - fechaNac.Year;
            if (referencia.Month < fechaNac.Month || (referencia.Month == fechaNac.Month && referencia.Day < fechaNac.Day))
                edad--;
            return edad;
        }

        public static bool TryValidarPasswordOpcional(string password, out string error)
        {
            error = null;
            if (string.IsNullOrEmpty(password))
                return true;
            if (password.Length < PasswordMinLen)
            {
                error = $"La contraseña debe tener al menos {PasswordMinLen} caracteres.";
                return false;
            }
            return true;
        }
    }
}
