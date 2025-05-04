using Clinipet.Dtos;
using Clinipet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinipet.Controllers
{
    
    public class ClienteController : BaseController
    {
        // GET: Cliente
        
        public ActionResult IndexCliente()
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
        public ActionResult CitasEspecDispon()
        {
            if (Session["UsuLoguedo"] != null)
            {
                GeneralService generalService = new GeneralService();
                List<DisponibDto> citas = generalService.ObtenerCitasDispon();

                return View(citas);
            }
            else
            {
                return RedirectToAction("Login", "General");
            }

        }   
        
        public ActionResult MascotasRegistradas()
        {
            if (Session["UsuLoguedo"] != null)
            {
                int id_usu = Convert.ToInt32(Session["Id"]);
                ClienteService servicio = new ClienteService();
                List<MascotaDto> mascotas = servicio.ListadoMascotas(id_usu);
                return View(mascotas);
            }
            else
            {
                return RedirectToAction("Login", "General");
            }
        }
        public ActionResult ServiciosGenerales()
        {
            if (Session["UsuLoguedo"] != null)
            {
            ClienteService clienteService = new ClienteService();
            List<ServicioDto> servicioGeneral = clienteService.ListadoServiciosGenerales(); 
            return View(servicioGeneral);
            }
            else
            {
                return RedirectToAction("Login", "General");
            }
        }
        
        public ActionResult CitasGenDispon()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    return RedirectToAction("IndexCliente", "Cliente");
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
        
        public ActionResult ListadoMascotas()
        {
            try
            {
                    if (Session["UsuLoguedo"] != null)
                    {
                    return RedirectToAction("IndexCliente", "Cliente");
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
        
        public ActionResult AgendarCita()
        {
            if (Session["UsuLoguedo"] != null)
            {
                return RedirectToAction("CitasEspecDispon", "Cliente");
            }
            else
            {
                return RedirectToAction("Login", "General");
            }
        }             
               
        public ActionResult ElegirMascota()
        {
            if (Session["UsuLoguedo"] != null)
            {
                return RedirectToAction("IndexCliente", "Cliente");
            }
            else
            {
                return RedirectToAction("Login", "General");
            }
        }


        //POST: Cliente
        [HttpPost]
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

                ClienteService clienteService = new ClienteService();
                List<DisponibDto> citasGenDispon = clienteService.ObtenerCitasGenDispon(id_servicio.Value);
                return View(citasGenDispon);
                
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("Error");  
            }
        }
        
        public ActionResult HistorialCitasGenerales()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    int id_usu = (int)Session["Id"];
                    ClienteService clienteService = new ClienteService();
                    List<CitaGeneralDto> citasGen = clienteService.HistorialCitasGenerales(id_usu);
                    return View(citasGen);
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
        public ActionResult ElegirHistorialCitas()
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
        public ActionResult HistorialCitasEspec()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    int id_usu = (int)Session["Id"];
                    ClienteService clienteService = new ClienteService();
                    List<CitaEspecDto> citasGen = clienteService.HistorialCitasEspec(id_usu);
                    return View(citasGen);
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
        public ActionResult ListadoMascotas(int id_usu, int id_dispon)
        {
            
            try
            {
                // Forzar un error
                //throw new Exception("Forzando error para probar vista Error");
                if (Session["UsuLoguedo"] != null)
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
        public ActionResult AgendarCita(int id_dispon, int id_mascota, int id_usu)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    ClienteService clienteService = new ClienteService();
                    MascotaDto mascota = clienteService.ObtenerMascotaPorId(id_mascota);
                    DisponibDto dispon = clienteService.obtenerDisponPorId(id_dispon);         
                    
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
        public ActionResult ConfirmarCita(CitaEspecDto nuevaCita)
        {
            try
            {
                ClienteService clienteService = new ClienteService(); // instancia el UserService.
                CitaEspecDto citaResponse = clienteService.RegistrarCitaEspec(nuevaCita); // Llama al método de creación de usuario.
                
                    if (citaResponse.Response == 1)
                    {
                    return Json(new { success = true, redirectUrl = Url.Action("IndexCliente") });
                    
                    }
                    else
                    {
                    return Json(new { success = false, message = "No se pudo guardar correctamente" });
                    }
                                     
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return View("Error");   
            }
        }
       
        [HttpPost]
        public ActionResult ConfirmarCitaGeneral(CitaGeneralDto nuevaCita)
        {

            try
            {
                ClienteService clienteService = new ClienteService(); // instancia el UserService.
                CitaGeneralDto citaGenResponse = clienteService.AgendarCitaGeneral(nuevaCita); // Llama al método de creación de usuario.

                if (citaGenResponse.Response == 1)
                {
                    return Json(new { success = true, redirectUrl = Url.Action("IndexCliente") });
                    
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
        public ActionResult ElegirMascota(int id_usu, int id_dispon, int id_servicio)
        {
            //id_dispon = 0; //error intencional 
            try
            {
                
                    if (Session["UsuLoguedo"] != null)
                    {

                        ClienteService clienteService = new ClienteService();
                        List<MascotaDto> mascotas = clienteService.ListadoMascotas(id_usu);

                        var modelo = new MascotaCitaDto //variable modelo de tipo MascotaCitaDto
                        {
                        Mascotas = mascotas, //Se asigna la lista para mostrar en la vista
                        IdDispon = id_dispon,
                        IdUsu = id_usu,
                        IdServicio = id_servicio
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


    }
}