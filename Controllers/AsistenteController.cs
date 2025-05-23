﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clinipet.Dtos;
using Clinipet.Services;
using Clinipet.Repositories;
using Clinipet.Utilities;

namespace Clinipet.Controllers
{
    public class AsistenteController : BaseController
    {
        // GET: Asistente
        [ValidarRolUtility(2)] //Validación para que solo rol 2 (asistente) acceda a la vista
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
        [ValidarRolUtility(2)] 
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
        [ValidarRolUtility(2)]
        public ActionResult RegistroUsuario()
        {
            try
            {
                return View(new UserDto());
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        [ValidarRolUtility(2)]
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
        [ValidarRolUtility(2)]
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

        [ValidarRolUtility(2)]
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
        [ValidarRolUtility(2)]
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

        [ValidarRolUtility(2)]
        public ActionResult ServiciosGenerales()
        {
            try
            {

                if (Session["UsuLoguedo"] != null)
                {
                    AsistenteService asisService = new AsistenteService();
                    List<ServicioDto> servicioGeneral = asisService.ListadoServiciosGenerales();
                    return View(servicioGeneral);
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

        [ValidarRolUtility(2)]
        public ActionResult CitasGenDispon()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    return RedirectToAction("IndexAsistente", "Asistente");
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

        [ValidarRolUtility(2)]
        public ActionResult ListadoMascotas()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    return RedirectToAction("IndexAsistente", "Asistente");
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

        [ValidarRolUtility(2)]
        public ActionResult ElegirMascota()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    return RedirectToAction("IndexAsistente", "Asistente");
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
        [ValidarRolUtility(2)]
        public ActionResult CitasGenDispon(int? id_servicio)
        {
            if (Session["UsuLoguedo"] == null)
            {
                return RedirectToAction("Login", "General");
            }
            try
            {
                if (id_servicio == null)
                {
                    throw new Exception("ID de servicio no proporcionado.");
                }

                AsistenteService asisService = new AsistenteService();
                List<DisponibDto> citasGenDispon = asisService.ObtenerCitasGenDispon(id_servicio.Value);
                return View(citasGenDispon);

            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidarRolUtility(2)]
        public ActionResult ListadoMascotas(int id_usu, int id_dispon)
        {

            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    AsistenteService asisService = new AsistenteService();
                    List<MascotaDto> mascotas = asisService.ListadoMascotas(id_usu);

                    var modelo = new MascotaCitaDto //variable modelo de tipo MascotaCitaDto
                    {
                        Mascotas = mascotas, //Se asigna la lista para mostrar en la vista
                        IdDispon = id_dispon,
                        IdUsu = id_usu
                    };

                    return View(modelo);
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
        [ValidarRolUtility(2)]
        public ActionResult AgendarCita(int id_dispon, int id_mascota, int id_usu)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    AsistenteService asisService = new AsistenteService();
                    MascotaDto mascota = asisService.ObtenerMascotaPorId(id_mascota);
                    DisponibDto dispon = asisService.obtenerDisponPorId(id_dispon);

                    var modelo = new MascotaCitaDto
                    {
                        Mascota = mascota,
                        Disponib = dispon,
                        IdUsu = id_usu
                    };

                    return View(modelo);
                }
                else
                {
                    return RedirectToAction("Login", "General");
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return View("Error");  // Muestra la vista en caso de que ocurra una excepción.
            }

        }

        [HttpPost]
        [ValidarRolUtility(2)]
        public ActionResult ConfirmarCitaGeneral(CitaGeneralDto nuevaCita)
        {

            try
            {
                AsistenteService asiseService = new AsistenteService(); // instancia el UserService.
                CitaGeneralDto citaGenResponse = asiseService.AgendarCitaGeneral(nuevaCita); // Llama al método de creación de usuario.

                if (citaGenResponse.Response == 1)
                {
                    return Json(new { success = true, redirectUrl = Url.Action("IndexAsistente") });

                }

                else
                {

                    return Json(new { success = false, message = "No se pudo confirmar la cita." });
                }

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidarRolUtility(2)]
        public ActionResult ElegirMascota(int id_usu, int id_dispon, int id_servicio, string nombreMascota = null, string cedulaDueno = null)
        {
            try
            {

                if (Session["UsuLoguedo"] != null)
                {

                    AsistenteService asisService = new AsistenteService();
                    List<MascotaDto> mascotas = new List<MascotaDto>();
                    System.Diagnostics.Debug.WriteLine($"NombreMascota: {nombreMascota}, CedulaDueno: {cedulaDueno}");

                    if (!string.IsNullOrEmpty(nombreMascota) || !string.IsNullOrEmpty(cedulaDueno))
                    {
                        mascotas = asisService.BuscarMascotas(nombreMascota, cedulaDueno);
                    }
                    else
                    {
                        mascotas = asisService.ListadoTodasMascotas(); // mostrar todas si no hay filtros
                    }

                    var modelo = new MascotaCitaDto //variable modelo de tipo MascotaCitaDto
                    {
                        Mascotas = mascotas, //Se asigna la lista para mostrar en la vista
                        IdDispon = id_dispon,
                        IdUsu = id_usu,
                        IdServicio = id_servicio
                    };

                    ViewBag.NombreMascota = nombreMascota;
                    ViewBag.CedulaDueno = cedulaDueno;

                    return View(modelo);
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
                //return Content("Error: " + ex.Message);
            }
        }

        [ValidarRolUtility(2)]
        public ActionResult BuscarMascotas()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    return View(new List<MascotaDto>()); // vista vacía inicial
                }
                else
                {
                    return RedirectToAction("Login", "General");
                }
            }
            catch (Exception ex)
            {
                //TempData["Mensaje"] = "Ocurrió un error al cargar la vista: " + ex.Message;
                //TempData["Exito"] = false;
                System.Diagnostics.Debug.WriteLine(">>> ERROR ElegirMascota: " + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View(new List<MascotaDto>());
            }
        }

        [HttpPost]
        [ValidarRolUtility(2)]
        public ActionResult BuscarMascotas(string nombreMascota, string cedulaDueno)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    AsistenteService asistenteService = new AsistenteService();
                    var mascotas = asistenteService.BuscarMascotas(nombreMascota, cedulaDueno);
                    return View(mascotas);
                }
                else
                {
                    return RedirectToAction("Login", "General");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Ocurrió un error al buscar las mascotas: " + ex.Message;
                TempData["Exito"] = false;
                return View(new List<MascotaDto>());
            }
        }
        [ValidarRolUtility(2)]
        public ActionResult PublicarDisponGen()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    VeterinarioService veterinarioService = new VeterinarioService(); // instancia el UserService.

                    ViewBag.Dias = veterinarioService.ObtenerDiasSelect(); //Envia lista de dias a la vista
                    ViewBag.Horas = veterinarioService.ObtenerHorasSelect(); //Envia lista de horas a la vista
                    ViewBag.Servicios = veterinarioService.ObtenerServiciosGenSelect(); //Envia lista de horas a la vista
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
        [ValidarRolUtility(2)]
        public ActionResult PublicarDisponGen(DisponibDto disponib)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    disponib.id_usu = Convert.ToInt32(Session["Id"]); // Asigna el ID del usuario logueado

                    AsistenteService asistService = new AsistenteService();
                    DisponibDto disponibRespuesta = asistService.PublicarDisponGen(disponib); // Llama al método de publicación de disponibilidad.            

                    if (disponibRespuesta.Response == 1)
                    {
                        return Json(new
                        {
                            success = true,
                            redirectUrl = Url.Action("IndexAsistente", "Asistente"),
                            message = "Guardado exitosamente"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "No se pudo guardar la disponibilidad. Inténtalo nuevamente."
                        });

                    }
                }
                else
                {
                    return RedirectToAction("Login", "General");
                }

            }
            catch (Exception ex)
            {
                // Captura cualquier excepción y la devuelve al cliente
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }

        }

    }
}