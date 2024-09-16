using Ebasproyecto.Model;
using iTextSharp.text.pdf;
using iTextSharp.text;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ebasproyecto.Controllers
{
    public class ProgramaFormacionController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");

        // GET: ProgramaFormacion
        public ActionResult Programa()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion");
            List<ProgramaFormacion> List = collection.Find(d => true).ToList();
            return View(List);
        }
        public ActionResult GenerarReporteProgramas()
        {
            // Configuración de la conexión a MongoDB
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("Ebas");
            var collection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion");

            // Obtener todos los programas de la base de datos
            var programas = collection.Find(_ => true).ToList();

            // Crear el documento PDF
            Document documentoPDF = new Document(PageSize.A4);
            MemoryStream stream = new MemoryStream();
            PdfWriter.GetInstance(documentoPDF, stream).CloseStream = false;

            documentoPDF.Open();

            // Título del documento
            Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Paragraph titulo = new Paragraph("Reporte de Programas de Formación", tituloFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            documentoPDF.Add(titulo);
            documentoPDF.Add(new Paragraph("\n")); // Espacio

            // Crear tabla con 4 columnas para las propiedades del programa de formación
            PdfPTable tabla = new PdfPTable(4)
            {
                WidthPercentage = 100
            };
            tabla.SetWidths(new float[] { 2f, 3f, 2f, 2f });

            // Encabezados de la tabla
            tabla.AddCell("Código");
            tabla.AddCell("Nombre");
            tabla.AddCell("Tipo");
            tabla.AddCell("Duración");

            // Agregar datos de cada programa a la tabla
            foreach (var programa in programas)
            {
                tabla.AddCell(programa.Codigo);
                tabla.AddCell(programa.Nombre);
                tabla.AddCell(programa.Tipo);
                tabla.AddCell(programa.Duracion);
            }

            // Añadir la tabla al documento PDF
            documentoPDF.Add(tabla);

            // Cerrar el documento
            documentoPDF.Close();

            // Devolver el PDF como archivo descargable
            stream.Position = 0;
            return File(stream, "application/pdf", "ReporteProgramasFormacion.pdf");
        }


        [HttpPost]
        public ActionResult Crear(string Codigo,string Nombre, string Tipo, string Duracion)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion"); // Consistencia en el nombre de la colección
                var ProgramaFormacion = new ProgramaFormacion
                {
                    Codigo = Codigo,
                    Tipo = Tipo,
                    Nombre = Nombre,
                    Duracion = Duracion,
                };

                collection.InsertOne(ProgramaFormacion);
                return RedirectToAction("Programa"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Programa", new { mensaje = "Error al insertar la sala." });
            }

        }


        [HttpPost]
        public ActionResult Editar(string objectId, string Codigo, string Nombre, string Tipo, string Duracion)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion"); // Consistencia en el nombre de la colección

                var ProgramaFormacion = new ProgramaFormacion
                {
                    Id = parsedObjectId,
                    Codigo = Codigo,
                    Tipo = Tipo,
                    Nombre = Nombre,
                    Duracion = Duracion,
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, ProgramaFormacion);
                return RedirectToAction("Programa"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Programa", new { mensaje = ex.Message });
            }
        }
        public ActionResult Eliminar(string objectId)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("Programa"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Programa", new { mensaje = "Error al eliminar la sala." });
            }
        }

    }
}