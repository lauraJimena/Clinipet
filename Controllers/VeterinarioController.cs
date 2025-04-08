using Clinipet.Dtos;
using Clinipet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinipet.Controllers
{
    public class VeterinarioController : Controller
    {
        // GET: Veterinario
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PubCitas()
        {
            VeterinarioService veterinarioService = new VeterinarioService(); // instancia el UserService.
            
            ViewBag.Dias = veterinarioService.ObtenerDiasSelect(); //Envia lista de dias a la vista
            ViewBag.Horas = veterinarioService.ObtenerHorasSelect(); //Envia lista de horas a la vista
            return View();
        }
        [HttpPost]
        public ActionResult PublicarDisponibilidad(DisponibDto disponib)
        {
            try
            {
               
                disponib.id_usu = Convert.ToInt32(Session["Id"]); // Asigna el ID del usuario logueado
                VeterinarioService veterinarioService = new VeterinarioService(); // instancia el UserService.
                DisponibDto disponibResponse = veterinarioService.PublicarDisponibilidad(disponib); // Llama al método de creación de usuario.            

                if (disponibResponse.Response == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("About", "Home");

                }


            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return View(); // Muestra la vista en caso de que ocurra una excepción.
            }

        }
        [HttpPost]
        public ActionResult Testeo()
        {
            
            Console.WriteLine("POR FAVORRRRRR");
            return Content("¡Entró al POST!");
        }
    }
    

}