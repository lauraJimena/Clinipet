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
        public ActionResult IndexVeterinario()
        {
            return View();
        }
        public ActionResult PubCitas()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    VeterinarioService veterinarioService = new VeterinarioService(); // instancia el UserService.

                    ViewBag.Dias = veterinarioService.ObtenerDiasSelect(); //Envia lista de dias a la vista
                    ViewBag.Horas = veterinarioService.ObtenerHorasSelect(); //Envia lista de horas a la vista
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "General");
                }

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return View("Error");
            }
    }
    [HttpPost]
        public ActionResult PublicarDisponibilidad(DisponibDto disponib)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    disponib.id_usu = Convert.ToInt32(Session["Id"]); // Asigna el ID del usuario logueado
                
                    VeterinarioService veterinarioService = new VeterinarioService(); 
                    DisponibDto disponibResponse = veterinarioService.PublicarDisponibilidad(disponib); // Llama al método de publicación de disponibilidad.            

                    if (disponibResponse.Response == 1)
                    {
                        //disponibilidad publicada
                        return RedirectToAction("IndexVeterinario");
                    }
                    else
                    {
                        return RedirectToAction("About", "Home");

                    }
                }
                else
                {
                    return RedirectToAction("Login", "General");
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