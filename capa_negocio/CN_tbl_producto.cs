using Capa_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capa_negocio
{
    public class CN_tbl_producto
    {
        private static DataClasses1DataContext dc = new DataClasses1DataContext();

        public static List<tbl_producto> traerproductos()
        {
            return dc.tbl_producto.Where(p => p.pro_estado == 'A').ToList();
        }


        public static tbl_producto Traerproductosxid(int id)
        {
            return dc.tbl_producto.FirstOrDefault(p => p.pro_id == id && p.pro_estado == 'A');
        }

        public static List<tbl_producto> buscarporductopornombre(string nombre)
        {
            return dc.tbl_producto.Where(p => p.pro_nombre.Contains(nombre) && p.pro_estado == 'A').ToList();
        }

        public static void modify (tbl_producto producto) {
            try
            {
                dc.SubmitChanges();

            }
            catch (Exception)
            {
                throw;
            }
        
        }

        public static void save(tbl_producto producto)
        {

            try
            {
                producto.pro_estado = 'A';
                dc.tbl_producto.InsertOnSubmit(producto);
                dc.SubmitChanges();

            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}
