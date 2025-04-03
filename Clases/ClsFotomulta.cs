using Examen2.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Examen2.Clases
{
    public class ClsFotomulta
    {
        private DBExamenEntities dbExamen = new DBExamenEntities();
        public Vehiculo vehiculo { get; set; }
        public Infraccion infraccion { get; set; }

        public string Insertar()
        {
            try
            {
                dbExamen.Infraccion.Add(infraccion);
                

                bool vehiculoExiste = ConsultarPlaca(infraccion.PlacaVehiculo) != null;

                if (!vehiculoExiste)
                {

                    dbExamen.Vehiculo.Add(vehiculo);
                }


                dbExamen.SaveChanges();


                return "Se ingresó la infraccion para la placa " + vehiculo.Placa + " a la base de datos";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Vehiculo ConsultarPlaca(string placa) {
            Vehiculo resultado = dbExamen.Vehiculo.FirstOrDefault(p => p.Placa == placa);
            return resultado;
        }

        public string Actualizar()
        {
            try
            {

                dbExamen.Infraccion.AddOrUpdate(infraccion);
                dbExamen.SaveChanges();

                return "Se actualizo la infraccion para el vehiculo " + vehiculo.Placa + " correctamente"; //Mensaje de confirmación
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public Infraccion Consultar(string placa)
        {
            Infraccion infra = dbExamen.Infraccion
                    .FirstOrDefault(p => p.PlacaVehiculo == placa);
            return infra;
        }

        public List<Infraccion> ConsultarTodos()
        {
            return dbExamen.Infraccion.ToList();
        }

        //public List<Infraccion> ConsultarXTipo(int TipoProducto)
        //{
        //    return dbExamen.Infraccion
        //        .Where(p => p.CodigoTipoProducto == TipoProducto)
        //        .OrderBy(p => p.Nombre)
        //        .ToList();
        //}

        public IQueryable ConsultarImagenesXVehiculo(string placa)
        {
            return from V in dbExamen.Set<Vehiculo>()
                   join IN in dbExamen.Set<Infraccion>()
                   on V.Placa equals IN.PlacaVehiculo
                   join F in dbExamen.Set<FotoInfraccion>()
                   on IN.idFotoMulta equals F.idFoto
                   where V.Placa == placa
                   orderby F.NombreFoto
                   select new
                   {
                       placaVehiculo = V.Placa,
                       TipoVehiculo = V.TipoVehiculo,
                       marca = V.Marca,
                       color = V.Color,
                       tipoInfraccion = IN.TipoInfraccion,
                       fechaInfraccion = IN.FechaInfraccion,
                       Foto = F.NombreFoto 

                   };

        }

    }
}