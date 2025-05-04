using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinipet.Controllers
{
    // Este controlador base configura cabeceras HTTP para evitar que el navegador o servidor almacenen en caché las vistas protegidas,
    // especialmente después de cerrar sesión. Los controladores que hereden de este evitarán que se pueda volver a páginas protegidas con el botón "Atrás".
    public class BaseController : Controller 
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {   
            base.OnActionExecuting(filterContext);

            //  Evitan completamente que la vista protegida se quede guardada en caché
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));   // Establece una fecha de expiración en el pasado para que el navegador considere la página como caducada.
            Response.Cache.SetValidUntilExpires(false);  // Indica que no se debe confiar en la expiración: el navegador debe verificar siempre con el servidor.        
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);    // Obliga a todos los cachés intermedios (como proxies o firewalls) a revalidar la página con el servidor.
            Response.Cache.SetCacheability(HttpCacheability.NoCache); // Indica al navegador que no debe guardar esta página en la caché.
            Response.Cache.SetNoStore(); // No guardar en ningún tipo de almacenamiento

            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Expires", "-1");
        }
    }
}