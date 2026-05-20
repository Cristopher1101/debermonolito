using System;
using System.Text;
using System.Web.Security;

namespace capa_negocio
{
    /// <summary>
    /// Token firmado para el QR de OTP (sin exponer el código en la URL en texto plano).
    /// </summary>
    public static class OtpQrTokenHelper
    {
        public static string CrearToken(int usuId, string otpPlano, DateTime expiraUtc)
        {
            var payload = $"{usuId}|{expiraUtc.Ticks}|{otpPlano}";
            byte[] protegido = MachineKey.Protect(Encoding.UTF8.GetBytes(payload), "Monolito4B", "OtpQr", "v1");
            return Convert.ToBase64String(protegido);
        }

        public static bool TryValidarToken(string tokenBase64, out int usuId, out string otpPlano, out string mensajeError)
        {
            usuId = 0;
            otpPlano = null;
            mensajeError = null;

            if (string.IsNullOrWhiteSpace(tokenBase64))
            {
                mensajeError = "Token vacío.";
                return false;
            }

            try
            {
                byte[] raw = Convert.FromBase64String(tokenBase64.Trim());
                byte[] des = MachineKey.Unprotect(raw, "Monolito4B", "OtpQr", "v1");
                if (des == null || des.Length == 0)
                {
                    mensajeError = "Token inválido o manipulado.";
                    return false;
                }

                string s = Encoding.UTF8.GetString(des);
                var partes = s.Split('|');
                if (partes.Length != 3)
                {
                    mensajeError = "Formato de token incorrecto.";
                    return false;
                }

                if (!int.TryParse(partes[0], out usuId))
                {
                    mensajeError = "Identificador inválido.";
                    return false;
                }

                if (!long.TryParse(partes[1], out long ticks))
                {
                    mensajeError = "Expiración inválida.";
                    return false;
                }

                if (new DateTime(ticks, DateTimeKind.Utc) < DateTime.UtcNow)
                {
                    mensajeError = "El enlace del QR ha expirado. Solicite un nuevo inicio de sesión.";
                    return false;
                }

                otpPlano = partes[2];
                return true;
            }
            catch (Exception ex)
            {
                mensajeError = "No se pudo leer el token: " + ex.Message;
                return false;
            }
        }
    }
}
