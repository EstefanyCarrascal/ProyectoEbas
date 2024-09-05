using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Ebasproyecto.Model;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace Ebasproyecto.Controllers
{
    public class FichasController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");
        // GET: Fichas
        public ActionResult Index()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<Fichas>("Fichas");
            List<Fichas> List = collection.Find(d => true).ToList();
            return View(List);
        }

        public ActionResult GenerarReporteFichas()
        {
            // Configuración de la conexión a MongoDB
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("Ebas");
            var collection = database.GetCollection<Fichas>("Fichas");

            // Obtener todas las fichas de la base de datos
            var fichas = collection.Find(_ => true).ToList();

            // Crear el documento PDF
            Document documentoPDF = new Document(PageSize.A4);
            MemoryStream stream = new MemoryStream();
            PdfWriter.GetInstance(documentoPDF, stream).CloseStream = false;

            documentoPDF.Open();

            // Título del documento
            Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Paragraph titulo = new Paragraph("Reporte de Fichas", tituloFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            documentoPDF.Add(titulo);
            documentoPDF.Add(new Paragraph("\n")); // Espacio

            // Crear tabla con 6 columnas para las propiedades de las fichas
            PdfPTable tabla = new PdfPTable(6)
            {
                WidthPercentage = 100
            };
            tabla.SetWidths(new float[] { 2f, 2f, 2f, 2f, 2f, 2f });

            // Encabezados de la tabla
            tabla.AddCell("Código");
            tabla.AddCell("Jornada");
            tabla.AddCell("Modalidad");
            tabla.AddCell("Tipo");
            tabla.AddCell("Fecha Inicio");
            tabla.AddCell("Fecha Fin");

            // Agregar datos de cada ficha a la tabla
            foreach (var ficha in fichas)
            {
                tabla.AddCell(ficha.Codigo);
                tabla.AddCell(ficha.Jornada);
                tabla.AddCell(ficha.Modalidad);
                tabla.AddCell(ficha.Tipo);
                tabla.AddCell(ficha.FechaInicio);
                tabla.AddCell(ficha.FechaFin);
            }

            // Añadir la tabla al documento PDF
            documentoPDF.Add(tabla);

            // Cerrar el documento
            documentoPDF.Close();

            // Devolver el PDF como archivo descargable
            stream.Position = 0;
            return File(stream, "application/pdf", "ReporteFichas.pdf");
        }

        [HttpPost]
        public ActionResult Crear(string Codigo, string Tipo, string Jornada, string Modalidad, string FechaInicio, string FechaFin)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Fichas>("Fichas"); // Consistencia en el nombre de la colección
                var Fichas = new Fichas
                {
                    Codigo = Codigo,
                    Tipo = Tipo,
                    Jornada = Jornada,
                    Modalidad = Modalidad,
                    FechaInicio = FechaInicio,
                    FechaFin = FechaFin
                };

                collection.InsertOne(Fichas);
                return RedirectToAction("Index"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Index", new { mensaje = "Error al insertar la sala." });
            }
        }

        [HttpPost]
        public ActionResult Editar(string objectId, string Codigo, string Tipo, string Jornada, string Modalidad, string FechaInicio, string FechaFin)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Fichas>("Fichas"); // Consistencia en el nombre de la colección

                var Fichas = new Fichas
                {
                    Id = parsedObjectId,
                    Codigo = Codigo,
                    Tipo = Tipo,
                    Jornada = Jornada,
                    Modalidad = Modalidad,
                    FechaInicio = FechaInicio,
                    FechaFin = FechaFin
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, Fichas);
                return RedirectToAction("Index"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Index", new { mensaje = ex.Message });
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
                var collection = database.GetCollection<Fichas>("Fichas"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("Index"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Index", new { mensaje = "Error al eliminar la sala." });
            }
        }
    }
}