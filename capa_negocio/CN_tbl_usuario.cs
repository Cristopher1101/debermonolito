using Capa_Datos;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace capa_negocio
{
    public class CN_tbl_usuario
    {
        /// <summary>Eager-load de perfil para listados que se enlazan a UI fuera del DataContext (evita ObjectDisposedException).</summary>
        private static void AplicarCargaConTipoUsuario(DataClasses1DataContext dc)
        {
            var lo = new DataLoadOptions();
            lo.LoadWith<tbl_usuario>(u => u.tbl_tipo_usuario);
            dc.LoadOptions = lo;
        }

        public static List<tbl_usuario> ListarUsuariosActivos()
        {
            using (var dc = new DataClasses1DataContext())
            {
                AplicarCargaConTipoUsuario(dc);
                return dc.tbl_usuario.Where(u => u.usu_estado == 'A').OrderBy(u => u.usu_apellidos).ToList();
            }
        }

        /// <summary>Cuentas para dashboards (sin cargar filas innecesarias).</summary>
        public static (int activos, int bloqueados, int total) ObtenerEstadisticas()
        {
            using (var dc = new DataClasses1DataContext())
            {
                int total = dc.tbl_usuario.Count();
                int activos = dc.tbl_usuario.Count(u => u.usu_estado == 'A');
                int bloqueados = dc.tbl_usuario.Count(u => u.usu_estado == 'T');
                return (activos, bloqueados, total);
            }
        }

        public static bool UsuarioSigueValido(int usuId)
        {
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario.Any(u => u.usu_id == usuId && u.usu_estado == 'A');
            }
        }

        public static bool autentixced(string cedula)
        {
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario.Any(u => u.usu_cedula == cedula && u.usu_estado == 'A');
            }
        }

        public static bool autentixpass(string cedula, string password)
        {
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario.Any(u =>
                    u.usu_cedula == cedula &&
                    u.usu_estado == 'A' &&
                    dc.desencriptacion(u.usu_contraseña) == password);
            }
        }

        public static tbl_usuario traterusuario(string cedula, string password)
        {
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario.FirstOrDefault(u =>
                    u.usu_cedula == cedula &&
                    dc.desencriptacion(u.usu_contraseña) == password &&
                    (u.usu_estado == 'A' || u.usu_estado == 'T'));
            }
        }

        public static void RegistrarUsuario(tbl_usuario usuario)
        {
            using (var dc = new DataClasses1DataContext())
            {
                usuario.usu_fecha_creacion = DateTime.Now;
                usuario.usu_estado = 'A';
                usuario.usu_intentos = 0;
                dc.tbl_usuario.InsertOnSubmit(usuario);
                dc.SubmitChanges();
            }
        }

        /// <summary>Usuario activo o temporalmente bloqueado (recuperación / admin).</summary>
        public static tbl_usuario traterced(string cedula)
        {
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario.FirstOrDefault(u =>
                    u.usu_cedula == cedula && (u.usu_estado == 'A' || u.usu_estado == 'T'));
            }
        }

        public static tbl_usuario ObtenerPorId(int usuId)
        {
            using (var dc = new DataClasses1DataContext())
            {
                AplicarCargaConTipoUsuario(dc);
                return dc.tbl_usuario.FirstOrDefault(u => u.usu_id == usuId);
            }
        }

        public static void modificarintentos(string cedula)
        {
            using (var dc = new DataClasses1DataContext())
            {
                var u = dc.tbl_usuario.FirstOrDefault(x => x.usu_cedula == cedula);
                if (u == null) return;

                u.usu_fecha_ultimo_intento = DateTime.Now;
                int intentos = (u.usu_intentos ?? 0) + 1;
                u.usu_intentos = intentos;
                if (intentos >= 3)
                    u.usu_estado = 'T';

                dc.SubmitChanges();
            }
        }

        public static void modificarusu(tbl_usuario usinfo)
        {
            using (var dc = new DataClasses1DataContext())
            {
                var db = dc.tbl_usuario.FirstOrDefault(x => x.usu_id == usinfo.usu_id);
                if (db == null) return;

                db.usu_cedula = usinfo.usu_cedula;
                db.usu_nombres = usinfo.usu_nombres;
                db.usu_apellidos = usinfo.usu_apellidos;
                db.usu_direccion = usinfo.usu_direccion;
                db.usu_celular = usinfo.usu_celular;
                db.usu_correo = usinfo.usu_correo;
                db.usu_fecha_cumple = usinfo.usu_fecha_cumple;
                db.usu_nick = usinfo.usu_nick;
                db.usu_intentos = usinfo.usu_intentos;
                db.usu_estado = usinfo.usu_estado;
                db.tusu_id = usinfo.tusu_id;
                db.usu_fecha_ultimo_intento = usinfo.usu_fecha_ultimo_intento;
                if (usinfo.usu_foto_perfil != null)
                    db.usu_foto_perfil = usinfo.usu_foto_perfil;

                dc.SubmitChanges();
            }
        }

        public static bool ActualizarPasswordTemporal(string cedula, string nuevaClaveTemporal)
        {
            try
            {
                using (var dc = new DataClasses1DataContext())
                {
                    var usu = dc.tbl_usuario.FirstOrDefault(u =>
                        u.usu_cedula == cedula && (u.usu_estado == 'A' || u.usu_estado == 'T'));
                    if (usu == null) return false;

                    const string query = "UPDATE tbl_usuario SET usu_contraseña = dbo.encriptacion({0}), usu_estado = 'A', usu_intentos = 0 WHERE usu_cedula = {1}";
                    dc.ExecuteCommand(query, nuevaClaveTemporal, cedula);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static string LeerOtpPlanoPorCedula(DataClasses1DataContext dc, string cedula)
        {
            const string query = @"
SELECT CASE
  WHEN usu_codigo_OTP IS NULL OR LTRIM(RTRIM(usu_codigo_OTP)) = '' THEN NULL
  WHEN LEFT(LTRIM(RTRIM(usu_codigo_OTP)), 2) = '0x'
    THEN dbo.desencriptacion(CONVERT(VARBINARY(MAX), LTRIM(RTRIM(usu_codigo_OTP)), 1))
  ELSE LTRIM(RTRIM(usu_codigo_OTP))
END
FROM tbl_usuario WHERE usu_cedula = {0}";
            return dc.ExecuteQuery<string>(query, cedula).FirstOrDefault()?.Trim();
        }

        /// <summary>
        /// Genera automáticamente un OTP numérico de 6 dígitos (RNG criptográfico) distinto del valor previo en BD si existía,
        /// y ejecuta UPDATE únicamente sobre <c>usu_codigo_OTP</c> (valor cifrado). Devuelve el código en claro para la sesión/canales.
        /// </summary>
        public static string GenerarYActualizarOtpEncriptado(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                throw new ArgumentException("La cédula es obligatoria.", nameof(cedula));

            cedula = cedula.Trim();
            using (var dc = new DataClasses1DataContext())
            {
                string anterior = LeerOtpPlanoPorCedula(dc, cedula);
                string nuevo;
                int reintentos = 0;
                do
                {
                    nuevo = GenerarOtpSeisDigitosCripto();
                    reintentos++;
                }
                while (string.Equals(nuevo, anterior, StringComparison.Ordinal) && reintentos < 64);

                const string query = "UPDATE tbl_usuario SET usu_codigo_OTP = CONVERT(VARCHAR(512), dbo.encriptacion({0}), 1) WHERE usu_cedula = {1}";
                int filas = dc.ExecuteCommand(query, nuevo, cedula);
                if (filas != 1)
                    throw new InvalidOperationException("No se actualizó el OTP: usuario no encontrado.");

                return nuevo;
            }
        }

        private static string GenerarOtpSeisDigitosCripto()
        {
            var buf = new byte[4];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                rng.GetBytes(buf);
            uint v = BitConverter.ToUInt32(buf, 0);
            int code = (int)(v % 900000U) + 100000;
            return code.ToString("D6");
        }

        public static bool VerificarOTPEncriptado(string cedula, string otpIngresado)
        {
            if (string.IsNullOrWhiteSpace(otpIngresado)) return false;

            using (var dc = new DataClasses1DataContext())
            {
                string otpDesencriptado = LeerOtpPlanoPorCedula(dc, cedula);
                return string.Equals(otpDesencriptado, otpIngresado.Trim(), StringComparison.Ordinal);
            }
        }

        public static bool VerificarOTPEncriptadoPorId(int usuId, string otpIngresado)
        {
            if (string.IsNullOrWhiteSpace(otpIngresado)) return false;

            using (var dc = new DataClasses1DataContext())
            {
                const string query = @"
SELECT CASE
  WHEN usu_codigo_OTP IS NULL OR LTRIM(RTRIM(usu_codigo_OTP)) = '' THEN NULL
  WHEN LEFT(LTRIM(RTRIM(usu_codigo_OTP)), 2) = '0x'
    THEN dbo.desencriptacion(CONVERT(VARBINARY(MAX), LTRIM(RTRIM(usu_codigo_OTP)), 1))
  ELSE LTRIM(RTRIM(usu_codigo_OTP))
END
FROM tbl_usuario WHERE usu_id = {0}";
                string otpDesencriptado = dc.ExecuteQuery<string>(query, usuId).FirstOrDefault();
                return string.Equals(otpDesencriptado?.Trim(), otpIngresado.Trim(), StringComparison.Ordinal);
            }
        }

        public static void LimpiarOTP(string cedula)
        {
            using (var dc = new DataClasses1DataContext())
            {
                const string query = "UPDATE tbl_usuario SET usu_codigo_OTP = NULL, usu_intentos = 0 WHERE usu_cedula = {0}";
                dc.ExecuteCommand(query, cedula);
            }
        }

        public static void LimpiarOTPporId(int usuId)
        {
            using (var dc = new DataClasses1DataContext())
            {
                const string query = "UPDATE tbl_usuario SET usu_codigo_OTP = NULL, usu_intentos = 0 WHERE usu_id = {0}";
                dc.ExecuteCommand(query, usuId);
            }
        }

        public static List<tbl_usuario> ListarUsuariosBloqueados()
        {
            using (var dc = new DataClasses1DataContext())
            {
                AplicarCargaConTipoUsuario(dc);
                return dc.tbl_usuario
                    .Where(u => u.usu_estado == 'T')
                    .OrderBy(u => u.usu_apellidos)
                    .ToList();
            }
        }

        public static bool DesbloquearUsuario(int usuId)
        {
            try
            {
                using (var dc = new DataClasses1DataContext())
                {
                    var u = dc.tbl_usuario.FirstOrDefault(x => x.usu_id == usuId);
                    if (u == null) return false;
                    u.usu_estado = 'A';
                    u.usu_intentos = 0;
                    u.usu_fecha_ultimo_intento = null;
                    dc.SubmitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void EstablecerContraseñaEncriptadaPorId(int usuId, string passwordPlano)
        {
            using (var dc = new DataClasses1DataContext())
            {
                const string query = "UPDATE tbl_usuario SET usu_contraseña = dbo.encriptacion({0}) WHERE usu_id = {1}";
                dc.ExecuteCommand(query, passwordPlano, usuId);
            }
        }

        /// <summary>Listado completo para administración (todos los estados).</summary>
        public static List<tbl_usuario> ListarTodosParaAdmin()
        {
            using (var dc = new DataClasses1DataContext())
            {
                AplicarCargaConTipoUsuario(dc);
                return dc.tbl_usuario
                    .OrderBy(u => u.usu_apellidos)
                    .ThenBy(u => u.usu_nombres)
                    .ToList();
            }
        }

        public static bool ExisteCedulaOtroUsuario(int usuIdExcluir, string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula)) return false;
            cedula = cedula.Trim();
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario.Any(u => u.usu_id != usuIdExcluir && u.usu_cedula == cedula);
            }
        }

        public static bool ExisteNickOtroUsuario(int usuIdExcluir, string nick)
        {
            if (string.IsNullOrWhiteSpace(nick)) return false;
            nick = nick.Trim();
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario.Any(u => u.usu_id != usuIdExcluir && u.usu_nick == nick);
            }
        }

        public static bool ExisteNick(string nick)
        {
            if (string.IsNullOrWhiteSpace(nick)) return false;
            nick = nick.Trim();
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario.Any(u => u.usu_nick == nick);
            }
        }

        public static bool EliminarUsuarioPorId(int usuId)
        {
            try
            {
                using (var dc = new DataClasses1DataContext())
                {
                    var u = dc.tbl_usuario.FirstOrDefault(x => x.usu_id == usuId);
                    if (u == null) return false;
                    dc.tbl_usuario.DeleteOnSubmit(u);
                    dc.SubmitChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Fotos adicionales (no principal) para galería.</summary>
        public static List<tbl_usuario_foto> ListarFotosGaleriaNoPrincipal(int usuId)
        {
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario_foto
                    .Where(f => f.usu_id == usuId && (f.ufoto_es_principal != true))
                    .OrderBy(f => f.ufoto_id)
                    .ToList();
            }
        }

        public static int ContarFotosGaleriaNoPrincipal(int usuId)
        {
            using (var dc = new DataClasses1DataContext())
            {
                return dc.tbl_usuario_foto.Count(f => f.usu_id == usuId && (f.ufoto_es_principal != true));
            }
        }

        public static void InsertarFotosGaleria(int usuId, IEnumerable<byte[]> imagenes)
        {
            if (imagenes == null) return;
            using (var dc = new DataClasses1DataContext())
            {
                foreach (var raw in imagenes)
                {
                    if (raw == null || raw.Length == 0) continue;
                    dc.tbl_usuario_foto.InsertOnSubmit(new tbl_usuario_foto
                    {
                        usu_id = usuId,
                        ufoto_datos = new Binary(raw),
                        ufoto_es_principal = false
                    });
                }
                dc.SubmitChanges();
            }
        }

        /// <summary>Elimina una fila de galería si pertenece al usuario y no es la foto principal.</summary>
        public static bool EliminarFotoGaleriaNoPrincipal(int ufotoId, int usuIdDueño)
        {
            try
            {
                using (var dc = new DataClasses1DataContext())
                {
                    var f = dc.tbl_usuario_foto.FirstOrDefault(x =>
                        x.ufoto_id == ufotoId &&
                        x.usu_id == usuIdDueño &&
                        (x.ufoto_es_principal != true));
                    if (f == null) return false;
                    dc.tbl_usuario_foto.DeleteOnSubmit(f);
                    dc.SubmitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Genera OTP en BD y URL absoluta del QR de acceso (OtpQr.aspx?t=...).</summary>
        public static string ConstruirUrlAbsolutaAccesoQr(System.Web.HttpRequest request, int usuId, string cedula, int validezMinutos)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            cedula = (cedula ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(cedula))
                throw new ArgumentException("La cédula es obligatoria.", nameof(cedula));

            string otpPlano = GenerarYActualizarOtpEncriptado(cedula);
            DateTime expUtc = DateTime.UtcNow.AddMinutes(validezMinutos);
            string token = OtpQrTokenHelper.CrearToken(usuId, otpPlano, expUtc);
            string path = VirtualPathUtility.ToAbsolute("~/Seguridad/OtpQr.aspx");
            string autoridad = request.Url.GetLeftPart(UriPartial.Authority);
            return autoridad + path + "?t=" + HttpUtility.UrlEncode(token);
        }

        /// <summary>
        /// URL absoluta del QR de OTP para correo, usando el mismo código ya generado en BD (no vuelve a llamar a GenerarYActualizarOtpEncriptado).
        /// </summary>
        public static string ConstruirUrlAbsolutaOtpQrCorreo(HttpRequest request, int usuId, string otpPlano, DateTime expiraUtc)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (usuId <= 0)
                throw new ArgumentException("Identificador de usuario inválido.", nameof(usuId));
            if (string.IsNullOrWhiteSpace(otpPlano))
                throw new ArgumentException("El OTP es obligatorio.", nameof(otpPlano));

            DateTime exp = expiraUtc.Kind == DateTimeKind.Utc ? expiraUtc : expiraUtc.ToUniversalTime();
            string token = OtpQrTokenHelper.CrearToken(usuId, otpPlano.Trim(), exp);
            string path = VirtualPathUtility.ToAbsolute("~/Seguridad/OtpQr.aspx");
            string autoridad = request.Url.GetLeftPart(UriPartial.Authority);
            return autoridad + path + "?t=" + HttpUtility.UrlEncode(token);
        }

        public static byte[] GenerarPngQrAcceso(System.Web.HttpRequest request, int usuId, string cedula, int validezMinutos, out string urlCodificada)
        {
            urlCodificada = ConstruirUrlAbsolutaAccesoQr(request, usuId, cedula, validezMinutos);
            return QrPngService.GenerarPng(urlCodificada, 6);
        }
    }
}
