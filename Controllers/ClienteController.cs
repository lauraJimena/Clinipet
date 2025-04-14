using Clinipet.Dtos;
using Clinipet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinipet.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult IndexCliente()
        {
            return View();
        }
        public ActionResult CitasEspecDispon()
        {
            GeneralService generalService = new GeneralService();
            List<DisponibDto> citas = generalService.ObtenerCitasDispon();

            return View(citas);
        }
        public ActionResult AgendarCita(int id_dispon)
        {

            ClienteService clienteService = new ClienteService();

            DisponibDto dispon = clienteService.obtenerDisponPorId(id_dispon);
            //System.Diagnostics.Debug.WriteLine(dispon.nom_usu);

            if (dispon == null)
            {
                return HttpNotFound("No se encontró la cita.");
            }

            return View(dispon);

        }
        [HttpPost]
        public ActionResult ConfirmarCita()
        {

            return RedirectToAction("IndexCliente", "Cliente");
        }




    }
}