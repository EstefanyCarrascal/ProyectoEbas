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
    public class EventoController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");
        // GET: Evento
        public ActionResult Evento()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<Evento>("Evento");
            List<Evento> List = collection.Find(d => true).ToList();
            return View(List);
        }
        public ActionResult GenerarReporteEventos()
        {
            // Configuración de la conexión a MongoDB
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("Ebas");
            var collection = database.GetCollection<Evento>("Evento");

            // Obtener todos los eventos de la base de datos
            var eventos = collection.Find(_ => true).ToList();

            // Crear el documento PDF
            Document documentoPDF = new Document(PageSize.A4);
            MemoryStream stream = new MemoryStream();
            PdfWriter.GetInstance(documentoPDF, stream).CloseStream = false;

            documentoPDF.Open();

            // Título del documento
            Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Paragraph titulo = new Paragraph("Reporte de Eventos", tituloFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            documentoPDF.Add(titulo);
            documentoPDF.Add(new Paragraph("\n")); // Espacio

            // Crear tabla con 6 columnas para las propiedades del evento
            PdfPTable tabla = new PdfPTable(6)
            {
                WidthPercentage = 100
            };
            tabla.SetWidths(new float[] { 3f, 2f, 2f, 2f, 2f, 2f });

            // Encabezados de la tabla
            tabla.AddCell("Nombre del Evento");
            tabla.AddCell("Descripción");
            tabla.AddCell("Fecha");
            tabla.AddCell("Organizador");
            tabla.AddCell("Hora Inicio");
            tabla.AddCell("Hora Fin");

            // Agregar datos de cada evento a la tabla
            foreach (var evento in eventos)
            {
                tabla.AddCell(evento.NombreEvento);
                tabla.AddCell(evento.Descripcion);
                tabla.AddCell(evento.FechaEvento);
                tabla.AddCell(evento.Organizador);
                tabla.AddCell(evento.HoraInicio);
                tabla.AddCell(evento.HoraFin);
            }

            // Añadir la tabla al documento PDF
            documentoPDF.Add(tabla);

            // Cerrar el documento
            documentoPDF.Close();

            // Devolver el PDF como archivo descargable
            stream.Position = 0;
            return File(stream, "application/pdf", "ReporteEventos.pdf");
        }

        [HttpPost]
        public ActionResult Crear(string NombreEvento, string Descripcion, string Organizador, string FechaEvento, string HoraInicio, string HoraFin)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Evento>("Evento"); // Consistencia en el nombre de la colección
                var Evento = new Evento
                {
                    NombreEvento = NombreEvento,
                    Descripcion = Descripcion,
                    Organizador = Organizador,
                    FechaEvento = FechaEvento,
                    HoraInicio = HoraInicio,
                    HoraFin = HoraFin
                };

                collection.InsertOne(Evento);
                return RedirectToAction("Evento"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Evento", new { mensaje = "Error al insertar la sala." });
            }

        }
        [HttpPost]
        public ActionResult Editar(string objectId, string NombreEvento, string Descripcion, string Organizador, string FechaEvento, string HoraInicio, string HoraFin)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Evento>("Evento"); // Consistencia en el nombre de la colección

                var Evento = new Evento
                {
                    Id = parsedObjectId,
                    NombreEvento = NombreEvento,
                    Descripcion = Descripcion,
                    Organizador = Organizador,
                    FechaEvento = FechaEvento,
                    HoraInicio = HoraInicio,
                    HoraFin = HoraFin
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, Evento);
                return RedirectToAction("Evento"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Evento", new { mensaje = ex.Message });
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
                var collection = database.GetCollection<Evento>("Evento"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("Evento"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Evento", new { mensaje = "Error al eliminar la sala." });
            }
        }
    }
}