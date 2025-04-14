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
        public ActionResult ListadoMascotas()
        {

            

            return View();
        }

        [HttpPost]
        public ActionResult ListadoMascotas(int id_usu, int id_dispon)
        {

            ClienteService clienteService = new ClienteService();
            List<MascotaDto> mascotas = clienteService.ListadoMascotas(id_usu);

            var modelo = new MascotaCitaDto //variable modelo de tipo MascotaCitaDto
            {
                Mascotas = mascotas, //Se asigna la lista para mostrar en la vista
                IdDispon = id_dispon,
                IdUsu = id_usu
            };

            return View(modelo);
        }
        public ActionResult AgendarCita(int id_dispon, int id_mascota, int id_usu)
        {

            ClienteService clienteService = new ClienteService();
            MascotaDto mascota = clienteService.ObtenerMascotaPorId(id_mascota);
            DisponibDto dispon = clienteService.obtenerDisponPorId(id_dispon);
            //System.Diagnostics.Debug.WriteLine(dispon.nom_usu);

            if (dispon == null)
            {
                return HttpNotFound("No se encontró la cita.");
            }
            var modelo = new MascotaCitaDto
            {
                Mascota = mascota,
                Disponib = dispon,
                IdUsu = id_usu
            };

            return View(modelo);

        }

        [HttpPost]
        public ActionResult ConfirmarCita()
        {

            return RedirectToAction("IndexCliente", "Cliente");
        }




    }
}