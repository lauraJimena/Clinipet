using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinipet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Servicios()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Error()
        {
            return View(); // Muestra Views/Home/Error.cshtml
        }
        public ActionResult AccesoDenegado()
        {
            if (Session["RolUsu"] != null) // Si el usuario tiene un rol, significa que intentó entrar a una vista sin permiso
            {
                return View();
            }

            return RedirectToAction("Index", "Home"); // Si intenta acceder manualmente, lo redirige al inicio
        }
    }
}