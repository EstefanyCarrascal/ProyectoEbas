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
using System.Net;
using Newtonsoft.Json;

namespace Ebasproyecto.Controllers
{
    public class AprendicesController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");

        // GET: Aprendices
        public ActionResult Aprendices()
        {
            var database = cn.GetDatabase("Ebas");
            var usuariosCollection = database.GetCollection<Usuarios>("Usuarios");
            List<Usuarios> usuariosList = usuariosCollection.Find(d => d.TipoUsuario == "Aprendiz").ToList();

            var fichasCollection = database.GetCollection<Fichas>("Fichas");
            var fichasList = fichasCollection.Find(d => true).ToList();
            var fichasDictionary = fichasList.ToDictionary(f => f.Id.ToString(), f => f.Codigo);
            ViewBag.Fichas = new SelectList(fichasList, "Id", "Codigo");

            var programasCollection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion");
            var programasList = programasCollection.Find(d => true).ToList();
            var programasDictionary = programasList.ToDictionary(p => p.Id.ToString(), p => p.Nombre);
            ViewBag.Programas = new SelectList(programasList, "Id", "Nombre");

            // Map the ficha code and program name to the users
            var result = usuariosList.Select(u => new Usuarios
            {
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Documento = u.Documento,
                TipoDocumento = u.TipoDocumento,
                Correo = u.Correo,
                Telefono = u.Telefono,
                Sexo = u.Sexo,
                TipoPoblacion = u.TipoPoblacion,
                TipoUsuario = u.TipoUsuario,
                Contraseña = u.Contraseña,
                CodigoFicha = fichasDictionary.ContainsKey(u.FichaId) ? fichasDictionary[u.FichaId] : "No asignado",
                ProgramaNombre = programasDictionary.ContainsKey(u.ProgramaId) ? programasDictionary[u.ProgramaId] : "No asignado"
            }).ToList();

            return View(result);
        }


        public ActionResult GenerarReporteAprendices()
        {
            // Configuración de la conexión a MongoDB
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("Ebas");
            var collection = database.GetCollection<Usuarios>("Usuarios");

            // Obtener todos los usuarios de la base de datos
            var usuarios = collection.Find(d => d.TipoUsuario == "Aprendiz").ToList(); // Filtrar por TipoUsuario = "Aprendiz"

            // Crear el documento PDF
            Document documentoPDF = new Document(PageSize.A4);
            MemoryStream stream = new MemoryStream();
            PdfWriter.GetInstance(documentoPDF, stream).CloseStream = false;

            documentoPDF.Open();

            // Título del documento
            Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
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
        public ActionResult Crear(string Nombres, string Apellidos, string Documento, string TipoDocumento, string Correo, string Sexo, string Telefono, string TipoPoblacion, string TipoUsuario, string Contraseña, string Codigoficha, string ProgramaId)
        {
            try
            {
                var database = cn.GetDatabase("Ebas");
                var collection = database.GetCollection<Usuarios>("Usuarios");
                var usuario = new Usuarios
                {
                    Nombres = Nombres,
                    Apellidos = Apellidos,
                    Documento = Documento,
                    TipoDocumento = TipoDocumento,
                    Correo = Correo,
                    Sexo = Sexo,
                    Telefono = Telefono,
                    TipoPoblacion = TipoPoblacion,
                    TipoUsuario = "Aprendiz",
                    Contraseña = Contraseña,
                    FichaId = Codigoficha,
                    ProgramaId = ProgramaId // Asociar el programa de formación
                };

                collection.InsertOne(usuario);
                return RedirectToAction("Aprendices");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Aprendices", new { mensaje = "Error al insertar el usuario." });
            }
        }
        [HttpPost]
        public ActionResult Editar(string objectId, string Nombres, string Apellidos, string Documento, string TipoDocumento, string Correo, string Sexo, string Edad, string Municipio, string Direccion, string EstadoCivil, string Telefono, string TipoPoblacion, string TipoUsuario, string Contraseña)
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
                    TipoUsuario = TipoUsuario,
                    Contraseña = Contraseña
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, Usuarios);
                return RedirectToAction("Aprendices"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Aprendices", new { mensaje = ex.Message });
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
                return RedirectToAction("Aprendices"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Aprendices", new { mensaje = "Error al eliminar la sala." });
            }
        }
    }
}