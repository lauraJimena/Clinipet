using Clinipet.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;

namespace Clinipet.Utilities
{
    public class EmailConfigUtility
    {
        private SmtpClient cliente;
        private MailMessage email;
        private string Host = "smtp.gmail.com";
        private int Port = 587;
        private string User = "veteri.clinipet@gmail.com";
        private string Password = "wtlc gerj tgan xpmw"; // Contraseña de Aplicación 
        private bool EnabledSSL = true;

        public EmailConfigUtility()
        {
            cliente = new SmtpClient(Host, Port)
            {
                EnableSsl = EnabledSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };
            
        }
        // Método para enviar correos
        public void EnviarCorreoCita(string destinatario, string asunto, UserDto user, CitaEspecDto cita, MascotaDto mascota)
        {
            try
            {
                string mensaje = ObtenerPlantillaCitaEspec(user, cita, mascota);
                email = new MailMessage(User, destinatario, asunto, mensaje)
                {
                    IsBodyHtml = true
                };
                // Cabeceras para marcar el correo como de alta prioridad
                email.Headers.Add("X-Priority", "1"); // Alta prioridad
                email.Headers.Add("X-MSMail-Priority", "High");
                email.Headers.Add("Importance", "High");

                // Crea el recurso LinkedResource para la imagen que se mostrará inline
                LinkedResource logo = new LinkedResource(@"C:\Users\viher\source\repos\Prueba1\Prueba1\Imagenes\logo_clinipet-Photoroom.png", "image/png");
                logo.ContentId = "LogoCliniPet";  // Este ID será utilizado en el HTML
                logo.ContentType.Name = "logo_clinipet.png";


                // Crea una vista alterna que contiene el cuerpo del correo en HTML
                AlternateView vista = AlternateView.CreateAlternateViewFromString(mensaje, null, "text/html");

                // Agrega el recurso de la imagen a la vista alterna
                vista.LinkedResources.Add(logo);
              
                email.AlternateViews.Add(vista);


                cliente.Send(email);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar correo: " + ex.Message);
                throw; // Asegúrate de manejar esta excepción donde se llama a este método
            }
        }
        public void EnviarCorreoBienv(string destinatario, string asunto, UserDto user)
        {
            try
            {
                string mensaje = ObtenerPlantillaBienv(user);
                email = new MailMessage(User, destinatario, asunto, mensaje)
                {
                    IsBodyHtml = true
                };
                // Cabeceras para marcar el correo como de alta prioridad
                email.Headers.Add("X-Priority", "1"); // Alta prioridad
                email.Headers.Add("X-MSMail-Priority", "High");
                email.Headers.Add("Importance", "High");

                // Crea el recurso LinkedResource para la imagen que se mostrará inline
                //LinkedResource logo = new LinkedResource(@"C:\Users\viher\source\repos\Clinipet\Imagenes\logo_clinipet_sin_fondo.png", "image/png");
                string rutaLogo = HostingEnvironment.MapPath("~/Imagenes/logo_clinipet_sin_fondo.png");
                LinkedResource logo = new LinkedResource(rutaLogo, "image/png");
                logo.ContentId = "LogoCliniPet";  // Este ID será utilizado en el HTML
                logo.ContentType.Name = "logo_clinipet.png";

                // Nueva imagen (bienvenida)
                //LinkedResource imagenBienvenida = new LinkedResource(@"C:\Users\viher\source\repos\Clinipet\Imagenes\correo_bienv.jpg", "image/jpg");
                string rutaBienv = HostingEnvironment.MapPath("~/Imagenes/correo_bienv.jpg");
                LinkedResource imagenBienvenida = new LinkedResource(rutaBienv, "image/jpg");
                imagenBienvenida.ContentId = "ImagenBienvenida";
                logo.ContentType.Name = "bienvenido_clinipet.png";


                // Crea una vista alterna que contiene el cuerpo del correo en HTML
                AlternateView vista = AlternateView.CreateAlternateViewFromString(mensaje, null, "text/html");

                // Agrega el recurso de la imagen a la vista alterna
                vista.LinkedResources.Add(logo);

                // Asigna la vista alterna al correo
                vista.LinkedResources.Add(imagenBienvenida);

                email.AlternateViews.Add(vista);


                cliente.Send(email);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar correo: " + ex.Message);
                throw; // Asegúrate de manejar esta excepción donde se llama a este método
            }
        }
        public void EnviarCorreoRestablecimiento(string destinatario, string asunto, UserDto user, string token)
        {
            try
            {
         
                string mensaje = ObtenerPlantillaRestab(user, token); // Pasa el token aquí
                email = new MailMessage(User, destinatario, asunto, mensaje)
                {
                    IsBodyHtml = true
                };
                // Cabeceras para marcar el correo como de alta prioridad
                email.Headers.Add("X-Priority", "1"); // Alta prioridad
                email.Headers.Add("X-MSMail-Priority", "High");
                email.Headers.Add("Importance", "High");

                // Crea el recurso LinkedResource para la imagen que se mostrará inline
               
                string rutaLogo = HostingEnvironment.MapPath("~/Imagenes/logo_clinipet_sin_fondo.png");
                LinkedResource logo = new LinkedResource(rutaLogo, "image/png");
                logo.ContentId = "LogoCliniPet";  // Este ID será utilizado en el HTML
                logo.ContentType.Name = "logo_clinipet.png";           

                // Crea una vista alterna que contiene el cuerpo del correo en HTML
                AlternateView vista = AlternateView.CreateAlternateViewFromString(mensaje, null, "text/html");

                // Agrega el recurso de la imagen a la vista alterna
                vista.LinkedResources.Add(logo);
            

                email.AlternateViews.Add(vista);


                cliente.Send(email);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar correo: " + ex.Message);
                throw; // Asegúrate de manejar esta excepción donde se llama a este método
            }
        }




