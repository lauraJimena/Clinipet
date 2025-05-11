using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Clinipet.Utilities
{
    public class ValidarRolUtility: ActionFilterAttribute
    {
    private readonly int[] rolesPermitidos;

        public ValidarRolUtility(params int[] roles)
        {
            rolesPermitidos = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filtroContexto)
        {
            var rolUsuario = (int?)filtroContexto.HttpContext.Session["RolUsu"];

            if (rolUsuario == null || !rolesPermitidos.Contains(rolUsuario.Value))
            {
                filtroContexto.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "controller", "Home" },
                    { "action", "AccesoDenegado" }
                    });
            }

            base.OnActionExecuting(filtroContexto);
        }
    }

}