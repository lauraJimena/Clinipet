using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clinipet.Dtos;
using Clinipet.Services;
using Clinipet.Repositories;

namespace Clinipet.Controllers
{
    public class AsistenteController : Controller
    {
        // GET: Asistente
        public ActionResult IndexAsistente()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "General");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public JsonResult RegistroUsuario(UserDto usuario)
        {
            var servicio = new AsistenteService(); 
            var resultado = servicio.RegistrarUsuario(usuario);

            return Json(new
            {
                success = resultado.Response == 1,
                message = resultado.Mensaje
            });
        }

        [HttpGet]
        public ActionResult RegistroUsuario()
        {
            return View(new UserDto());
        }

        public ActionResult RegistroMascota()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    AsistenteService asistenteService = new AsistenteService();
                    ViewBag.Razas = asistenteService.ObtenerRazasSelect();
                    ViewBag.Tipos = asistenteService.ObtenerTiposSelect();
                    ViewBag.Clientes = asistenteService.ObtenerClientesSelect();

                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "General");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Ocurrió un error al cargar la vista: " + ex.Message;
                TempData["Exito"] = false;
                return View(); 
            }
        }

        [HttpPost]
        public ActionResult RegistroMascota(MascotaDto mascota)
        {
            try
            {
                if (Session["UsuLoguedo"] == null)
                {
                    return RedirectToAction("Login", "General");
                }

                AsistenteService asistenteService = new AsistenteService();
                var resultado = asistenteService.RegistrarMascota(mascota);

                TempData["Mensaje"] = resultado.Mensaje;
                TempData["Exito"] = resultado.Response == 1;

                return RedirectToAction("RegistroMascota");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Ocurrió un error al registrar la mascota: " + ex.Message;
                TempData["Exito"] = false;
                return RedirectToAction("RegistroMascota");
            }
        }

        public ActionResult ConsultarCitas(int? idDia)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    AsistenteService asistenteService = new AsistenteService();
                    List<CitaEspecVistaDto> citas = asistenteService.ObtenerCitasEspecializadas();

                    if (idDia.HasValue)
                    {
                        citas = asistenteService.ObtenerCitasPorDia(idDia.Value);
                    }
                    else
                    {
                        citas = asistenteService.ObtenerCitasEspecializadas();
                    }

                    return View(citas);
                }
                else
                {
                    return RedirectToAction("Login", "General");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al cargar la agenda de citas: " + ex.Message;
                return View(new List<CitaEspecVistaDto>());
            }
        }

        [HttpPost]
        public ActionResult CambiarEstadoCita(int id_cita_esp, int nuevo_estado)
        {
            try
            {
                AsistenteService asistenteService = new AsistenteService();
                asistenteService.ActualizarEstadoCita(id_cita_esp, nuevo_estado);
                return RedirectToAction("ConsultarCitas");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cambiar el estado de la cita: " + ex.Message;
                return RedirectToAction("ConsultarCitas");
            }
        }

        
    }
}