        // Método para obtener la plantilla de bienvenido
        private string ObtenerPlantillaBienv(UserDto user)
        {

            return ObtenerPlantillaBienvenido(user);

        }
        // Método para obtener la plantilla de cita agendada
        private string ObtenerPlantillaCitaEspec(UserDto usu, CitaEspecDto cita, MascotaDto mascota)
        {

            return ObtenerPlantillaCita(usu, cita, mascota);

        }
        // Método para obtener la plantilla de restablcer contraseña
        private string ObtenerPlantillaRestab(UserDto usu, string token)
        {

            return ObtenerPlantillaRestablecer(usu, token);

        }

        // Plantilla de cita confirmada
        private string ObtenerPlantillaCita(UserDto usu, CitaEspecDto cita, MascotaDto mascota)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Detalles de la Cita</title>
               
            </head>
           
            <body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f9f9f9; color: #333;'>
                <div style='max-width: 600px; margin: auto; background-color: #ffffff; border: 1px solid #ddd; padding: 20px;'>

                    <!-- Encabezado -->
                    <div style='display: flex; align-items: center; justify-content: center; gap: 20px; margin-bottom: 20px; background-color: #78a890; padding: 15px; '>
                         <img src='cid:LogoCliniPet' alt='Logo CliniPet' style='max-width:165px;' />
                        <h1 style='margin: 0; font-size: 24px; color: white; margin-top:8px;'>Cita agendada, {usu.nom_usu}!</h1>
                    </div>

                    <!-- Texto de introducción -->
                    <p style='font-size: 16px;'>
                        Estimado(a) cliente, la cita de su mascota se agendó correctamente. Recuerde asistir 15 minutos antes de la hora de su cita.
                    </p>
                    <p style='font-size: 16px;'>
                        A continuación, los detalles de la cita médica agendada y la mascota a la cual se realizará la consulta:
                    </p>

                    <!-- Detalles de la Cita -->
                    <table width='100%' cellpadding='0' cellspacing='0' style='max-width: 100%; margin-top: 20px; font-size: 15px; border-collapse: collapse;'>
                        <tr>
                            <td colspan='2' style='background-color: #78a890; color: white; padding: 10px; font-weight: bold;'>Detalles de la Cita</td>
                        </tr>
                        <tr>
                            <th style='text-align: left; padding: 8px; background-color: #d9a86c; border: 1px solid #ddd;'>Fecha</th>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{cita.fecha_cita.ToString("dd/MM/yyyy")}</td>
                        </tr>
                        <tr>
                            <th style='text-align: left; padding: 8px; background-color: #d9a86c; border: 1px solid #ddd;'>Hora</th>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{cita.nom_hora}</td>
                        </tr>
                        <tr>
                            <th style='text-align: left; padding: 8px; background-color: #d9a86c; border: 1px solid #ddd;'>Veterinario</th>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{cita.nom_usu}</td>
                        </tr>
                        <tr>
                            <th style='text-align: left; padding: 8px; background-color: #d9a86c; border: 1px solid #ddd;'>Especialidad</th>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{cita.nom_espec}</td>
                        </tr>
                    </table>

                    <!-- Datos de la Mascota -->
                    <table width='100%' cellpadding='0' cellspacing='0' style='max-width: 100%; margin-top: 30px; font-size: 15px; border-collapse: collapse;'>
                        <tr>
                            <td colspan='2' style='background-color: #78a890; color: white; padding: 10px; font-weight: bold;'>Datos de la Mascota</td>
                        </tr>
                        <tr>
                            <th style='text-align: left; padding: 8px; background-color: #d9a86c; border: 1px solid #ddd;'>Nombre</th>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{mascota.nom_masc}</td>
                        </tr>
                        <tr>
                            <th style='text-align: left; padding: 8px; background-color: #d9a86c; border: 1px solid #ddd;'>Tipo</th>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{mascota.nom_tipo}</td>
                        </tr>
                        <tr>
                            <th style='text-align: left; padding: 8px; background-color: #d9a86c; border: 1px solid #ddd;'>Raza</th>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{mascota.nom_raza}</td>
                        </tr>
                        <tr>
                            <th style='text-align: left; padding: 8px; background-color: #d9a86c; border: 1px solid #ddd;'>Dueño</th>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{mascota.nom_usu}</td>
                        </tr>
                        <tr>
                            <th style='text-align: left; padding: 8px; background-color: #d9a86c; border: 1px solid #ddd;'>Documento</th>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{usu.num_ident}</td>
                        </tr>
                    </table>

