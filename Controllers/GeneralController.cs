using Clinipet.Dtos;
using Clinipet.Repositories;
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
        public ActionResult CerrarSesionYVolver()
        {
            Session.Clear(); // Elimina todas las variables de sesión
            return RedirectToAction("Index", "Home"); // Redirige al inicio
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
            if (Session["UsuLoguedo"] != null)
            {
                GeneralService generalService = new GeneralService();
                ViewBag.Razas = generalService.ObtenerRazasSelect();
                ViewBag.Tipos = generalService.ObtenerTiposSelect();

                return View();
            }
            else
            {

                return RedirectToAction("Login", "General");
            }           

        }

        //POST: General
        [HttpPost]
        public ActionResult Login(UserDto nuevoUsu)
        {
            GeneralService userService = new GeneralService();
            UserDto userResponse = userService.Login(nuevoUsu);
            System.Diagnostics.Debug.WriteLine("Llegué al método Login");
            
                if(userResponse.id_usu != 0)
                {
                    if (userResponse.Response == 1)
                    {
                        // Almacena los datos en variables de sesión
                        Session["UsuLoguedo"] = userResponse;
                        Session["Id"] = userResponse.id_usu;
                        Session["Nombre"] = userResponse.nom_usu;
                        Session["Apellido"] = userResponse.apel_usu;
                        Session["Num_docu"] = userResponse.num_ident;
                        Session["Rol"] = userResponse.id_rol;
                    
                        //Redirigir a vistas correspondientes   
                        if (userResponse.id_rol == 1)//Administrador
                        {
                            return RedirectToAction("IndexAdmin", "Admin");
                        }
                        if (userResponse.id_rol == 2)//Asistente
                        {
                            return RedirectToAction("IndexAsistente", "Asistente");
                        }
                        if (userResponse.id_rol == 3)//Cliente
                        {
                            return RedirectToAction("IndexCliente", "Cliente");
                        }
                        if (userResponse.id_rol == 4)//Veterinario
                        {
                            return RedirectToAction ("IndexVeterinario", "Veterinario");
                        }
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
                    return Json(new { success = true, message = userResponse.Mensaje }); // Si la creación fue exitosa.

                }
                else
                {
                    if (userResponse.Response == -1)
                    {
                        
                        return Json(new { success = false, message = userResponse.Mensaje }); //Si el correo ya existe
                    }
                    else
                    {
                        if (userResponse.Response == -2)
                        {

                            return Json(new { success = false, message = userResponse.Mensaje }); //Si el documento ya existe
                        }
                        else
                        {
                            return RedirectToAction("About", "Home"); // Si el registro falla
                        }                                            
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
                if (Session["UsuLoguedo"] != null)
                {
                     System.Diagnostics.Debug.WriteLine(Session["Id"]);
                     nuevaMasc.id_usu = Convert.ToInt32(Session["Id"]); // Asigna el ID del usuario logueado
                     GeneralService mascService = new GeneralService(); // instancia el UserService.
                     MascotaDto mascResponse = mascService.RegistrarMascota(nuevaMasc); // Llama al método de creación de usuario.
               
                    if (mascResponse.Response == 1)
                    {
                        //return Json(new { success = true, message = "Registro exitoso" });
                        return RedirectToAction("Index", "Home");                     
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
                    return View(); // Muestra la vista en caso de que ocurra una excepción.
            }

        }
    }
}
