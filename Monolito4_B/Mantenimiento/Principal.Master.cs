using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Monolito4_B.Mantenimiento
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

            if (Session["adm"] != null)
            {

                lbl_nse.Text = Session["adm"].ToString();
                adm.visible = true;
                usu.Visible = false;

            } 
            if else ()

        }
    }
}