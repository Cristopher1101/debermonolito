using System;
using System.IO;
using System.Linq;

namespace capa_negocio
{
    /// <summary>Reglas comunes de imágenes (servidor): solo JPG/PNG reales, máx. 2 MB.</summary>
    public static class FotoSubidaReglas
    {
        public const int TamanoMaximoBytes = 2 * 1024 * 1024;

        public static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png" };

        public static bool ExtensionPermitida(string nombreArchivo)
        {
            string ext = Path.GetExtension(nombreArchivo)?.ToLowerInvariant() ?? "";
            return ExtensionesPermitidas.Contains(ext);
        }

        /// <summary>JPEG (FF D8 FF) o PNG (89 50 4E 47 0D 0A 1A 0A).</summary>
        public static bool EsJpegOPngPorFirma(byte[] datos, out string mimeDetectado)
        {
            mimeDetectado = null;
            if (datos == null || datos.Length < 8)
                return false;

            if (datos.Length >= 3 && datos[0] == 0xFF && datos[1] == 0xD8 && datos[2] == 0xFF)
            {
                mimeDetectado = "image/jpeg";
                return true;
            }

            if (datos[0] == 0x89 && datos[1] == 0x50 && datos[2] == 0x4E && datos[3] == 0x47
                && datos[4] == 0x0D && datos[5] == 0x0A && datos[6] == 0x1A && datos[7] == 0x0A)
            {
                mimeDetectado = "image/png";
                return true;
            }

            return false;
        }

        public static bool ValidarArchivoFoto(string nombreArchivo, int contentLength, byte[] contenido, out string mime, out string error)
        {
            mime = null;
            error = null;

            if (string.IsNullOrEmpty(nombreArchivo))
            {
                error = "No se indicó nombre de archivo.";
                return false;
            }

            if (!ExtensionPermitida(nombreArchivo))
            {
                error = "Solo se permiten imágenes .jpg, .jpeg o .png.";
                return false;
            }

            if (contenido == null || contenido.Length == 0)
            {
                error = "El archivo está vacío o no se pudo leer.";
                return false;
            }

            if (contentLength > TamanoMaximoBytes || contenido.Length > TamanoMaximoBytes)
            {
                error = "El archivo supera el máximo de 2 MB.";
                return false;
            }

            if (!EsJpegOPngPorFirma(contenido, out string firmaMime))
            {
                error = "El contenido no es una imagen JPG o PNG válida.";
                return false;
            }

            string ext = Path.GetExtension(nombreArchivo)?.ToLowerInvariant() ?? "";
            if (ext == ".png" && firmaMime != "image/png")
            {
                error = "La extensión no coincide con el contenido (se esperaba PNG).";
                return false;
            }

            if ((ext == ".jpg" || ext == ".jpeg") && firmaMime != "image/jpeg")
            {
                error = "La extensión no coincide con el contenido (se esperaba JPG).";
                return false;
            }

            mime = firmaMime;
            return true;
        }
    }
}
