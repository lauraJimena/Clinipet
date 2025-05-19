using Clinipet.Dtos;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinipet.Utilities
{
    public class ReportUtility
    {
        public static ActionResult GenerarPdf(ControllerContext context, ReporteDto reportData)
        {
            
                return new ViewAsPdf("ReporteServicios", reportData)
                {
                    FileName = "ReporteServicios.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    //CustomSwitches = "--print-media-type"
                    CustomSwitches = "--print-media-type --enable-smart-shrinking" //para aplicar correctamente los estilos de impresión y ajustar automáticamente el contenido al tamaño de página.
                };
           
        }
    }
}