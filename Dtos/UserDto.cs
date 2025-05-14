using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clinipet.Dtos
{
    public class UserDto
    {
       
        public int id_usu { get; set; }

        [Display(Name = "Nombres")]
        public string nom_usu { get; set; } = string.Empty;

        [Display(Name = "Apellidos")]
        public string apel_usu { get; set; } = string.Empty;

        [Display(Name = "Tipo de documento")]
        public int id_tipo_ident { get; set; }

        [Display(Name = "Número de documento")]
        public string num_ident { get; set; } = string.Empty;

        [Display(Name = "Correo")]
        public string correo_usu { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        public string tel_usu { get; set; } = string.Empty;

        [Display(Name = "Contraseña")]
        public String contras_usu{ get; set; }= string.Empty;

        public int id_rol{ get; set; }

        public int id_estado { get; set; }

        [Display(Name = "Nivel académico")]
        public int id_nivel{ get; set; }

        [Display(Name = "Especialidad")]
        public int id_espec{ get; set; }

        [Display(Name = "Nom especialidad")]
        public String nom_espec { get; set; } = string.Empty;

        public bool cambio_contras { get; set; }

        public string contras_nueva { get; set; }
        public string confirmar_contras { get; set; }

        public int Response{ get; set; }

        public string Mensaje { get; set; }= string.Empty;
    }
}