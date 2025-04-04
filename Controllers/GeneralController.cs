using Clinipet.Dtos;
using Clinipet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinipet.Controllers
{
    public class GeneralController : Controller
    {
        // GET: General
        public ActionResult Index()
        {
            return View();
        }
        //Login igual para todos los roles
        public ActionResult Login()
        {
            return View();
        }
        //RegistroCliente lo realizan tanto clientes como asistentes
        public ActionResult RegistroCliente()
        {
            UserDto user = new UserDto();
            return View(user);
        }
        public ActionResult RegistroMascota()
        {
            GeneralService generalService = new GeneralService();

            /*List<MascotaDto> razas;     
            razas = generalService.obtenerRazas();


            //Convertir la lista raza en SelectList para pasar a la vista
            ViewBag.Razas = razas.Select(r => new SelectListItem
              {
                  Value = r.id_raza.ToString(),
                  Text = r.nom_raza,
                 Group = new SelectListGroup { Name = r.nom_tipo } // Group para almacenar el tipo
             }).ToList();*/

            //List<MascotaDto> tipos;
            //tipos = generalService.obtenerTipos();

            // Convertir la lista tipo en SelectList para pasar a la vista
            /*ViewBag.Tipos = tipos.Select(t => new SelectListItem
            {
                Value = t.id_tipo.ToString(),
                Text = t.nom_tipo
            }).ToList();*/

            ViewBag.Razas = generalService.ObtenerRazasSelect();
            ViewBag.Tipos = generalService.ObtenerTiposSelect();

            return View();
        }
        [HttpPost]
        public ActionResult Login(UserDto nuevoUsu)
        {
            GeneralService userService = new GeneralService();
            UserDto userResponse = userService.Login(nuevoUsu);
            System.Diagnostics.Debug.WriteLine("Llegué al método Login");
            if (userResponse.Response == 1)
            {
                // Almacena los datos en variables de sesión
                Session["Id"] = userResponse.id_usu;
                Session["Nombre"] = userResponse.nom_usu;
                Session["Apellido"] = userResponse.apel_usu;
                Session["Num_docu"] = userResponse.num_ident;
                Session["Rol"] = userResponse.id_rol;                
                System.Diagnostics.Debug.WriteLine(Session["Rol"]);
                

                if (userResponse.id_rol == 1)
                {
                    return RedirectToAction("Index", "Home");
                }               
                if (userResponse.id_rol == 2)
                {
                    return RedirectToAction("About", "Home");
                }
                if (userResponse.id_rol == 3)
                {
                    return RedirectToAction("Contact", "Home");
                }


                {
                    return RedirectToAction("RegistroCliente");
                }

            }
            else
            {
                // Muestra el mensaje de error si las credenciales son incorrectas
                ModelState.AddModelError("", userResponse.Mensaje);
            }

            return View();
        }



        [HttpPost]
        public ActionResult RegistroCliente(UserDto nuevoUsu)
        {
            try
            {
                GeneralService userService = new GeneralService(); // instancia el UserService.
                UserDto userResponse = userService.RegistrarCliente(nuevoUsu); // Llama al método de creación de usuario.
                Console.WriteLine(userResponse.correo_usu);
                if (userResponse.Response == 1)
                {
                    return Json(new { success = true, message = "Registro exitoso" });
                    //return RedirectToAction("Index", "Home");// Redirige a la vista principal si la creación fue exitosa.
                }

                else
                {
                    if (userResponse.Response == -1)
                    {
                        
                        return Json(new { success = false, message = "El correo ya existe. Intente nuevamente." });
                    }
                    else
                    {
                        return RedirectToAction("About", "Home");
                        //return View(userResponse); // Muestra la vista con la respuesta del servicio en caso de error.
                    }

                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return View(); // Muestra la vista en caso de que ocurra una excepción.
            }

        }
            [HttpPost]
            public ActionResult RegistroMascota(MascotaDto nuevaMasc)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine(Session["Id"]);
                    nuevaMasc.id_usu = Convert.ToInt32(Session["Id"]); // Asigna el ID del usuario logueado
                    GeneralService mascService = new GeneralService(); // instancia el UserService.
                    MascotaDto mascResponse = mascService.RegistrarMascota(nuevaMasc); // Llama al método de creación de usuario.
               
                if (mascResponse.Response == 1)
                    {
                        //return Json(new { success = true, message = "Registro exitoso" });
                    return RedirectToAction("Index", "Home");
                    //return RedirectToAction("Index", "Home");// Redirige a la vista principal si la creación fue exitosa.
                }

                   
                        else
                        {
                            return RedirectToAction("About", "Home");
                           
                        }

                    
                }
                catch (Exception ex)
                {
                    string mensaje = ex.Message;
                    return View(); // Muestra la vista en caso de que ocurra una excepción.
                }

            }
    }
}
