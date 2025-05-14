using Clinipet.Dtos;
using Clinipet.Repositories;
using Clinipet.Services;
using Clinipet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        //Cerrar Sesión
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            FormsAuthentication.SignOut(); 
            return RedirectToAction("Login", "General");
        }

        public ActionResult SesionCerrada()
        {
            // Evita que la página quede en la caché del navegador
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
            Response.Cache.SetNoStore();

            return View(); // Renderiza la vista "SesionCerrada.cshtml" en lugar de redirigir nuevamente
        }   

        
        public ActionResult RegistroCliente()
        {
            UserDto user = new UserDto();
            return View(user);
        }

        [ValidarRolUtility(3)]
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
            try
            {
                System.Diagnostics.Debug.WriteLine("Datos recibidos - num_ident: " + nuevoUsu.num_ident + " contras_usu: " + nuevoUsu.contras_usu);
                GeneralService userService = new GeneralService();
                UserDto userResponse = userService.Login(nuevoUsu);
                System.Diagnostics.Debug.WriteLine("Llegué al método Login");

                if (userResponse != null && userResponse.id_usu != 0)
                {
                    if (userResponse.Response == 1)
                    {
                        // Almacena los datos en variables de sesión
                        Session["UsuLoguedo"] = userResponse;
                        Session["Id"] = userResponse.id_usu;
                        Session["Nombre"] = userResponse.nom_usu;
                        Session["Apellido"] = userResponse.apel_usu;
                        Session["Num_docu"] = userResponse.num_ident;
                        Session["RolUsu"] = userResponse.id_rol;
                        Session["Contras"] = userResponse.contras_usu;
                        Session["Correo"] = userResponse.correo_usu;

                        // Si necesita cambiar la contraseña, redirige a la vista de cambio de contraseña
                        if (userResponse.cambio_contras == true)
                        {
                            return Json(new { success = true, changePassword = true, message = "Debes actualizar tu contraseña." });
                        }
                        else
                        {
                            // Si no necesita cambiar la contraseña, redirige al índice de cada usuario
                            if (userResponse.id_rol == 1) //Administrador
                            {
                                return Json(new { success = true, redirectUrl = Url.Action("IndexAdmin", "Administrador"), message = "Inicio de sesión exitoso" });
                            }
                            if (userResponse.id_rol == 2) //Asistente
                            {
                                return Json(new { success = true, redirectUrl = Url.Action("IndexAsistente", "Asistente"), message = "Inicio de sesión exitoso" });
                            }
                            if (userResponse.id_rol == 3) //Cliente
                            {
                                return Json(new { success = true, redirectUrl = Url.Action("IndexCliente", "Cliente"), message = "Inicio de sesión exitoso" });
                            }
                            if (userResponse.id_rol == 4) //Veterinario
                            {
                                return Json(new { success = true, redirectUrl = Url.Action("IndexVeterinario", "Veterinario"), message = "Inicio de sesión exitoso" });
                            }
                        }
                    }
                }
                // Si las credenciales son incorrectas, redirige de nuevo al login con un mensaje de error
                return Json(new { success = false, message = "Credenciales incorrectas." });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en Login: " + ex.ToString());
                return Json(new { success = false, message = "Ocurrió un error en el servidor. Intenta de nuevo." });
            }
        }



        [HttpPost]        
        public ActionResult RegistroCliente(UserDto nuevoUsu)
        {
            try
            {
                GeneralService userService = new GeneralService(); // instancia el UserService.
                UserDto userResponse = userService.RegistrarCliente(nuevoUsu); // Llama al método de creación de usuario.
              
                if (userResponse.Response == 1)
                {
                    userResponse.Mensaje = "Cliente registrado exitosamente.";     
                    return Json(new { success = true, redirectUrl = Url.Action("Login", "General")});

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
                            return View("Error"); // Si el registro falla
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
        [ValidarRolUtility(3)]
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
                        return Json(new { success = true, redirectUrl = Url.Action("IndexCliente", "Cliente") });
                    }
                 
                    else
                    {
                        return Json(new { success = false, message = "No se pudo confirmar la cita." });

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

        [HttpGet]
        public ActionResult CambiarContraseña()
        {

            // Verificar si el usuario está logueado
            if (Session["UsuLoguedo"] != null)
            {
                return View();
            }
            else
            {
                // Si no está logueado, redirigir al login
                return RedirectToAction("Login", "General");
            }
        }


        [HttpPost]
        public ActionResult CambiarContraseña(UserDto user)
        {
            try
            {
                // Instanciamos el servicio.
                GeneralService contrasService = new GeneralService();

                // Llamar al método CambiarContraseña, pasando la contraseña actual y la nueva contraseña.
                bool cambioExitoso = contrasService.CambiarContraseña(user.num_ident, user.contras_usu, user.contras_nueva, user.confirmar_contras);

                if (cambioExitoso)
                {
                    ViewBag.MensajeExito = "Contraseña actualizada correctamente.";
                    return View(); 
                }
                else
                {
                    // Si el cambio de contraseña falló, mostramos un error.
                    ViewBag.Error = "Hubo un error al cambiar la contraseña.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Si hay una excepción, la capturamos y mostramos un mensaje de error.
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult OlvideContrasena(string num_ident)
        {
            GeneralService generalService = new GeneralService();

            // Buscar el usuario por número de identidad
            UserDto usuRespuesta = generalService.BuscarPorIdentidad(num_ident);

            if (usuRespuesta != null)
            {
                string correoUsuario = usuRespuesta.correo_usu; // Obtener el correo del usuario

                if (!string.IsNullOrEmpty(correoUsuario))
                {
                    // Llamar al servicio de envío de correo
                    
                    generalService.EnviarCorreoRestablecimiento(usuRespuesta);
                    return Json(new { success = true });
                    
                }              
                return Json(new { success = false, message = "El usuario no tiene un correo registrado." });
                
            }
            return Json(new { success = false, message = "Número de identidad no encontrado." });
            
        }
        [HttpGet]
        public ActionResult RestablecerContras(string token)
        {            
            return View(new ContrasDto { Token = token });           
        }

        [HttpPost]
        public ActionResult RestablecerContras(ContrasDto contras)
        {
            if (contras.NuevaContrasena != contras.ConfirmarContrasena)
            {
                return Json(new { exito = false, mensaje = "Las contraseñas no coinciden." });
            }

            var generalService = new GeneralService();
            int? idUsuario = generalService.ObtenerIdUsuarioPorToken(contras.Token);

            if (idUsuario == null)
            {
                return Json(new { exito = false, mensaje = "El enlace ha expirado o no es válido." });
            }

            bool resultado = generalService.RestablecerContrasena(idUsuario.Value, contras.NuevaContrasena);

            if (resultado)
            {
                return Json(new { exito = true, mensaje = "Tu contraseña ha sido actualizada con éxito.", redirectUrl = Url.Action("Login", "General") });
            }
            else
            {
                return Json(new { exito = false, mensaje = "Error al actualizar la contraseña." });
            }
        }


    }

}
