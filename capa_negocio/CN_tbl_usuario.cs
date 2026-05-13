using Capa_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capa_negocio
{
    public class CN_tbl_usuario
    {
        private static DataClasses1DataContext dc = new DataClasses1DataContext();

        public static List<tbl_usuario> ListarUsuarios()
        {
            return dc.tbl_usuario.Where(u => u.tusu_id == 'A').ToList();
        }

        public static bool autentixced(string cedula)
        {
            return dc.tbl_usuario.Any(u => u.usu_cedula == cedula && u.usu_estado == 'A');
        }

        public static bool autentixpass(string cedula, string password)
        {

            return dc.tbl_usuario.Any(u => u.usu_cedula == cedula && dc.desencriptacion(u.usu_contraseña) == password && u.usu_estado == 'A');
        }



        public static tbl_usuario traterusuario(string cedula, string password)
        {
            return dc.tbl_usuario.FirstOrDefault(u => u.usu_cedula == cedula && dc.desencriptacion(u.usu_contraseña) == password && (u.usu_estado == 'A' || u.usu_estado == 'T'));

        }

        public static void RegistrarUsuario(tbl_usuario usuario)
        {
            usuario.usu_fecha_creacion = DateTime.Now;
            usuario.usu_estado = 'A';
            dc.tbl_usuario.InsertOnSubmit(usuario);
            dc.SubmitChanges();
        }

        public static tbl_usuario traterced(string cedula)
        {
            return dc.tbl_usuario.FirstOrDefault(u => u.usu_cedula == cedula && (u.usu_estado == 'A' || u.usu_estado == 'T'));

        }

        public static void modificarintentos(string cedula, string password)
        {
            dc.SubmitChanges();

        }

        public static void modificarusu(tbl_usuario usinfo)
        {
            dc.SubmitChanges();
        }
    }
}
