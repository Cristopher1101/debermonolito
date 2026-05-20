using System;
using System.Web;

namespace capa_negocio
{
    /// <summary>Evita que queden datos de acceso en cookies del navegador (versiones anteriores de la app).</summary>
    public static class BrowserSensitiveCookies
    {
        public const string LegacyRememberCedula = "Monolito4B_RememberCed";

        public static void ExpirarLegacyRememberCedula(HttpResponse response)
        {
            if (response?.Cookies == null)
                return;
            var ck = new HttpCookie(LegacyRememberCedula, string.Empty)
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                Path = "/"
            };
            response.Cookies.Set(ck);
        }
    }
}