                    <!-- Footer -->
                    <div style='text-align: center; margin-top: 40px; font-size: 14px;  background-color: #78a890;'>
                        <p  style='color: white;'>&copy; 2025 CliniPet</p>
                    </div>
                </div>
             </body>

        </html>";
     }
        // Plantilla de bienvenido 
        private string ObtenerPlantillaBienvenido(UserDto user)
        {
            return $@"
            <body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #9fb2a9;'>
                <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
                    <tr>
                        <td align='center'>
                            <table width='700' style='background-color: #ffffff;'>
                    
                            <!-- Encabezado con logo y texto -->
                                <tr>
                                    <td style='background-color: #78a890; padding: 20px;'>
                                        <table width='100%'>
                                            <tr>
                                                <!-- Logo -->
                                                <td style='text-align: center;'>
                                                        <img src='cid:LogoCliniPet' alt='Logo CliniPet' style='max-width:150px;'/>
                                                </td>

                                                <!-- Texto Bienvenida -->
                                                <td style='width: 80%; text-align: left;'>
                                                    <h1 style='margin: 0; color: white; font-size: 29px;'>Bienvenido, {user.nom_usu}</h1>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <!-- Contenido con imagen y texto -->
                                <tr style='background-color:#e4e4e3; padding:15px;'>
                                    <td style='padding-left: 8px;'>
                                        <table width='100%' >
                                            <tr>
                                                <!-- Columna izquierda: texto -->
                                                <td style='width: 80%; padding-right: 2px;'>
                                                    <div style='background-color: #d9a86c; padding: 15px; border-radius: 10px; color: #333333; font-size: 17px; text-align: center;'>
                                                    Estamos emocionados de tenerte con nosotros en <strong>CliniPet</strong>, tu clínica veterinaria de confianza, Ofrecemos 
                                                    servicios especializados y generales para el cuidado de perros 🐶 y gatos 🐱. ¡Disfruta tu experiencia en nuestro sistema! 🐾
                                                    </div>
                                                </td>

                                                <!-- Columna derecha: imagen -->
                                                <td style='width: 18%; text-align: right;'>
                                                   <img src='cid:ImagenBienvenida' alt='Imagen Bienvenida' style='max-width: 380px; height: auto; border-radius: 10px;' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <!-- Footer -->
                                <tr>
                                    <td style='background-color: #78a890; text-align: center; padding: 15px;'>
                                        <p style='color: white; margin: 0; font-size: 14px;'>© 2025 CliniPet</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                
                </table>
            </body>";

        }
        // Plantilla de bienvenido 
        private string ObtenerPlantillaRestablecer(UserDto user, string token)
        {
           
            string baseUrl = "https://localhost:44388/";
            string urlCambio = $"{baseUrl}General/RestablecerContras?token={token}";

            return $@"
    <body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #9fb2a9;'>
        <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
            <tr>
                <td align='center'>
                    <table width='700' style='background-color: #ffffff;'>

                        <!-- Encabezado -->
                        <tr>
                            <td style='background-color: #78a890; padding: 20px;'>
                                <table width='100%'>
                                    <tr>
                                        <td style='text-align: center;'>
                                            <img src='cid:LogoCliniPet' alt='Logo CliniPet' style='max-width:150px;'/>
                                        </td>
                                        <td style='width: 80%; text-align: left;'>
                                            <h1 style='margin: 0; color: white; font-size: 26px;'>Restablecer Contraseña</h1>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <!-- Contenido -->
                        <tr style='background-color:#e4e4e3; padding:15px;'>
                            <td style='padding: 20px;'>
                                <div style='background-color: #d9a86c; padding: 20px; border-radius: 10px; color: #333333; font-size: 17px; text-align: center;'>
                                    <p>Recibimos una solicitud para restablecer tu contraseña en <strong>CliniPet</strong>.</p>
                                    <p>Haz clic en el botón de abajo para cambiar tu contraseña de forma segura:</p>
                                    <a href='{urlCambio}' style='display: inline-block; padding: 12px 24px; background-color: #78a890; color: white; text-decoration: none; border-radius: 8px; font-size: 16px; margin-top: 10px;'>Restablecer Contraseña</a>
                                    <p style='margin-top: 15px; font-size: 14px;'>Si no solicitaste este cambio, puedes ignorar este mensaje.</p>
                                </div>
                            </td>
                        </tr>

                        <!-- Footer -->
                        <tr>
                            <td style='background-color: #78a890; text-align: center; padding: 15px;'>
                                <p style='color: white; margin: 0; font-size: 14px;'>© 2025 CliniPet</p>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>
    </body>";

        }

    }
}