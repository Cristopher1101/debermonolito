using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace capa_negocio
{
    /// <summary>
    /// Recupera todos los archivos de un FileUpload con AllowMultiple.
    /// Combina PostedFiles y Request.Files; maneja claves nulas en multipart y evita deduplicar
    /// por nombre+tamaño (imágenes distintas con el mismo nombre y peso se perdían).
    /// </summary>
    public static class FileUploadMultiplesHelper
    {
        private static bool ClaveCoincideControl(string key, FileUpload fu)
        {
            if (string.IsNullOrEmpty(key) || fu == null)
                return false;
            if (key.Equals(fu.UniqueID, System.StringComparison.OrdinalIgnoreCase))
                return true;
            if (key.IndexOf(fu.UniqueID, System.StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            string id = fu.ID ?? string.Empty;
            if (!string.IsNullOrEmpty(id) && key.EndsWith("$" + id, System.StringComparison.OrdinalIgnoreCase))
                return true;
            string clientId = fu.ClientID ?? string.Empty;
            if (!string.IsNullOrEmpty(clientId))
            {
                if (key.Equals(clientId, System.StringComparison.OrdinalIgnoreCase))
                    return true;
                string alt = clientId.Replace('_', '$');
                if (key.Equals(alt, System.StringComparison.OrdinalIgnoreCase))
                    return true;
                if (key.EndsWith("$" + id, System.StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static void AgregarPorReferencia(List<HttpPostedFile> list, HttpPostedFile f)
        {
            if (f == null || f.ContentLength <= 0)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                if (ReferenceEquals(list[i], f))
                    return;
            }
            list.Add(f);
        }

        public static List<HttpPostedFile> EnumerarArchivos(FileUpload fu, HttpRequest request)
        {
            var list = new List<HttpPostedFile>();
            if (fu == null || request?.Files == null)
                return list;

            try
            {
                foreach (HttpPostedFile f in fu.PostedFiles)
                    AgregarPorReferencia(list, f);
            }
            catch
            {
                // continuar con Request.Files
            }

            HttpFileCollection all = request.Files;
            string lastKeyQueCoincideGaleria = null;

            for (int i = 0; i < all.Count; i++)
            {
                HttpPostedFile f = all[i];
                if (f == null || f.ContentLength <= 0)
                    continue;

                string key = all.GetKey(i) ?? string.Empty;
                if (!string.IsNullOrEmpty(key))
                {
                    lastKeyQueCoincideGaleria = ClaveCoincideControl(key, fu) ? key : null;
                }

                if (lastKeyQueCoincideGaleria == null)
                    continue;

                if (string.IsNullOrEmpty(key) || ClaveCoincideControl(key, fu))
                    AgregarPorReferencia(list, f);
            }

            return list;
        }
    }
}
