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
            var collection = database.GetCollection<Usuarios>("Usuarios");
            List<Usuarios> List = collection.Find(d => d.TipoUsuario == "Aprendiz").ToList(); // Filtrar por TipoUsuario = "Aprendiz"
            return View(List);
        }
        public ActionResult GenerarReporteUsuarios()
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
        Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
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
        tabla.SetWidths(new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f });

        // Encabezados de la tabla
        tabla.AddCell("Nombres");
        tabla.AddCell("Apellidos");
        tabla.AddCell("Documento");
        tabla.AddCell("Teléfono");
        tabla.AddCell("Correo");
        tabla.AddCell("Sexo");
        tabla.AddCell("Edad");

        // Agregar datos de cada usuario a la tabla
        foreach (var usuario in usuarios)
        {
            tabla.AddCell(usuario.Nombres);
            tabla.AddCell(usuario.Apellidos);
            tabla.AddCell(usuario.Documento);
            tabla.AddCell(usuario.Telefono);
            tabla.AddCell(usuario.Correo);
            tabla.AddCell(usuario.Sexo);
            tabla.AddCell(usuario.Edad);
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
        public ActionResult Crear(string Nombres, string Apellidos, string Documento, string TipoDocumento, string Correo, string Sexo, string Edad, string Municipio, string Direccion, string EstadoCivil, string Telefono, string TipoPoblacion, string TipoUsuario, string Contraseña)
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
                    Edad = Edad,
                    Municipio = Municipio,
                    Direccion = Direccion,
                    EstadoCivil = EstadoCivil,
                    Telefono = Telefono,
                    TipoPoblacion = TipoPoblacion,
                    TipoUsuario = TipoUsuario,
                    Contraseña = Contraseña
                };

                collection.InsertOne(Usuarios);
                return RedirectToAction("Aprendices"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Aprendices", new { mensaje = "Error al insertar la sala." });
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
                    Edad = Edad,
                    Municipio = Municipio,
                    Direccion = Direccion,
                    EstadoCivil = EstadoCivil,
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