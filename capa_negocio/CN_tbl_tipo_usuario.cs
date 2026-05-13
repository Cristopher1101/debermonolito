using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using Capa_Datos;

namespace capa_negocio
{
    public class CN_tbl_tipo_usuario
    {
        private static DataClasses1DataContext dc = new DataClasses1DataContext();

        //consulta que trae toda la informacion de la tabla tipo usuario 

        public static List<tbl_tipo_usuario> ListarTipoUsuario()
        {
            using (var dc = new DataClasses1DataContext())
            {

                return dc.tbl_tipo_usuario.Where(tu => tu.tusu_estado == 'A').ToList();

             
            }
        }
    }
}
