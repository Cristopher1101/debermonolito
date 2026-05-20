using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;

namespace capa_negocio
{
    public static class QrPngService
    {
        public static byte[] GenerarPng(string texto, int pixelesPorModulo = 8)
        {
            using (var qr = new QRCodeGenerator())
            using (QRCodeData data = qr.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(data))
            using (Bitmap bmp = qrCode.GetGraphic(pixelesPorModulo))
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
