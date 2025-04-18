﻿using System;
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
    }
}