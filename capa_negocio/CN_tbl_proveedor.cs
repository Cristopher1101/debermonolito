using Capa_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capa_negocio
{
    public class CN_tbl_proveedor
    {
        private static DataClasses1DataContext dc = new DataClasses1DataContext();

        public static List <tbl_proveedor> traerproveedores()
        {
            return dc.tbl_proveedor.Where(p => p.prov_estado == 'A').ToList();
        }

        private void cargar_proveedores()
        {
                var proveedores = dc.tbl_proveedor.Where(p => p.prov_estado == 'A').ToList();
                // Aquí puedes hacer algo con la lista de proveedores, como asignarla a una propiedad o devolverla
        }

    }
}
