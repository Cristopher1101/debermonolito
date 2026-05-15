using System;
using System.Text;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace Monolito4_B
{
    public class Global : HttpApplication
    {
        void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
        }

        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex == null) return;

            try
            {
                string ruta = Server.MapPath("~/App_Data/last_error.log");
                System.IO.File.AppendAllText(ruta,
                    $"{DateTime.UtcNow:o}\t{ex.GetType().Name}\t{ex.Message}\r\n{ex.StackTrace}\r\n---\r\n");
            }
            catch
            {
                // Si no se puede escribir log, no romper el pipeline.
            }

            HttpException httpEx = ex as HttpException;
            if (httpEx != null && httpEx.GetHttpCode() == 404)
                return;
        }
    }
}
