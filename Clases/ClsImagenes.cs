using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Examen2.Models;

namespace Examen2.Clases
{
    public class ClsImagenes
    {

        private DBExamenEntities dbExamen = new DBExamenEntities();
        public HttpRequestMessage request { get; set; }
        public string Datos { get; set; }
        public string Proceso { get; set; }


        public async Task<HttpResponseMessage> GrabarArchivo(bool Actualizar)
        {
            string RptaError = "";

            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.UnsupportedMediaType);
            }
            string root = HttpContext.Current.Server.MapPath("~/Archivos");
            var provider = new MultipartFormDataStreamProvider(root); // permite gestionar la información que llega de internet.

            try
            {
                await request.Content.ReadAsMultipartAsync(provider);
                List<string> Archivos = new List<string>();
                // Para obtener el nombre del archivo
                foreach (MultipartFileData file in provider.FileData)
                {
                    string fileName = file.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") && fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    if (File.Exists(Path.Combine(root, fileName))) //Si el archivo existe
                    {
                        if (Actualizar)
                        {
                            //Se borra el original
                            File.Delete(Path.Combine(root, fileName));
                            //Se crea el archivo con el mismo nombre
                            File.Move(file.LocalFileName, Path.Combine(root, fileName));
                            //No se debe agregar en la base de datos, por que ya existe

                        }
                        else
                        {
                            File.Delete(file.LocalFileName);
                            RptaError += "El archivo: " + fileName + " ya existe";
                            // return request.CreateErrorResponse(HttpStatusCode.Conflict, "El archivo ya existe");

                        }

                    }
                    else
                    {
                        //Agregar el archivo a la lista de archivos
                        Archivos.Add(fileName);

                        // se renombra el archivo, No se pueden tener archivos con el mismo nombre
                        File.Move(file.LocalFileName, Path.Combine(root, fileName));

                    }


                }
                if (Archivos.Count > 0)
                {
                    //Envia a grabar la información de las imagenes
                    string Respuesta = ProcesarArchivos(Archivos);

                    return request.CreateResponse(HttpStatusCode.OK, "Archivo subido con exito");
                }
                else
                {
                    if (Actualizar)
                    {
                        return request.CreateResponse(HttpStatusCode.OK, "Archivo actualizado con éxito");

                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.Conflict, "El(los) archivo(s) ya existe(n)");
                    }

                }

            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al cargar el archivo: " + ex.Message);
            }

        }

        public HttpResponseMessage ConsultarArchivo(string NombreArchivo)
        {
            try
            {
                string Ruta = HttpContext.Current.Server.MapPath("~/Archivos");
                string Archivo = Path.Combine(Ruta, NombreArchivo);
                if (File.Exists(Archivo))
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(Archivo, FileMode.Open, FileAccess.Read);
                    response.Content = new StreamContent(stream);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = NombreArchivo;
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octect-stream");
                    return response;

                }
                else
                {
                    return request.CreateErrorResponse(HttpStatusCode.NotFound, "Archivo no encontrado");
                }

            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al consultar el archivo: " + ex.Message);
            }

        }

        private string ProcesarArchivos(List<string> Archivos)
        {
            switch (Proceso.ToUpper())
            {
                case "PRODUCTO":
                    //clsImagenesProducto imagenesProducto = new clsImagenesProducto();
                    //imagenesProducto.idProducto = Datos;
                    //imagenesProducto.Archivos = Archivos;

                    return GrabarImagenes(Archivos);

                default:
                    return "Proceso no válido";

            }
        }

        public string GrabarImagenes(List<string> files)
        {
            try
            {

                if (files.Count > 0)
                {
                    foreach (string archivo in files)
                    {
                        FotoInfraccion Imagen = new FotoInfraccion();
                        Imagen.idInfraccion = Convert.ToInt32(Datos);
                        Imagen.NombreFoto = archivo;
                        dbExamen.FotoInfraccion.Add(Imagen);
                        dbExamen.SaveChanges();
                    }
                    return "Imagenes guardadas correctamente";
                }
                else
                {
                    return "No se enviaron los archivos para guardar";
                }
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    
    }
}