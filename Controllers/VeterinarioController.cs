using Clinipet.Dtos;
using Clinipet.Services;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinipet.Controllers
{
    public class VeterinarioController : BaseController
    {
        // GET: Veterinario
        [ValidarRolUtility(4)] //Validación para que solo rol 4 (veterinario) acceda a la vista
        public ActionResult IndexVeterinario()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    UserDto usu = Session["UsuLoguedo"] as UserDto;
                    return View(usu);
                }
                else
                {
                    return RedirectToAction("Login", "General");
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return View("Error");  // Muestra la vista Error en caso de que ocurra una excepción.
            }
        }

        [ValidarRolUtility(4)]
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
        [ValidarRolUtility(4)]
        public ActionResult PublicarDisponibilidad(DisponibDto disponib)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    disponib.id_usu = Convert.ToInt32(Session["Id"]); // Asigna el ID del usuario logueado
                
                    VeterinarioService veterinarioService = new VeterinarioService(); 
                    DisponibDto disponibRespuesta = veterinarioService.PublicarDisponibilidad(disponib); // Llama al método de publicación de disponibilidad.            

                    if (disponibRespuesta.Response == 1)
                    {
                        return Json(new { success = true, redirectUrl = Url.Action("IndexVeterinario", "Veterinario"), message = "Guardado exitosamente" });
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

                return View("Error"); 
            }

        }

        [ValidarRolUtility(4)]
        public ActionResult BuscarCliente()
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
                string mensaje = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidarRolUtility(4)]
        public ActionResult BuscarCliente(string num_ident)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    VeterinarioService veterinarioService = new VeterinarioService();
                    UserDto usu = veterinarioService.ObtenerUsuarioPorNumIdent(num_ident);
                    if (usu==null)
                    {
                        TempData["Error"] = "El numero de documento ingresado no esta registrado.";
                        return RedirectToAction("BuscarCliente");
                    }
                    else
                    {
                        List<MascotaDto> mascotas = veterinarioService.ListadoMascotas(num_ident);
                        return View("ElegirMascDescrip", mascotas);
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
                return View("Error");
            }
        }

        [ValidarRolUtility(4)]
        public ActionResult ElegirMascDescrip()
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
                string mensaje = ex.Message;
                return View(); // Muestra la vista en caso de que ocurra una excepción.
            }

        }

        [HttpPost]
        [ValidarRolUtility(4)]
        public ActionResult ElegirCitaEspec(int id_mascota)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    
                    
                    int id_usu= (int)Session["Id"];
                    VeterinarioService veterinarioService = new VeterinarioService();
                    List<CitaEspecDto> citas = veterinarioService.ObtenerCitasEspecAgend(id_usu, id_mascota);
                           
                    return View(citas);

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

        [ValidarRolUtility(4)]
        public ActionResult HistorialCitas()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {

                    int id_usu = (int)Session["Id"];
                    VeterinarioService veterinarioService = new VeterinarioService();
                    List<CitaEspecDto> citas = veterinarioService.ObtenerHistorialCitas(id_usu);
                    List<DisponibDto> disponib = veterinarioService.ObtenerDisponib(id_usu);

                    var viewModel = new HistorialCitasDto
                    {
                        Citas = citas,
                        Disponib = disponib
                    };

                    return View(viewModel);

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

        [ValidarRolUtility(4)]
        public ActionResult HistorialMascota(string num_ident)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    //int id_mascota = 1;
                    int id_usu = (int)Session["Id"];
                    VeterinarioService veterinarioService = new VeterinarioService();
                    List<CitaEspecDto> citas = veterinarioService.ObtenerHistorialMascota(id_usu);
                    if (!string.IsNullOrEmpty(num_ident))
                    {
                        citas = citas.Where(c => c.num_ident == num_ident).ToList(); // Filtrar por documento
                                                                                     // Si no hay resultados, enviar una bandera a la vista
                                                                                     // Si no hay resultados, enviamos NULL en lugar de una lista vacía
                        if (!citas.Any())
                        {
                            ViewBag.DocumentoNoEncontrado = true;
                            return View(new List<CitaEspecDto>()); // Esto hará que la vista detecte la ausencia de datos
                        }
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
                string mensaje = ex.Message;
                return View(); // Muestra la vista en caso de que ocurra una excepción.
            }

        }

        [ValidarRolUtility(4)]
        public ActionResult ObtenerMascotas()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                   
                    int id_usu = (int)Session["Id"];
                    VeterinarioService veterinarioService = new VeterinarioService();
                    List<MascotaDto> mascotas = veterinarioService.ObtenerMascotas(id_usu);
                    return View(mascotas);

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

        [ValidarRolUtility(4)]
        public ActionResult DescripConsulta()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    int id_cita_esp = (int)(TempData["id_cita_esp"] ?? 0);
                    if (id_cita_esp == 0)
                    {
                        return View("Error");
                    }
                    VeterinarioService veterinarioService = new VeterinarioService();
                    ViewBag.Motivo = veterinarioService.ObtenerMotivoSelect();             
                    ViewBag.IdCitaEsp = id_cita_esp;
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
                return View(); // Muestra la vista en caso de que ocurra una excepción.
            }

        }
       
        [HttpPost]
        [ValidarRolUtility(4)]
        public ActionResult DescripConsulta(CitaEspecDto citaEsp)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    
                    if (citaEsp.id_motivo == 0)
                    {
                        TempData["id_cita_esp"] = citaEsp.id_cita_esp;
                        return RedirectToAction("DescripConsulta");                       

                    }
                    else
                    {
                        VeterinarioService veterService = new VeterinarioService();
                        CitaEspecDto citaEspecRespuesta = veterService.ActualizarDescripConsulta(citaEsp);
                        if (citaEspecRespuesta.Response == 1)
                        {
                            // Si se guardó correctamente
                            return Json(new { success = true, redirectUrl = Url.Action("IndexVeterinario", "Veterinario"), message = "Guardado exitosamente" });
                        }
                        else
                        {
                            return Json(new { success = false, changePassword = true, message = "No se pudo guardar correctamente" });
                        }
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
                return View("Error"); // Muestra la vista en caso de que ocurra una excepción.
            }

        }

    }
    

}