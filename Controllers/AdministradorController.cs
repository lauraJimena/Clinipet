using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clinipet.Dtos;
using Clinipet.Services;
using Clinipet.Repositories;
using Rotativa;
using Clinipet.Utilities;

namespace Clinipet.Controllers
{
    public class AdministradorController : BaseController
    {
        //GET: Administrador

        // Vista principal admin
        [ValidarRolUtility(1)] //Validación para que solo rol 1 (administrador) acceda a la vista
        public ActionResult IndexAdmin()
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
        // Registro de asistentes
        [ValidarRolUtility(1)]
        public ActionResult RegistroAsistente()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    UserDto user = new UserDto();
                    return View(user);
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

        //Eliminar Usuarios
        [ValidarRolUtility(1)]
        public ActionResult EliminarUsuarios()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    UserDto user = new UserDto();
                    return View(user);
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

        // Guardar datos del formulario
        [ValidarRolUtility(1)]
        public ActionResult RegistVet(UserDto nuevoVete)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    VeterinarioService servicio = new VeterinarioService();
                    UserDto result = servicio.RegistrarVeterinario(nuevoVete);

                    if (result.Response == 1)
                    {
                        return Json(new { success = true, message = result.Mensaje });
                    }
                    else
                    {
                        return Json(new { success = false, message = result.Mensaje });
                    }
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

        //Mostrar lista de veterinarios
        [ValidarRolUtility(1)]
        public ActionResult EliminarVeterinario()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    AdminService adminService = new AdminService();
                    List<UserDto> veterinario = adminService.ObtenerVeterinarios();

                    return View("EliminarVeterinario", veterinario);
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

        //Buscar Veterinario
        [ValidarRolUtility(1)]
        public ActionResult BuscarVeterinario(string num_ident)
        {
            try
            {
                if (Session["UsuLoguedo"] != null) 
                {
                    AdminService adminService = new AdminService();
                    var todosLosVeterinarios = adminService.ObtenerVeterinarios();

                    if (string.IsNullOrEmpty(num_ident))
                    {
                        return View("EliminarVeterinario", todosLosVeterinarios);
                    }

                    var filtrados = todosLosVeterinarios
                                    .Where(v => v.num_ident.Contains(num_ident))
                                    .ToList();

                    return View("EliminarVeterinario", filtrados);
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

        //Registrar Veterinario
        [ValidarRolUtility(1)]
        public ActionResult RegistroVeterinario()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    UserDto user = new UserDto();

                    // Obtener lista de especialidades
                    VeterinarioService VeteService = new VeterinarioService();
                    List<UserDto> especialidades = VeteService.ObtenerEspecialidad();

                    // Enviar la lista a la vista
                    ViewBag.Especialidad = VeteService.ObtenerEspecialidad();

                    return View(user);
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
        //Mostrar lista de asistentes
        [ValidarRolUtility(1)]
        public ActionResult EliminarAsistente()
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    AdminService adminService = new AdminService();
                    List<UserDto> asistente = adminService.ObtenerAsistentes();

                    return View(asistente);
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
        //Buscar Asistente
        [ValidarRolUtility(1)]
        public ActionResult BuscarAsistente(string num_ident)
        {
            try
            {
                if (Session["UsuLoguedo"] != null)
                {
                    AdminService adminService = new AdminService();
                    var todosLosAsistentes = adminService.ObtenerAsistentes();

                    if (string.IsNullOrEmpty(num_ident))
                    {
                        return View("EliminarAsistente", todosLosAsistentes);
                    }

                    var filtrados = todosLosAsistentes
                                    .Where(v => v.num_ident.Contains(num_ident))
                                    .ToList();

                    return View("EliminarAsistente", filtrados);
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
        //Generar reporte de servicios prestados 
        [ValidarRolUtility(1)]
        public ActionResult ReporteServicios()
        {
            try
            {

                ReporteService reporService = new ReporteService();
                var modelo = reporService.ObtenerDatosReporteServicios();            
                return ReportUtility.GenerarPdf(this.ControllerContext, modelo);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
       
        //POST: Administrador

        //Eliminar Veterinario
        [HttpPost]
        [ValidarRolUtility(1)]
        public ActionResult EliminVete(int id_usu)
        {
            try
            {
                AdminService adminService = new AdminService();
                adminService.EliminVete(id_usu);

                TempData["Mensaje"] = "Veterinario eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar el veterinario.";
            }

            // Recarga la lista después de eliminar
            return RedirectToAction("EliminarVeterinario");
        }
        // Guardar datos del formulario
        [HttpPost]
        [ValidarRolUtility(1)]
        public ActionResult RegistAsist(UserDto nuevoAsist)
        {
            try
            {
                AsistenteService servicio = new AsistenteService();
                UserDto result = servicio.RegistrarAsistente(nuevoAsist);

                if (result.Response == 1)
                {
                    return Json(new { success = true, message = result.Mensaje });
                }
                else
                {
                    return Json(new { success = false, message = result.Mensaje });
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        
        //Eliminar Asistente
        [HttpPost]
        [ValidarRolUtility(1)]
        public ActionResult EliminAsis(int id_usu)
        {
            try
            {
                AdminService adminService = new AdminService();
                adminService.EliminAsis(id_usu);

                TempData["Mensaje"] = "Asistente eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar el asistente.";
            }

            // Recarga la lista después de eliminar
            return RedirectToAction("EliminarAsistente");
        }
    }
}