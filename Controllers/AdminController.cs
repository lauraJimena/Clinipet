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
    public class AdminController : Controller
    {
        // Vista principal admin
        public ActionResult IndexAdmin()
        {
            return View();
        }
        // Registro de asistentes
        public ActionResult RegistroAsistente()
        {
            UserDto user = new UserDto();
            return View(user);
        }
        // Guardar datos del formulario
        [HttpPost]
        public ActionResult RegistAsist(UserDto nuevoAsist)
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
        //Registrar Veterinario
        public ActionResult RegistroVeterinario()
        {
            UserDto user = new UserDto();

            // Obtener lista de especialidades
            VeterinarioService VeteService = new VeterinarioService();
            List<UserDto> especialidades = VeteService.ObtenerEspecialidad();

            // Enviar la lista a la vista
            ViewBag.Especialidad = VeteService.ObtenerEspecialidad();

            return View(user);
        }

        //Eliminar Usuarios
        public ActionResult EliminarUsuarios()
        {
            UserDto user = new UserDto();
            return View(user);
        }

        // Guardar datos del formulario
        public ActionResult RegistVet(UserDto nuevoVete)
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

        //Mostrar lista de veterinarios
        public ActionResult EliminarVeterinario()
        {
            AdminService adminService = new AdminService();
            List<UserDto> veterinario = adminService.ObtenerVeterinarios();

            return View("EliminarVeterinario", veterinario);
        }

        //Buscar Veterinario
        public ActionResult BuscarVeterinario(string num_ident)
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

        //Eliminar Veterinario
        [HttpPost]
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

        //Mostrar lista de asistentes
        public ActionResult EliminarAsistente()
        {
            AdminService adminService = new AdminService();
            List<UserDto> asistente = adminService.ObtenerAsistentes();

            return View("EliminarAsistente", asistente);
        }

        //Eliminar Asistente
        [HttpPost]
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

        //Buscar Asistente
        public ActionResult BuscarAsistente(string num_ident)
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
    }
}