using Examen2.Clases;
using Examen2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Examen2.Controllers
{

    [RoutePrefix("api/Fotomultas")]
    public class FotomultasController : ApiController
    {
        
            [HttpGet]
            [Route("ConsultarImagenes")]

            public IQueryable ConsultarImagenes(string placa)
            {
                ClsFotomulta Placa = new ClsFotomulta();
                return Placa.ConsultarImagenesXVehiculo(placa);
            }


            [HttpGet]
            [Route("ConsultarTodos")]
            public List<Infraccion> ConsultarTodos()
            {
                ClsFotomulta infracionService = new ClsFotomulta();
            return infracionService.ConsultarTodos();
            }

            [HttpGet]
            [Route("Consultar")]
            public Infraccion Consultar(string PlacaVehiculo)
            {
                ClsFotomulta infracionService = new ClsFotomulta();
                return infracionService.Consultar(PlacaVehiculo);
            }

            [HttpGet]
            [Route("ConsultarXTipoVehiculo")]

            //public List<Infraccion> ConsultarXTipoVehiculo(string TipoProducto)
            //{
            //    ClsFotomulta infracionService = new ClsFotomulta();
            //    return infracionService.consulpor

            //}

            [HttpPost]
            [Route("Insertar")]

            public string Insertar([FromBody] Infraccion infraccion)
            {
                ClsFotomulta infraccionService = new ClsFotomulta();

                 infraccionService.infraccion = infraccion;
                // se llama el metodo Insertar de la clase clsEmpleado
                return infraccionService.Insertar();

            }

            [HttpPut]
            [Route("Actualizar")]

            public string Actualizar([FromBody] Infraccion infraccion)
            {
                ClsFotomulta infraccionService = new ClsFotomulta();
                infraccionService.infraccion = infraccion;
                return infraccionService.Actualizar();
            }



        }
}