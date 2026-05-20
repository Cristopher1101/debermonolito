using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace capa_negocio
{
    /// <summary>
    /// Envío vía CallMeBot (WhatsApp). El usuario debe activar el bot en https://www.callmebot.com/blog/free-api-whatsapp-messages/
    /// y colocar CallMeBotApiKey en Web.config / app.config de la web.
    /// </summary>
    public static class WhatsAppService
    {
        public static string NormalizarTelefonoEcuador(string celularLocal, string codigoPais)
        {
            if (string.IsNullOrWhiteSpace(celularLocal)) return null;
            string d = celularLocal.Trim().Replace(" ", "").Replace("-", "");
            if (d.StartsWith("+")) d = d.Substring(1);
            if (d.StartsWith("0")) d = d.Substring(1);
            if (!d.StartsWith(codigoPais))
                d = codigoPais + d;
            return d;
        }

        public static async Task<bool> EnviarMensajeWhatsAppAsync(string numeroInternacional, string mensaje)
        {
            try
            {
                string apiKey = ConfigurationManager.AppSettings["CallMeBotApiKey"];
                if (string.IsNullOrWhiteSpace(apiKey))
                    return false;

                string encoded = HttpUtility.UrlEncode(mensaje ?? "");
                string url = $"https://api.callmebot.com/whatsapp.php?phone={HttpUtility.UrlEncode(numeroInternacional)}&text={encoded}&apikey={HttpUtility.UrlEncode(apiKey)}";

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(20);
                    var response = await client.GetAsync(url).ConfigureAwait(false);
                    string body = response.Content != null
                        ? await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                        : "";
                    return response.IsSuccessStatusCode && body.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) < 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool EnviarMensajeWhatsApp(string numeroInternacional, string mensaje)
        {
            return EnviarMensajeWhatsAppAsync(numeroInternacional, mensaje).GetAwaiter().GetResult();
        }

        /// <summary>Envía únicamente el texto del OTP (p. ej. 6 dígitos), sin prefijos ni mensajes extra.</summary>
        public static bool EnviarSoloTextoOtp(string celularUsuarioEcuador, string textoExacto)
        {
            if (string.IsNullOrWhiteSpace(textoExacto))
                return false;
            string codPais = ConfigurationManager.AppSettings["WhatsAppDefaultCountryCode"] ?? "593";
            string tel = NormalizarTelefonoEcuador(celularUsuarioEcuador, codPais);
            if (string.IsNullOrEmpty(tel))
                return false;
            return EnviarMensajeWhatsApp(tel, textoExacto.Trim());
        }
    }
}
