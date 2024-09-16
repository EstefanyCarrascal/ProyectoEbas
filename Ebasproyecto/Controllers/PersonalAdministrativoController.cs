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
using System.Globalization;

namespace Ebasproyecto.Controllers
{
    public class PersonalAdministrativoController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");
        // GET: PersonalAdministrativo
        public ActionResult user()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<Usuarios>("Usuarios");
            List<Usuarios> List = collection.Find(d => d.TipoUsuario == "PersonalAdministrativo").ToList(); // Filtrar por TipoUsuario = "Administrativo"
            return View(List);
        }

        public ActionResult GenerarReporteAdministrativo()
        {
            // Configuración de la conexión a MongoDB
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("Ebas");
            var collection = database.GetCollection<Usuarios>("Usuarios");

            // Obtener todos los usuarios de la base de datos
            var usuarios = collection.Find(d => d.TipoUsuario == "PersonalAdministrativo").ToList(); // Filtrar por TipoUsuario = "Aprendiz"

            // Crear el documento PDF
            Document documentoPDF = new Document(PageSize.A4);
            MemoryStream stream = new MemoryStream();
            PdfWriter.GetInstance(documentoPDF, stream).CloseStream = false;

            documentoPDF.Open();

            // Título del documento
            Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            Paragraph titulo = new Paragraph("Reporte de Usuarios", tituloFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            documentoPDF.Add(titulo);
            documentoPDF.Add(new Paragraph("\n")); // Espacio

            // Crear tabla con 7 columnas para las propiedades de los usuarios
            PdfPTable tabla = new PdfPTable(7)
            {
                WidthPercentage = 100
            };
            tabla.SetWidths(new float[] { 2f, 2f, 2f, 2f, 3f, 1f, 2f }); // Ajuste de ancho de columnas

            // Encabezados de la tabla
            tabla.AddCell(new PdfPCell(new Phrase("Nombres", tituloFont)));
            tabla.AddCell(new PdfPCell(new Phrase("Apellidos", tituloFont)));
            tabla.AddCell(new PdfPCell(new Phrase("Documento", tituloFont)));
            tabla.AddCell(new PdfPCell(new Phrase("Teléfono", tituloFont)));
            tabla.AddCell(new PdfPCell(new Phrase("Correo", tituloFont)));
            tabla.AddCell(new PdfPCell(new Phrase("Sexo", tituloFont)));
            tabla.AddCell(new PdfPCell(new Phrase("Tipo Población", tituloFont)));

            // Agregar datos de cada usuario a la tabla
            foreach (var usuario in usuarios)
            {
                tabla.AddCell(usuario.Nombres ?? "N/A");
                tabla.AddCell(usuario.Apellidos ?? "N/A");
                tabla.AddCell(usuario.Documento ?? "N/A");
                tabla.AddCell(usuario.Telefono ?? "N/A");
                tabla.AddCell(usuario.Correo ?? "N/A");
                tabla.AddCell(usuario.Sexo ?? "N/A");
                tabla.AddCell(usuario.TipoPoblacion ?? "N/A");
            }

            // Añadir la tabla al documento PDF
            documentoPDF.Add(tabla);

            // Cerrar el documento
            documentoPDF.Close();

            // Devolver el PDF como archivo descargable
            stream.Position = 0;
            return File(stream, "application/pdf", "ReporteUsuarios.pdf");
        }



        [HttpPost]
        public ActionResult Crear(string Nombres, string Apellidos, string Documento, string TipoDocumento, string Correo, string Sexo, string Telefono, string TipoPoblacion, string TipoUsuario, string Contraseña)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Usuarios>("Usuarios"); // Consistencia en el nombre de la colección
                var Usuarios = new Usuarios
                {
                    Nombres = Nombres,
                    Apellidos = Apellidos,
                    Documento = Documento,
                    TipoDocumento = TipoDocumento,
                    Correo = Correo,
                    Sexo = Sexo,
                    Telefono = Telefono,
                    TipoPoblacion = TipoPoblacion,
                    TipoUsuario = "PersonalAdministrativo",
                    Contraseña = Contraseña
                };

                collection.InsertOne(Usuarios);
                return RedirectToAction("user"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("user", new { mensaje = "Error al insertar la sala." });
            }
        }

        [HttpPost]
        public ActionResult Editar(string objectId, string Nombres, string Apellidos, string Documento, string TipoDocumento, string Correo, string Sexo, string Telefono, string TipoPoblacion, string Contraseña)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Usuarios>("Usuarios"); // Consistencia en el nombre de la colección

                var Usuarios = new Usuarios
                {
                    Id = parsedObjectId, // Asegúrate de que la propiedad Id es del tipo ObjectId
                    Nombres = Nombres,
                    Apellidos = Apellidos,
                    Documento = Documento,
                    TipoDocumento = TipoDocumento,
                    Correo = Correo,
                    Sexo = Sexo,
                    Telefono = Telefono,
                    TipoPoblacion = TipoPoblacion,
                    Contraseña = Contraseña
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, Usuarios);
                return RedirectToAction("user"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("user", new { mensaje = ex.Message });
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
                var collection = database.GetCollection<Usuarios>("Usuarios"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("user"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("user", new { mensaje = "Error al eliminar la sala." });
            }
        }
        public ActionResult Asistencia()
        {
            var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
            var collection = database.GetCollection<RegistroAsistencia>("RegistroAsistencia"); // Consistencia en el nombre de la colección

            var asistencias = collection.Find(a => a.Asistio == true).ToList();



            var nombres = new List<dynamic>();

            foreach (var asistencia in asistencias)
            {
                var usuarioObjectId = MongoDB.Bson.ObjectId.Parse(asistencia.UsuarioId);
                var eventoObjectId = MongoDB.Bson.ObjectId.Parse(asistencia.EventoId);

                var aprendiz = collection.Find(u => u.Id == usuarioObjectId).FirstOrDefault();
            }
            return View(asistencias);
        }

        public ActionResult GenerarReporteAsistencia()
        {
            // Configuración de la conexión a MongoDB
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("Ebas");
            var asistenciaCollection = database.GetCollection<RegistroAsistencia>("RegistroAsistencia");

            // Obtener asistencias donde el usuario asistió
            var asistencias = asistenciaCollection.Find(a => a.Asistio == true).ToList();

            // Crear el documento PDF
            Document documentoPDF = new Document(PageSize.A4);
            MemoryStream stream = new MemoryStream();
            PdfWriter.GetInstance(documentoPDF, stream).CloseStream = false;

            documentoPDF.Open();

            // Título del documento
            Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Paragraph titulo = new Paragraph("Reporte de Asistencia", tituloFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            documentoPDF.Add(titulo);
            documentoPDF.Add(new Paragraph("\n")); // Espacio

            // Crear tabla con las columnas necesarias
            PdfPTable tabla = new PdfPTable(4) // Se eliminó la columna para la hora
            {
                WidthPercentage = 100
            };
            tabla.SetWidths(new float[] { 2f, 2f, 2f, 2f });

            // Encabezados de la tabla
            tabla.AddCell("Nombre del Usuario");
            tabla.AddCell("Evento");
            tabla.AddCell("Fecha de Asistencia");
            tabla.AddCell("Asistió");

            // Obtener la colección de usuarios y eventos
            var usuarioCollection = database.GetCollection<Usuarios>("Usuarios");
            var eventoCollection = database.GetCollection<Evento>("Evento");

            // Agregar datos de cada asistencia a la tabla
            foreach (var asistencia in asistencias)
            {
                var usuario = usuarioCollection.Find(u => u.Id == MongoDB.Bson.ObjectId.Parse(asistencia.UsuarioId)).FirstOrDefault();
                var evento = eventoCollection.Find(e => e.Id == MongoDB.Bson.ObjectId.Parse(asistencia.EventoId)).FirstOrDefault();

                // Asegúrate de que usuario y evento existan
                if (usuario != null && evento != null)
                {
                    tabla.AddCell(usuario.Nombres);  // Ajusta según el modelo de Usuarios
                    tabla.AddCell(evento.NombreEvento);    // Ajusta según el modelo de Eventos

                    // Fecha de asistencia
                    if (DateTime.TryParse(asistencia.Fecha, out DateTime fechaAsistencia))
                    {
                        tabla.AddCell(fechaAsistencia.ToString("dd/MM/yyyy")); // Mostrar solo la fecha
                    }
                    else
                    {
                        tabla.AddCell("Fecha no válida");
                    }

                    // Indicar si asistió
                    tabla.AddCell(asistencia.Asistio ? "Sí" : "No");
                }
            }

            // Añadir la tabla al documento PDF
            documentoPDF.Add(tabla);

            // Cerrar el documento
            documentoPDF.Close();

            // Devolver el PDF como archivo descargable
            stream.Position = 0;
            return File(stream, "application/pdf", "ReporteAsistencia.pdf");
        }
    }
}