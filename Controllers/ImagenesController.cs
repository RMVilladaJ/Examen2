using Examen2.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Examen2.Controllers
{
    [RoutePrefix("Imagenes")]
    public class ImagenesController : ApiController
    {
        [HttpPost]

        public async Task<HttpResponseMessage> GrabarArchivo(HttpRequestMessage Request, string Datos, string Proceso)
        {
            ClsImagenes imagenService = new ClsImagenes();
            imagenService.request = Request;
            imagenService.Datos = Datos;
            imagenService.Proceso = Proceso;

            return await imagenService.GrabarArchivo(false);
        }

        [HttpGet]

        public HttpResponseMessage Get(string FotoMulta)
        {
            ClsImagenes imagenService = new ClsImagenes();
            return imagenService.ConsultarArchivo(FotoMulta);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> ActualizarArchivo(HttpRequestMessage Request, string Datos, string Proceso)
        {
            ClsImagenes imagenService = new ClsImagenes();
            imagenService.request = Request;
            imagenService.Datos = Datos;
            imagenService.Proceso = Proceso;

            return await imagenService.GrabarArchivo(true);
        }
    }
}
